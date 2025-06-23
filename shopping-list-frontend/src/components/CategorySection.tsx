import React from 'react';
import { Card, Badge } from 'react-bootstrap';
import { CartCategory } from '../types/shopping';
import ItemCard from './ItemCard';

interface CategorySectionProps {
  category: CartCategory;
}

const CategorySection: React.FC<CategorySectionProps> = ({ category }) => {
  // Helper function to get icon based on category name
  const getCategoryIcon = (iconName?: string, categoryName?: string) => {
    if (iconName) return iconName;
    
    switch (categoryName) {
      case 'מוצרי ניקיון': return '🧽';
      case 'גבינות': return '🧀';
      case 'ירקות ופירות': return '🍎';
      case 'בשר ודגים': return '🥩';
      case 'מאפים': return '🍞';
      default: return '📦';
    }
  };

  return (
    <Card className="mb-4 border-0 shadow">
      <Card.Header className="bg-light border-0">
        <div className="d-flex justify-content-between align-items-center">
          <h5 className="mb-0 d-flex align-items-center">
            <span className="me-2 fs-4">
              {getCategoryIcon(category.categoryIcon, category.categoryName)}
            </span>
            {category.categoryName}
          </h5>
          <Badge bg="primary" className="fs-6">
            {category.totalItems} מוצרים
          </Badge>
        </div>
      </Card.Header>
      <Card.Body>
        {category.items.length === 0 ? (
          <p className="text-muted text-center mb-0">אין מוצרים בקטגוריה זו</p>
        ) : (
          category.items.map(item => (
            <ItemCard key={item.id} item={item} />
          ))
        )}
      </Card.Body>
    </Card>
  );
};

export default CategorySection;