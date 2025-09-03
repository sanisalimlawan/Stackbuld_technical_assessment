# 🛒 E-Commerce Web API (Assessment Project)

## 📌 Overview
This is a **production-grade C# Web API** built for an assessment.  
It demonstrates **clean architecture, scalable design, and real-world readiness** by handling products, orders, and inventory management.

---

## 🚀 Features

### Product Management
- CRUD operations on products
- **Soft Delete** to preserve data integrity
- Restock endpoint: `POST /products/{id}/restock`

### Order Management
- Place orders with one or multiple products
- Prevent overselling using **transactional concurrency control**
- Deduct stock **only when the order succeeds**

### Seed Data
- Default **products and users** seeded for easy testing

### API Documentation
- Integrated **Swagger/OpenAPI** for interactive testing

---

## 🗄️ Tech Stack
- **C# .NET 8 Web API**
- **Entity Framework Core** (with Soft Delete)
- **SQL Server / SQLite** (configurable)
- **FluentValidation** for request validation
- **Swagger** for API documentation

---

## ⚙️ Installation & Setup


### 1. Clone the repo
```bash
https://github.com/sanisalimlawan/Stackbuld_technical_assessment.git
cd Stackbuld_technical_assessment
````
### 2. Navigate into the Project Folder
```bash
cd Stackbuld_technical_assessment
```
### Run the API
```bsah
Run the API
```
# Costumers
| Method | Endpoint                                             | Description                    
|
| POST   | /api/Stackbuld_Ecommerce/Createcostumer                | add new Costumer                
| DELETE    | /api/Stackbuld_Ecommerce/DeleteCostumer/{id}         | remove costumer             
| GET    | /api/Stackbuld_Ecommerce/GetAllCostumer            | Retrieve all costumers           
| PUT    | /api/Stackbuld_Ecommerce/UpdateCostumer/{id} | Update costumer by his ID
| GET    | /api/Stackbuld_Ecommerce/GetCostumerById/{id} | Get costumer by his ID
# Order
| Method | Endpoint                                             | Description                    
|
| POST   | /api/Stackbuld\_Ecommerce/PlaceOrder                 | Place new order                
| GET    | /api/Stackbuld\_Ecommerce/GetOrderById/{id}          | Get order details              
| GET    | /api/Stackbuld\_Ecommerce/GetAllOrders               | Retrieve all orders            
| GET    | /api/Stackbuld\_Ecommerce/GetOrdersByUserId/{userId} | Get orders for a specific user 

