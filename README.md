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
git clone https://github.com/yourusername/ecommerce-api.git
cd ecommerce-api
