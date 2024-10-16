import React from 'react';
import './ConfirmationModal.css'; 
const ConfirmationModal = ({ isOpen, onClose, onConfirm }) => {
    if (!isOpen) return null; 

    return (
        <div className="confirmation-modal">
            <div className="confirmation-modal-content">
                <h2>Confirm Reservation</h2>
                <p>Are you sure you want to reserve this book?</p>
                <div className="confirmation-modal-buttons">
                    <button onClick={onConfirm}>Yes, Reserve</button>
                    <button onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
};

export default ConfirmationModal;
