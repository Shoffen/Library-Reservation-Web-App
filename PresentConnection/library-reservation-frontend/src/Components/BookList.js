import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import './BookList.css'; 
import Reservation from './Reservation';

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [error, setError] = useState('');
    const [filters, setFilters] = useState({
        searchTerm: '',
        searchYear: '',
        showAudiobooks: true,
        showPhysicalBooks: true,
    });
    const [isModalOpen, setIsModalOpen] = useState(false); 
    const [selectedBook, setSelectedBook] = useState(null); 

    useEffect(() => {
        const fetchBooks = async () => {
            try {
                const queryParams = new URLSearchParams(filters);
                const response = await fetch(`http://localhost:5274/api/book?${queryParams.toString()}`);

                if (!response.ok) {
                    throw new Error('Failed to fetch books');
                }

                const data = await response.json();
                console.log("Fetched books:", data); 
                setBooks(data);
            } catch (err) {
                setError(err.message); 
            }
        };

        fetchBooks();
    }, [filters]); 

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFilters(prevFilters => ({
            ...prevFilters,
            [name]: value,
        }));
    };

    const toggleFilter = (filterName) => {
        setFilters(prevFilters => ({
            ...prevFilters,
            [filterName]: !prevFilters[filterName],
        }));
    };

    const openModal = (book) => {
        setSelectedBook(book); 
        setIsModalOpen(true);  
    };

    const closeModal = () => {
        setIsModalOpen(false); 
        setSelectedBook(null); 
    };

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div className="book-list">
            <div className="search-container">
                <input
                    type="text"
                    name="searchTerm"
                    placeholder="Search for a book..."
                    value={filters.searchTerm}
                    onChange={handleInputChange}
                    className="search-input"
                />
                <input
                    type="text"
                    name="searchYear"
                    placeholder="Search for a book year"
                    value={filters.searchYear}
                    onChange={handleInputChange}
                    className="search-input"
                />
                <label>
                    <input
                        type="checkbox"
                        checked={filters.showAudiobooks}
                        onChange={() => toggleFilter('showAudiobooks')}
                    />
                    Show Audiobooks
                </label>
                <label>
                    <input
                        type="checkbox"
                        checked={filters.showPhysicalBooks}
                        onChange={() => toggleFilter('showPhysicalBooks')}
                    />
                    Show Physical Books
                </label>
                
                <Link to="/reservations">
                    <button className="reservations-button">
                        My Reservations
                    </button>
                </Link>
            </div>

            <h2>Book Library</h2>
            <div className="book-grid">
                {books.map(book => (
                    <div className="book-item" key={book.id}>
                        <button type="button" onClick={() => openModal(book)}>
                            <img src={book.imageUrl} alt={book.name} className="book-image" />
                        </button>
                        <div className="book-title">{book.name}</div>
                        <div className="book-year">{book.year}</div>
                        <div className="book-type">
                            {book.audiobook && <span className="audio-indicator">🎧 Audiobook</span>}
                            {book.physicalBook && <span className="physical-indicator">📚 Physical Book</span>}
                        </div>
                    </div>
                ))}
            </div>

            {books.length === 0 && <p>No books found.</p>}

            <Reservation 
                isOpen={isModalOpen} 
                onClose={closeModal} 
                selectedBook={selectedBook} 
            />
        </div>
    );
};

export default BookList;
