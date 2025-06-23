import React, { useState } from 'react';
import { Button, Modal, Form, Alert } from 'react-bootstrap';
import { CheckCircle } from 'react-bootstrap-icons';
import { useAppDispatch, useAppSelector } from '../hooks/useApi';
import { completeOrder, createSession } from '../store/shoppingSlice';

const CompleteOrderButton: React.FC = () => {
  const dispatch = useAppDispatch();
  const { currentCart, sessionId, loading } = useAppSelector(state => state.shopping);
  
  const [showModal, setShowModal] = useState(false);
  const [notes, setNotes] = useState('');
  const [showSuccess, setShowSuccess] = useState(false);

  const totalItems = currentCart?.totalItems || 0;
  const canComplete = totalItems > 0 && sessionId;

  const handleComplete = async () => {
    if (!sessionId) return;

    try {
      await dispatch(completeOrder({
        sessionId,
        notes: notes.trim() || undefined
      })).unwrap();
      
      setShowModal(false);
      setNotes('');
      setShowSuccess(true);
      
      // Create new session for next order
      await dispatch(createSession());
      
      // Hide success message after 3 seconds
      setTimeout(() => setShowSuccess(false), 3000);
    } catch (error) {
      console.error('Failed to complete order:', error);
    }
  };

  if (showSuccess) {
    return (
      <Alert variant="success" className="text-center">
        <CheckCircle className="me-2" />
        ההזמנה הושלמה בהצלחה! סשן חדש נוצר.
      </Alert>
    );
  }

  return (
    <>
      <div className="text-center">
        <Button
          variant="success"
          size="lg"
          disabled={!canComplete || loading}
          onClick={() => setShowModal(true)}
          className="px-5 py-3"
        >
          <CheckCircle className="me-2" />
          סיים הזמנה ({totalItems} מוצרים)
        </Button>
        
        {!canComplete && totalItems === 0 && (
          <p className="text-muted mt-2 mb-0">
            הוסף מוצרים לסל כדי לסיים הזמנה
          </p>
        )}
      </div>

      <Modal show={showModal} onHide={() => setShowModal(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>סיום הזמנה</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>
            האם אתה בטוח שברצונך לסיים את ההזמנה עם <strong>{totalItems} מוצרים</strong>?
          </p>
          
          <Form.Group className="mt-3">
            <Form.Label>הערות (אופציונלי)</Form.Label>
            <Form.Control
              as="textarea"
              rows={3}
              placeholder="הוסף הערות להזמנה..."
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
            />
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowModal(false)}>
            ביטול
          </Button>
          <Button 
            variant="success" 
            onClick={handleComplete}
            disabled={loading}
          >
            {loading ? 'מסיים...' : 'סיים הזמנה'}
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};

export default CompleteOrderButton;