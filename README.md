# 🛒 רשימת קניות - מטלת בית מפתח רמה א'

##  תיאור הפרויקט

אפליקציית רשימת קניות מתקדמת הבנויה במסגרת מטלת בית למשרד הביטחון. המערכת מאפשרת ניהול רשימות קניות עם קטגוריות, ספירת פריטים, ושמירת נתונים בענן.

##  קישורים לאתרים חיים

- ** אפליקציית React**: [[https://blue-ground-0e42e7610.6.azurestaticapps.net]([https://blue-ground-0e42e7610.6.azurestaticapps.net](https://blue-ground-0e42e7610.6.azurestaticapps.net/)](https://blue-ground-0e42e7610.6.azurestaticapps.net/))
- **🔧 API השרת**: [https://shopping-list-api.azurewebsites.net](https://shopping-list-api.azurewebsites.net)
- ** מסד נתונים**: Azure SQL Database בענן

##  ארכיטקטורת המערכת

```
Frontend (React + TypeScript) ← → Backend (.NET 8 Web API) ← → Azure SQL Database
       ↓                              ↓                           ↓
Azure Static Web Apps         Azure App Service           Azure SQL Server
```

##  טכנולוגיות ושפות

### Frontend (צד לקוח)
- **React 18** - ספריית JavaScript לבניית ממשקי משתמש
- **TypeScript** - שפת תכנות מבוססת JavaScript עם typing סטטי
- **React Bootstrap** - ספריית UI לעיצוב responsive
- **Redux Toolkit** - ניהול state גלובלי של האפליקציה
- **Axios** - ספריית HTTP client לתקשורת עם השרת
- **React Hook Form** - ניהול טפסים ו-validation
- **React Router** - ניווט בין דפים באפליקציה

### Backend (צד שרת)
- **C# .NET 8** - מסגרת פיתוח מתקדמת של Microsoft
- **ASP.NET Core Web API** - בניית RESTful APIs
- **Entity Framework Core** - ORM לניהול מסד הנתונים
- **AutoMapper** - מיפוי אובייקטים בין שכבות
- **SQL Server** - מסד נתונים יחסי מתקדם

### Cloud & Infrastructure (ענן ותשתית)
- **Microsoft Azure** - פלטפורמת ענן
- **Azure Static Web Apps** - אחסון ופרסום הfrontend
- **Azure App Service** - אחסון ופרסום הbackend
- **Azure SQL Database** - מסד נתונים מנוהל בענן
- **GitHub Actions** - CI/CD אוטומטי לפריסה





 

### הוראות התקנה והפעלה - שלב אחר שלב

#### שלב 1: הורדת הפרויקט מGitHub
```bash
# שכפול הפרויקט הראשי
git clone https://github.com/Rik-sys/shopping-list-app.git
cd ShoppingList

# וידוא מבנה התיקיות:
ls -la
# אמור להראות:
# 📁 Shopping_List/           - פרויקט השרת (.NET)
# 📁 shopping-list-frontend/  - פרויקט הלקוח (React)
```

#### שלב 2: הכנת מסד הנתונים המקומי
```bash
# וידוא שLocalDB פועל
sqllocaldb info

# אם LocalDB לא קיים, צור אותו:
sqllocaldb create MSSQLLocalDB

# התחלת LocalDB
sqllocaldb start MSSQLLocalDB
```

#### שלב 3: הכנת Backend (.NET) - בטרמינל 1
```bash
# מעבר לתיקיית השרת
cd Shopping_List

# התקנת כל החבילות והתלויות
dotnet restore

# בנייה של הפרויקט לוידוא שהכל תקין
dotnet build
```

** שלב קריטי - עדכון Connection String:**
לפני הפעלת השרת, **חובה לערוך את קובץ `appsettings.json`**:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShoppingListDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "AllowedHosts": "*"
}
```

**זה הConnection String למסד נתונים מקומי!** (במקום זה שמצביע לאזור)

```bash
# עכשיו יצירת מסד הנתונים והטבלאות
dotnet ef database update

# הרצת השרת עם הגדרות development
dotnet run --environment Development

# אל תסגור את הטרמינל הזה!
```

#### שלב 4: הכנת Frontend (React) - בטרמינל חדש (2)
```bash
# חזרה לתיקיית הראשית ומעבר לReact
cd ../shopping-list-frontend

# התקנת כל החבילות והתלויות (זה יכול לקחת כמה דקות)
npm install

# וידוא שכל החבילות הותקנו בהצלחה
npm audit fix
```

** שלב קריטי - עדכון API URL:**
לפני הפעלת React, **חובה לערוך את קובץ `src/utils/constants.ts`**:

```typescript
// החלף את השורה הזאת:
// export const API_BASE_URL = 'https://shopping-list-api.azurewebsites.net';

// בכתובת שהשרת שלך רץ:
export const API_BASE_URL = 'https://localhost:7066';

// שאר הקובץ נשאר כמו שהוא

```

**זה מצביע על השרת המקומי במקום השרת בענן!**

```bash
# עכשיו הרצת שרת הפיתוח של React
npm start

# האפליקציה תיפתח על: http://localhost:3000
```


###  פתרון בעיות נפוצות


# אם יש בעיה עם Migrations:
dotnet ef migrations add InitialCreate
dotnet ef database update

# אם יש שגיאות חבילות:
dotnet clean
dotnet restore
dotnet build
```

#### בעיות בReact:
```bash
# אם React מתחבר לשרת בענן במקום המקומי:
# וודא שערכת את src/utils/constants.ts
# API_BASE_URL צריך להיות: 'https://localhost:7066'

# אם npm install נכשל:
npm cache clean --force
npm install

# אם יש שגיאות TypeScript:
npm install --save-dev @types/node @types/react @types/react-dom

# אם יש בעיות עם versions:
npm update
```

#### אזהרה חשובה: 
** לפני שאתה מעלה את הקוד חזרה לGitHub:**
1. **החזר את ה-Connection String** ב-`appsettings.json` לערך הענן
2. **החזר את ה-API_BASE_URL** ב-`constants.ts` לערך הענן
3. **אחרת השרת בענן לא יעבוד!**

#### בעיות מסד נתונים:
```bash
# בדיקה אם LocalDB פועל:
sqllocaldb info MSSQLLocalDB

# הפעלה מחדש של LocalDB:
sqllocaldb stop MSSQLLocalDB
sqllocaldb start MSSQLLocalDB

# מחיקה ויצירה מחדש של מסד נתונים:
dotnet ef database drop
dotnet ef database update
```




**שם המפתח**: ריקי פישר 
**תאריך**: דצמבר 2024  
**מטרה**: מטלת בית למשרד הביטחון 
תודה למשרד הביטחון על ההזדמנות לפתח פרויקט מרתק זה ולהציג יכולות טכניות מתקדמות בפיתוח fullstack מודרני.
