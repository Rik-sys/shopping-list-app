import React, { useEffect } from 'react';
import { Container, Row, Col, Spinner, Alert } from 'react-bootstrap';
import { useAppDispatch, useAppSelector } from '../hooks/useApi';
import { 
  fetchCategories, 
  createSession, 
  fetchCurrentCart 
} from '../store/shoppingSlice';
import TotalCounter from './TotalCounter';
import AddItemForm from './AddItemForm';
import CategorySection from './CategorySection';
import CompleteOrderButton from './CompleteOrderButton';

const ShoppingList: React.FC = () => {
  const dispatch = useAppDispatch();
  const { 
    categories, 
    currentCart, 
    sessionId, 
    loading, 
    error 
  } = useAppSelector(state => state.shopping);

  useEffect(() => {
    // Initialize the app
    const initializeApp = async () => {
      try {
        // Fetch categories
        await dispatch(fetchCategories()).unwrap();
        
        // Create new session
        const newSessionId = await dispatch(createSession()).unwrap();
        
        // Fetch current cart (will be empty for new session)
        if (newSessionId) {
          await dispatch(fetchCurrentCart(newSessionId));
        }
      } catch (error) {
        console.error('Failed to initialize app:', error);
      }
    };

    initializeApp();
  }, [dispatch]);

  // Refresh cart when sessionId changes
  useEffect(() => {
    if (sessionId) {
      dispatch(fetchCurrentCart(sessionId));
    }
  }, [sessionId, dispatch]);

  if (loading && !currentCart) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">טוען...</span>
        </Spinner>
        <p className="mt-2">טוען את האפליקציה...</p>
      </Container>
    );
  }

  if (error) {
    return (
      <Container className="mt-4">
        <Alert variant="danger" className="text-center">
          <h5>שגיאה בטעינת האפליקציה</h5>
          <p>{error}</p>
          <button 
            className="btn btn-outline-danger"
            onClick={() => window.location.reload()}
          >
            נסה שוב
          </button>
        </Alert>
      </Container>
    );
  }

  return (
    <Container fluid className="py-4" style={{ maxWidth: '1200px' }}>
      {/* Header */}
      <Row className="mb-4">
        <Col>
          <h1 className="text-center text-primary mb-3" style={{ color: '#000000' }}>
            🛒 רשימת קניות
          </h1>
          <p className="text-center text-muted">
            הוסף מוצרים לסל הקניות ונהל את הקנייה בקלות
          </p>
        </Col>
      </Row>

      {/* Total Counter */}
      <Row className="mb-4">
        <Col>
          <TotalCounter />
        </Col>
      </Row>

      {/* Add Item Form */}
      <Row className="mb-4">
        <Col>
          <AddItemForm />
        </Col>
      </Row>

      {/* Shopping Cart Categories */}
      <Row>
        <Col>
          {currentCart && currentCart.categories.length > 0 ? (
            <>
              <h4 className="mb-3">הפריטים בסל שלך:</h4>
              {currentCart.categories.map(category => (
                <CategorySection 
                  key={category.categoryId} 
                  category={category} 
                />
              ))}
            </>
          ) : (
            <Alert variant="info" className="text-center">
              <h5>הסל ריק</h5>
              <p className="mb-0">הוסף מוצרים לסל כדי להתחיל קניות</p>
            </Alert>
          )}
        </Col>
      </Row>

      {/* Complete Order Button */}
      {currentCart && currentCart.totalItems > 0 && (
        <Row className="mt-4">
          <Col>
            <CompleteOrderButton />
          </Col>
        </Row>
      )}

      {/* Session Info (for debugging) */}
      {sessionId && (
        <Row className="mt-4">
          <Col>
            <small className="text-muted">
              Session ID: {sessionId}
            </small>
          </Col>
        </Row>
      )}
    </Container>
  );
};

export default ShoppingList;
