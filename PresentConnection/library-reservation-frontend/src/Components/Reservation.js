import React, { useState, useEffect } from 'react';
import './Reservation.css';
import ConfirmationModal from './ConfirmationModal';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

const serviceFee = 3;
const quickPickupCost = 5;
const audiobookRate = 2;
const physicalBookRate = 3;

const Reservation = ({ isOpen, onClose, selectedBook }) => {
    const [modalType, setModalType] = useState(null); // Use a single state for modals
    const [dateRange, setDateRange] = useState([null, null]);
    const [quickPickup, setQuickPickup] = useState(false);
    const [isConfirmationOpen, setConfirmationOpen] = useState(false);
    const [daysCount, setDaysCount] = useState(0);
    const [price, setPrice] = useState(0);

    useEffect(() => {
        if (dateRange[0] && dateRange[1]) {
            const count = Math.ceil((dateRange[1] - dateRange[0]) / (1000 * 60 * 60 * 24)) + 1;
            setDaysCount(count);
        } else {
            setDaysCount(0);
        }
    }, [dateRange]);

    useEffect(() => {
        if (isOpen) {
            calculateReservationPrice();
        }
    }, [isOpen, modalType, quickPickup, dateRange]);

    const resetStates = () => {
        setModalType(null);
        setQuickPickup(false);
        setDateRange([null, null]);
        setPrice(0);
        setDaysCount(0);
    };

    const handleClose = () => {
        resetStates();
        onClose();
    };

    const toggleModal = (type) => {
        setModalType(type);
    };

    const handleQuickPickupChange = (event) => {
        setQuickPickup(event.target.checked);
    };

    const calculateReservationPrice = async () => {
        const daysCount = dateRange[0] && dateRange[1] ? Math.ceil((dateRange[1] - dateRange[0]) / (1000 * 60 * 60 * 24)) : 0;
        const isAudiobook = modalType === 'audio';

        try {
            const response = await fetch(`http://localhost:5274/api/reservations/calculatePrice?${new URLSearchParams({
                days: daysCount + 1,
                isAudiobook: isAudiobook.toString(),
                quickPickup: quickPickup.toString(),
                serviceFee: serviceFee.toString(),
                pickupCost: quickPickupCost.toString(),
            })}`);

            if (!response.ok) throw new Error('Network response was not ok');

            const totalPrice = await response.json();
            setPrice(totalPrice);
        } catch (error) {
            console.error('Error fetching reservation price:', error);
        }
    };

    const handleReserve = () => {
        setConfirmationOpen(true);
    };

    const handleConfirm = async () => {
        if (!selectedBook) {
            console.error('No selected book available.');
            return;
        }

        const reservationData = {
            BookId: selectedBook.id,
            StartDate: dateRange[0].toISOString(),
            EndDate: dateRange[1].toISOString(),
            IsQuickPickup: quickPickup,
            ReservationCost: price,
            ImageUrl: selectedBook.imageUrl,
            IsAudiobook: modalType === 'audio',
        };

        try {
            const response = await fetch('http://localhost:5274/api/reservations', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(reservationData),
            });

            if (!response.ok) {
                const errorDetails = await response.text();
                throw new Error(`Error creating reservation: ${errorDetails}`);
            }

            setConfirmationOpen(false);
            handleClose();
        } catch (error) {
            console.error('Error submitting reservation:', error);
        }
    };

    const formatDateRange = (dates) => {
        if (!dates[0] || !dates[1]) return '';
        const start = dates[0].toLocaleDateString('en-GB');
        const end = dates[1].toLocaleDateString('en-GB');
        return `${start} - ${end}`;
    };

    if (!isOpen) return null;

    return (
        <div id="myModal" className="modal">
            <div className="modal-content">
                <span className="close" onClick={handleClose}>&times;</span>
                {selectedBook && (
                    <div className="reservation-layout">
                        <div className="reservation-options">
                            <h2>Reserve {selectedBook.name}</h2>
                            <img src={selectedBook.imageUrl} alt={selectedBook.name} className="reserve-image" />
                            <p>Year: {selectedBook.year}</p>
                            <div className="book-selection">
                                {selectedBook.audiobook && (
                                    <button type="button" onClick={() => toggleModal('audio')}>
                                        <span className="audio-indicator">🎧 Audiobook ({audiobookRate}€ a day)</span>
                                    </button>
                                )}
                                {selectedBook.physicalBook && (
                                    <button type="button" onClick={() => toggleModal('physical')}>
                                        <span className="physical-indicator">📚 Physical Book ({physicalBookRate}€ a day)</span>
                                    </button>
                                )}
                            </div>
                            {(modalType) && (
                                <div className="date-picker-container">
                                    <p>Select reservation dates:</p>
                                    <DatePicker
                                        selected={dateRange[0]}
                                        onChange={setDateRange}
                                        startDate={dateRange[0]}
                                        endDate={dateRange[1]}
                                        selectsRange
                                        inline
                                        dateFormat="yyyy/MM/dd"
                                        isClearable
                                        minDate={new Date()}
                                        popperPlacement="bottom"
                                    />
                                </div>
                            )}
                        </div>
                        <div className="reservation-summary">
                            <h2>Reservation Summary</h2>
                            {modalType && (
                                <SummaryDetails title={`${modalType === 'audio' ? 'Audiobook' : 'Physical Book'} Selected`} price={modalType === 'audio' ? audiobookRate : physicalBookRate}>
                                    {modalType === 'physical' && (
                                        <label>
                                            <input
                                                type="checkbox"
                                                checked={quickPickup}
                                                onChange={handleQuickPickupChange}
                                            />
                                            Quick pickup ({quickPickupCost}€)
                                        </label>
                                    )}
                                </SummaryDetails>
                            )}
                            <PriceBreakdown
                                daysCount={daysCount}
                                quickPickup={quickPickup}
                                modalType={modalType}
                            />
                            {dateRange[0] && dateRange[1] && (
                                <div className="total-price-container">
                                    <label className="total-price-label">Total Price: {price.toFixed(2)} €</label>
                                    <button className="reserve-button" onClick={handleReserve}>Reserve</button>
                                </div>
                            )}
                        </div>
                    </div>
                )}
                <ConfirmationModal
                    isOpen={isConfirmationOpen}
                    onClose={() => setConfirmationOpen(false)}
                    onConfirm={handleConfirm}
                />
            </div>
        </div>
    );
};

const SummaryDetails = ({ title, price, children }) => (
    <div className="modal-content-details">
        <h3>{title}</h3>
        <p>Price: {price}€ per day</p>
        {children}
    </div>
);

const PriceBreakdown = ({ daysCount, quickPickup, modalType }) => {
    const rate = modalType === 'audio' ? audiobookRate : physicalBookRate;
    const subtotal = (rate * daysCount).toFixed(2);
    const discount = (daysCount >= 3 && daysCount < 10) ? (subtotal * 0.1).toFixed(2) : 
                     (daysCount >= 10) ? (subtotal * 0.2).toFixed(2) : 0;

    const total = parseFloat(subtotal) + serviceFee + (quickPickup ? quickPickupCost : 0) - discount;

    return (
        <div className="price-display">
            {daysCount > 0 && (
                <div className="price-breakdown">
                    <h4>Price Breakdown:</h4>
                    <p>Daily Rate: {rate}€ x {daysCount} days</p>
                    <p>Subtotal: {subtotal} €</p>
                    {quickPickup && <p>Quick Pickup Fee: {quickPickupCost} €</p>}
                    {discount > 0 && (
                        <div className="discount-section">
                            <p>Discount: {discount} €</p>
                        </div>
                    )}
                    <p>Service Fee: {serviceFee} €</p>
                </div>
            )}
        </div>
    );
};

export default Reservation;
