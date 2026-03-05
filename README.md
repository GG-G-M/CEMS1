# Tech-Driven Media Agency System

## Overview

This is an ASP.NET Core web application deployed on MonsterASP.NET with a cloud-hosted SQL Server 2022 database.

The system provides:

* Online booking and scheduling
* Database-driven content management
* Secure client access
* Cloud deployment integration

---

## System Architecture

User
↓
MonsterASP.NET (ASP.NET Core Web App)
↓
Cloud SQL Server 2022 Database

---

## Technologies Used

* ASP.NET Core
* Entity Framework Core
* Microsoft SQL Server 2022
* Visual Studio 2022
* HTML, CSS, Bootstrap

---

## Deployment Summary (MonsterASP.NET)

### 1. Create Database

* Add SQL Server 2022 database in MonsterASP.NET
* Enable remote access temporarily
* Connect using SSMS
* Generate scripts from local database (Schema and Data)
* Execute script in cloud database
* Disable remote access after migration

### 2. Create Website

* Add website in MonsterASP.NET (Free Plan)
* Download Publish Profile from Deploy section

### 3. Publish from Visual Studio

* Open project in Visual Studio
* Click Publish
* Import Publish Profile
* Deploy

Once completed, access the provided domain to verify deployment.

---

## Local Setup

1. Clone the repository
2. Open in Visual Studio
3. Update `appsettings.json` with your connection string
4. Run database migrations if needed
5. Run the project locally

---

## Notes

* Disable remote database access after migration
* Keep connection strings secure
* Perform regular database backups

---

Status: Deployed and Operational
