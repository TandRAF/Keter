# Keter - Smart Task Management System

**Keter** is a robust task and project management platform designed to streamline team collaboration through Role-Based Access Control (RBAC) and real-time progress monitoring. 
![main img](./assets/main-img.jpg)
## Technologies

### Backend
* **Language:** C# (.NET 8)
* **Framework:** ASP.NET Core Web API
* **Security:** Identity Framework (JWT & RBAC)
* **ORM:** Entity Framework Core

### Frontend
* **Library:** React with TypeScript
* **Styling:** SCSS (Sass)
* **State Management:** React Context API / Hooks

### Infrastructure & Database
* **Database:** PostgreSQL
* **Containerization:** Docker & Docker Compose
* **Orchestration:** Kubernetes (K8s)

---

## Core Functionalities

### 1. Authentication & Authorization (RBAC)
* Secure Login/Register system using Identity Framework.
* **Roles:**
    * **Admin:** Full control over Projects and Boards. The only role capable of creating and assigning tasks to members.
    * **Member:** Can view boards they are assigned to and edit only the status of their own tasks.

### 2. Project & Task Management
* **Projects:** The top-level entity grouping multiple boards.
* **Boards:** Visual organization of tasks (e.g., Web Development, OS Project).
* **Tasks:** Entities containing title, description, and status tags (`Todo`, `In Progress`, `Done`).
* **Assignment:** Admins can attribute specific members to tasks to define accountability.

### 3. Notification System
* Users receive alerts when assigned to a new Project.
* Visual indicators for new Task assignments.

---

## System Architecture

[Image of full-stack web application architecture diagram showing React, .NET API, and PostgreSQL]

The application follows a containerized micro-services architecture, ensuring environment parity through Docker and scalability via Kubernetes.

---