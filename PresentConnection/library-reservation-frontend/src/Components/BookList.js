import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import './BookList.css'; 
import Reservation from './Reservation';

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [error, setError] = useState('');
    const [searchTerm, setSearchTerm] = useState(''); 
    const [searchYear, setSearchYear] = useState(''); 
    const [showAudiobooks, setShowAudiobooks] = useState(true); 
    const [showPhysicalBooks, setShowPhysicalBooks] = useState(true); 
    const [isModalOpen, setIsModalOpen] = useState(false); 
    const [selectedBook, setSelectedBook] = useState(null); 

    useEffect(() => {
        const fetchBooks = async () => {
            try {
               
                const queryParams = new URLSearchParams({
                    searchTerm: searchTerm || '',
                    searchYear: searchYear || '',
                    showAudiobooks: showAudiobooks.toString(),
                    showPhysicalBooks: showPhysicalBooks.toString()
                });

                
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

        // Fetch books whenever search or filter options change
        fetchBooks();
    }, [searchTerm, searchYear, showAudiobooks, showPhysicalBooks]); 

    
    const handleSearchInputChange = (event) => {
        setSearchTerm(event.target.value);
    };

    
    const handleYearInputChange = (event) => {
        setSearchYear(event.target.value);
    };

    
    const toggleAudiobookFilter = () => {
        setShowAudiobooks(!showAudiobooks);
    };

    
    const togglePhysicalBookFilter = () => {
        setShowPhysicalBooks(!showPhysicalBooks);
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
            
            <div style={{ backgroundColor: '#f0f0f0', padding: '20px 0', textAlign: 'center' }}>
                <input
                    type="text"
                    placeholder="Search for a book..."
                    value={searchTerm}
                    onChange={handleSearchInputChange}
                    style={{
                        padding: '10px',
                        fontSize: '16px',
                        width: '300px',
                        borderRadius: '8px',
                        border: '1px solid #ddd',
                        marginRight: '10px',
                    }}
                />
                <input
                    type="text"
                    placeholder="Search for a book year"
                    value={searchYear}
                    onChange={handleYearInputChange}
                    style={{
                        padding: '10px',
                        fontSize: '16px',
                        width: '300px',
                        borderRadius: '8px',
                        border: '1px solid #ddd',
                        marginRight: '10px',
                    }}
                />
                <label>
                    <input
                        type="checkbox"
                        checked={showAudiobooks}
                        onChange={toggleAudiobookFilter}
                    />
                    Show Audiobooks
                </label>
                <label>
                    <input
                        type="checkbox"
                        checked={showPhysicalBooks}
                        onChange={togglePhysicalBookFilter}
                    />
                    Show Physical Books
                </label>
                
                <Link to="/reservations">
                    <button 
                        style={{
                            padding: '10px 20px',
                            backgroundColor: '#007BFF',
                            color: 'white',
                            border: 'none',
                            borderRadius: '5px',
                            cursor: 'pointer',
                            marginLeft: '20px',
                            fontSize: '15px',
                        }}
                    >
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
                            {book.audiobook && (
                                <span className="audio-indicator">🎧 Audiobook</span>
                            )}
                            {book.physicalBook && (
                                <span className="physical-indicator">📚 Physical Book</span>
                            )}
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
