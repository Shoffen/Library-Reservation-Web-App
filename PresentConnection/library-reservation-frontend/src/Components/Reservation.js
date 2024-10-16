import React, { useState, useEffect } from 'react';
import './Reservation.css';
import ConfirmationModal from './ConfirmationModal';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

const Reservation = ({ isOpen, onClose, selectedBook }) => {
    const [audioModal, setAudioModalOpen] = useState(false);
    const [physicalModal, setPhysicalModalOpen] = useState(false);
    const [dateRange, setDateRange] = useState([null, null]);
    const [price, setPrice] = useState(0);
    const [quickPickup, setQuickPickup] = useState(false);
    const [isConfirmationOpen, setConfirmationOpen] = useState(false);
    const [daysCount, setDaysCount] = useState(0);
    const serviceFee = 3;
    const quickPickupCost = 5;

    
    useEffect(() => {
        const count = dateRange[0] && dateRange[1]
            ? Math.ceil((dateRange[1] - dateRange[0]) / (1000 * 60 * 60 * 24))
            : 0;
        setDaysCount(dateRange[0] ? count + 1 : 0);
    }, [dateRange]);

    
    useEffect(() => {
        if (isOpen) {
            calculateReservationPrice(dateRange, quickPickup);
        }
    }, [isOpen, audioModal, physicalModal, quickPickup, dateRange]);

    if (!isOpen) return null;

    const handleClose = () => {
        onClose();
        resetStates();
    };

    const resetStates = () => {
        setAudioModalOpen(false);
        setPhysicalModalOpen(false);
        setQuickPickup(false);
        setDateRange([null, null]);
        setPrice(0);
        setDaysCount(0);
    };

    const openAudioModal = () => {
        setAudioModalOpen(true);
        setPhysicalModalOpen(false);
    };

    const openPhysicalModal = () => {
        setPhysicalModalOpen(true);
        setAudioModalOpen(false);
    };

    const getQuickPickup = (event) => {
        setQuickPickup(event.target.checked);
    };

    const calculateReservationPrice = async (dates, isQuickPickup) => {
        const isAudiobook = audioModal;
        const daysCount = dates[0] && dates[1] ? Math.ceil((dates[1] - dates[0]) / (1000 * 60 * 60 * 24)) : 0;

        try {
            const queryParams = new URLSearchParams({
                days: dates[1] ? (daysCount + 1).toString() : 0,
                isAudiobook: isAudiobook.toString(),
                quickPickup: isQuickPickup.toString(),
                serviceFee: serviceFee.toString(),
                pickupCost: quickPickupCost.toString()
            });

            const response = await fetch(`http://localhost:5281/api/reservations/calculatePrice?${queryParams.toString()}`);

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

        const isAudiobook = audioModal;
        const reservationData = {
            bookId: selectedBook.id,
            startDate: dateRange[0].toISOString(), 
            endDate: dateRange[1].toISOString(),   
            isQuickPickup: quickPickup,
            reservationCost: price,
            ImageUrl: selectedBook.imageUrl,
            isAudiobook: audioModal
        };

        try {
            const response = await fetch('http://localhost:5281/api/reservations', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(reservationData),
            });

            if (!response.ok) {
                const errorDetails = await response.text(); 
                throw new Error(`Error creating reservation: ${errorDetails}`);
            }

            const newReservation = await response.json();
            console.log('Reservation created:', newReservation);
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
                                    <button type="button" onClick={openAudioModal}>
                                        <span className="audio-indicator">🎧 Audiobook (2€ a day)</span>
                                    </button>
                                )}
                                {selectedBook.physicalBook && (
                                    <button type="button" onClick={openPhysicalModal}>
                                        <span className="physical-indicator">📚 Physical Book (3€ a day)</span>
                                    </button>
                                )}
                            </div>

                            {(audioModal || physicalModal) && (
                                <div className="date-picker-container">
                                    <p>Select reservation dates:</p>
                                    <DatePicker
                                        selected={dateRange[0]}
                                        onChange={(update) => {
                                            setDateRange(update);
                                        }}
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
                            {audioModal && (
                                <div className="modal-content-details">
                                    <h3>Audiobook Selected</h3>
                                    <p>Price: 2€ per day</p>
                                </div>
                            )}

                            {physicalModal && (
                                <div className="modal-content-details">
                                    <h3>Physical Book Selected</h3>
                                    <p>Price: 3€ per day</p>
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={quickPickup}
                                            onChange={getQuickPickup}
                                        />
                                        Quick pickup (5€)
                                    </label>
                                </div>
                            )}
                            <div className="price-display">
                                {dateRange[0] && dateRange[1] && (
                                    <div className="price-breakdown">
                                        <h4>Price Breakdown:</h4>
                                        <p>Daily Rate: {audioModal ? '2€' : '3€'} x {daysCount} days</p>
                                        <p>Subtotal: {((audioModal ? 2 : 3) * daysCount).toFixed(2)} €</p>
                                        {quickPickup && <p>Quick Pickup Fee: {quickPickupCost} €</p>}
                                        {daysCount >= 3 && daysCount < 10 && (
                                            <div className="discount-section">
                                                <p>Discount 10%: {((audioModal ? 2 : 3) * daysCount * 0.1).toFixed(2)} €</p>
                                            </div>
                                        )}
                                        {daysCount >= 10 && (
                                            <div className="discount-section">
                                                <p>Discount 20%: {((audioModal ? 2 : 3) * daysCount * 0.2).toFixed(2)} €</p>
                                            </div>
                                        )}
                                        <p>Service Fee: {serviceFee} €</p>
                                    </div>
                                )}
                            </div>
                            <div className="total-price-container">
                                {dateRange[0] && dateRange[1] && (
                                    <>
                                        <label className="total-price-label">Total Price: {price.toFixed(2)} €</label>
                                        <button className="reserve-button" onClick={handleReserve}>Reserve</button>
                                    </>
                                )}
                            </div>
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

export default Reservation;
