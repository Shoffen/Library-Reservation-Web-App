import React from 'react';
import './App.css';
import BookList from './Components/BookList'; 
import ReservationList from './Components/ReservationList'; 
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

function App() {
    return (
        <Router>
            <div className="App">
                <Routes>
                    <Route path="/" element={<BookList />} />
                    <Route path="/reservations" element={<ReservationList />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
