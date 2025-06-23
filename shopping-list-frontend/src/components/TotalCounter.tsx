import React from 'react';
import { Badge, Card } from 'react-bootstrap';
import { useAppSelector } from '../hooks/useApi';

const TotalCounter: React.FC = () => {
  const currentCart = useAppSelector(state => state.shopping.currentCart);
  
  const totalItems = currentCart?.totalItems || 0;

  return (
    <Card className="mb-4 text-center bg-primary text-white">
      <Card.Body>
        <h4 className="mb-0">
          <Badge bg="light" text="dark" className="fs-5">
            סה"כ: {totalItems} מוצרים בסל
          </Badge>
        </h4>
      </Card.Body>
    </Card>
  );
};

export default TotalCounter;