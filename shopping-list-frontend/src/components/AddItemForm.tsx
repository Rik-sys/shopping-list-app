import React, { useState } from 'react';
import { Form, Button, Row, Col, Card, Alert } from 'react-bootstrap';
import { useAppDispatch, useAppSelector } from '../hooks/useApi';
import { addItemToCart, fetchCurrentCart } from '../store/shoppingSlice';
import { DEFAULT_VALUES } from '../utils/constants';

const AddItemForm: React.FC = () => {
  const dispatch = useAppDispatch();
  const { categories, sessionId, loading, error } = useAppSelector(state => state.shopping);
  
  const [productName, setProductName] = useState('');
  const [categoryId, setCategoryId] = useState<number | ''>('');
  const [quantity, setQuantity] = useState(1);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!productName.trim() || !categoryId || !sessionId) {
      return;
    }

    try {
      await dispatch(addItemToCart({
        productName: productName.trim(),
        categoryId: categoryId as number,
        quantity,
        unit: DEFAULT_VALUES.UNIT,
        priority: DEFAULT_VALUES.PRIORITY,
      })).unwrap();
      
      await dispatch(fetchCurrentCart(sessionId));
      
      setProductName('');
      setCategoryId('');
      setQuantity(1);
    } catch (error) {
      console.error('Failed to add item:', error);
    }
  };

  return (
    <Card className="mb-4">
      <Card.Header>
        <h5 className="mb-0">הוספת מוצר חדש</h5>
      </Card.Header>
      <Card.Body>
        {error && (
          <Alert variant="danger" dismissible>
            {error}
          </Alert>
        )}
        
        <Form onSubmit={handleSubmit}>
          <Row>
            <Col md={5}>
              <Form.Group className="mb-3">
                <Form.Label>שם המוצר</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="הכנס שם מוצר..."
                  value={productName}
                  onChange={(e) => setProductName(e.target.value)}
                  required
                />
              </Form.Group>
            </Col>
            
            <Col md={4}>
              <Form.Group className="mb-3">
                <Form.Label>קטגוריה</Form.Label>
                <Form.Select
                  value={categoryId}
                  onChange={(e) => setCategoryId(Number(e.target.value) || '')}
                  required
                >
                  <option value="">בחר קטגוריה...</option>
                  {categories.map(category => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
                </Form.Select>
              </Form.Group>
            </Col>
            
            <Col md={2}>
              <Form.Group className="mb-3">
                <Form.Label>כמות</Form.Label>
                <Form.Control
                  type="number"
                  min="1"
                  value={quantity}
                  onChange={(e) => setQuantity(Number(e.target.value))}
                />
              </Form.Group>
            </Col>
            
            <Col md={1} className="d-flex align-items-end">
              <Button 
                variant="primary" 
                type="submit" 
                disabled={loading || !productName.trim() || !categoryId}
                className="mb-3 w-100"
              >
                {loading ? 'מוסיף...' : 'הוסף'}
              </Button>
            </Col>
          </Row>
        </Form>
      </Card.Body>
    </Card>
  );
};

export default AddItemForm;