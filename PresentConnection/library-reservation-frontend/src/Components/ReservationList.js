import React, { useEffect, useState } from 'react';
import './ReservationList.css'; 
import { Link } from 'react-router-dom'; 

const ReservationList = () => {
    const [reservations, setReservations] = useState([]);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchReservations = async () => {
            try {
                const response = await fetch(`http://localhost:5274/api/reservations`);

                if (!response.ok) {
                    throw new Error('Failed to fetch reservations');
                }

                const data = await response.json();
                setReservations(data);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchReservations();
    }, []);

    return (
        <div className="reservation-list-container"> 
            <div className="reservation-content"> 
                <h2>My Reservations</h2>
                <div className="button-container">
                    <Link to="/">
                        <button className="button"> Book Library </button>
                    </Link>
                </div>
                {error && <p>Error: {error}</p>}
                {reservations.length === 0 ? (
                    <div className="no-reservations-message">No reservations found.</div>
                ) : (
                    <div>
                        {reservations.map(reservation => (
                            <div className="reservation-card" key={reservation.id}>
                                <img src={reservation.imageUrl} alt={reservation.imageUrl} className="book-image" />
                                <div className="card-details">

                                    {reservation.isAudiobook ? (
                                        <span className="audio-indicator">🎧 Audiobook</span>
                                    ) : (
                                        <>
                                            <span className="physical-indicator">📚 Physical Book</span>
                                            {reservation.isQuickPickup ? (
                                                <span className="quick-pickup-indicator">🚀 Quick Pickup</span>
                                            ): <span className="quick-pickup-indicator">❌ No Quick Pickup</span>}
                                        </>
                                    )}
                                    <div className="card-title">{reservation.bookName}</div> 
                                    <div className="card-date">
                                        Start Date: {new Date(reservation.startDate).toLocaleDateString()} <br />
                                        End Date: {new Date(reservation.endDate).toLocaleDateString()} 
                                    </div>
                                    <div className="card-cost">Total Cost: {reservation.reservationCost.toFixed(2)} €</div>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default ReservationList;
