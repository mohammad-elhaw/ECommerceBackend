# ECommerceBackend
E-Commerce Platform – Modular Monolith Architecture (DDD + Docker + .NET + PostgreSQL)
This project is a production-grade E-Commerce Platform built using a Modular Monolith Architecture and deeply aligned with Domain-Driven Design (DDD) principles. The goal of the project is to build a scalable, maintainable, and business-focused system with clean boundaries, strong domain models, and clear separation of concerns across modules.

Architecture Overview:
The system is organized into a Modular Monolith, where each business capability is placed inside an independent module (bounded context). Core modules include:

Customer Module
Product & Catalog Module
Order & Cart Module
Payment Module

Each module encapsulates its Domain, Application, Infrastructure, and API layers internally, ensuring high cohesion and minimal coupling.

This structure provides monolithic simplicity while maintaining the scalability and independence of a microservices-friendly architecture.

Domain-Driven Design Implementation

This project applies key DDD patterns:

✔ Rich Domain Model
Entities and Aggregates with business invariants
Value Objects for immutability and consistency
Domain Events for communication inside the domain
Guard clauses and validation logic inside constructors/factories

✔ Domain Layer at the Core
All business rules live in the domain layer, not in controllers or services.

✔ Application Layer
Handles use cases
Coordinates domain logic
Contains DTOs, mappers, and interfaces for repositories

✔ Infrastructure Layer
EF Core repository implementations
PostgreSQL migrations
Modular data configurations

✔ Shared Layer
External integrations (e.g., payments, identity, storage, RappitMQ)

Docker Integration
The entire system is containerized using Docker to ensure consistent local development and easy deployment.

Docker Features:
Separate containers for API and PostgreSQL
Environment variables for DB configuration
docker-compose.yml to orchestrate complete environment
One-command startup for developers:
docker compose up --build


Database — PostgreSQL:
EF Core used for ORM
Each module maintains its own schema & configurations
Clean separation between domain and persistence
All entities mapped using Fluent API for full control


Key Features of the System:
Modular Monolith Architecture with clean domain boundaries
Full Domain-Driven Design approach (Entities, Aggregates, Value Objects)
Rich domain logic and business validation using Abstract Validation
PostgreSQL database with EF Core
Dockerized environment (API + DB)
CQRS & Mediator for applying seperation of concern
Outbox Pattern for reliable messaging between modules with RabbitMQ Message Broker
Clean Architecture and Onion Architecture principles
Scalable foundation ready to evolve into microservices
Easy setup for any developer using Docker Compose
KeyCloak for Identity and Authorization

Tech Stack:
.NET 9
Domain-Driven Design (DDD)
Modular Monolith Architecture
EF Core
PostgreSQL
Docker & Docker Compose
Clean Architecture / Modular Monolith Architecture
CQRS & Mediator
RabbitMQ and Outbox Pattern
Keycloak Identity Server
