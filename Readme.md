# .NET Training Project

## Overview
Welcome to my .NET Training Project! This project showcases my expertise in building a microservices-based e-commerce platform using .NET technologies. The platform includes functionalities for catalog management, ordering, discounts, and basket management.

## Technologies Used
### Backend
- **.NET Core and ASP.NET Core**: Frameworks for building the microservices and APIs.
- **Entity Framework Core**: ORM for data access and database management.
- **MongoDB**: NoSQL database for storing product catalog data.
- **Docker**: Containerization of microservices for consistent deployment.
- **Ocelot API Gateway**: Manages and routes API requests to appropriate services.
- **Swagger/OpenAPI**: API documentation and testing interface.
- **Serilog**: Logging library for structured logging.
- **ElasticSearch**: Search engine for indexing and querying logs.


## Architecture
The project follows a microservices architecture where each service is independent, focusing on a specific business capability. Services communicate over HTTP/REST, and each service implement in clean architecture.

### Microservices
- **Catalog.API**: Manages product information and catalog.
- **Ordering.API**: Handles order creation, processing, and management.
- **Basket.API**: Manages user shopping baskets.
- **Discount.API**: Manages discount rules and applies discounts to orders.
- **Ocelot.ApiGateway**: Routes requests to the appropriate microservices and handles cross-cutting concerns.


### Logging and Monitoring
- **Serilog**: Used for logging application events.
- **ElasticSearch**: Stores and indexes logs for querying and monitoring.

## Getting Started
To run the project locally, follow these steps:

1. **Clone the repository**:
    ```sh
    git clone https://github.com/MsN-12/eShop
    cd eShop
    ```

2. **Build and run the Docker containers**:
    ```sh
    docker-compose up --build
    ```

3. **Access the APIs**:
    - Catalog API: `http://localhost:9000/swagger`
    - Ordering API: `http://localhost:9001/swagger`
    - Basket API: `http://localhost:9002/swagger`
    - Discount API: `http://localhost:9003/swagger`
    - API Gateway: `http://localhost:7004/swagger`

