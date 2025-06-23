import React from 'react';
import { Card, Badge, Button, Row, Col } from 'react-bootstrap';
import { Trash3 } from 'react-bootstrap-icons';
import { ShoppingCartItem } from '../types/shopping';
import { useAppDispatch, useAppSelector } from '../hooks/useApi';
import { removeItem, fetchCurrentCart } from '../store/shoppingSlice';

interface ItemCardProps {
  item: ShoppingCartItem;
}

const ItemCard: React.FC<ItemCardProps> = ({ item }) => {
  const dispatch = useAppDispatch();
  const sessionId = useAppSelector(state => state.shopping.sessionId);

  const handleRemove = async () => {
    if (!sessionId) return;
    
    try {
      await dispatch(removeItem(item.id)).unwrap();
      await dispatch(fetchCurrentCart(sessionId));
    } catch (error) {
      console.error('Failed to remove item:', error);
    }
  };

  return (
    <Card className="mb-2 shadow-sm">
      <Card.Body className="py-2">
        <Row className="align-items-center">
          <Col xs={8} md={9}>
            <div className="d-flex align-items-center">
              <strong className="me-2">{item.productName}</strong>
              {item.quantity > 1 && (
                <Badge bg="primary" className="me-2">
                  {item.quantity}
                </Badge>
              )}
              <small className="text-muted">({item.unit})</small>
            </div>
            {item.notes && (
              <small className="text-muted d-block">{item.notes}</small>
            )}
          </Col>
          <Col xs={4} md={3} className="text-end">
            <Button
              variant="outline-danger"
              size="sm"
              onClick={handleRemove}
              className="me-2"
            >
              <Trash3 />
            </Button>
            {item.priority !== 'Normal' && (
              <Badge 
                bg={item.priority === 'High' ? 'danger' : 'warning'}
                text={item.priority === 'High' ? 'white' : 'dark'}
              >
                {item.priority}
              </Badge>
            )}
          </Col>
        </Row>
      </Card.Body>
    </Card>
  );
};

export default ItemCard;
