Here is a **clean, detailed README-style version** of your deployment documentation. I kept **all important steps and technical details**, but structured it properly so it looks **professional on GitHub**.

---

# Cloud Deployment Guide (MonsterASP.NET)

This document describes the deployment architecture and step-by-step process for deploying an **ASP.NET Core full-stack application** using **MonsterASP.NET cloud hosting**.

---

# Architecture Overview

**Frontend:** ASP.NET Core (Razor Pages / MVC)
**Backend:** ASP.NET Core Web Application
**Hosting Provider:** MonsterASP.NET
**Database:** Microsoft SQL Server 2022 (Cloud Hosted)
**Development Tool:** Microsoft Visual Studio 2022
**Deployment Method:** Publish Profile (.publishsettings)

---

# System Architecture

```
User
  ↓
MonsterASP.NET (ASP.NET Core Web App)
  ↓
Cloud SQL Server 2022 (MonsterASP.NET Database)
```

---

# Database Deployment Guide

## 1. Creating the Online Database

1. Go to the **MonsterASP.NET** website.
2. Create a **new database**.
3. Select the **Free Plan**.
4. Choose **SQL Server 2022**.
5. Click **Continue**.
6. Select the **EU Datacenter**.
7. Click the **Settings/Manage icon** of the created database.
8. Enable **Remote Access**.
9. Copy the database credentials and connect using **SQL Server Management Studio (SSMS)**.

   * Ensure **Trust Server Certificate** is enabled.

---

## 2. Migrating Local Database to the Cloud

1. In **SSMS**, right-click the **local database**.
2. Go to:

```
Tasks → Generate Scripts
```

3. Configure the following:

**Set Scripting Options → Advanced**

* Script USE DATABASE → **False**
* Types of data to script → **Schema and Data**

4. Click **Choose Objects**.
5. Select:

```
Script entire database and all database objects
```

6. Continue to the next step.
7. Select:

```
Open in new query window
```

8. After the script is generated:

* Remove the **first section** of the script containing the comment:

```
Object: DATABASE [DatabaseName]
```

9. Copy the remaining script.

---

## 3. Importing the Database to the Cloud

1. Select the **Cloud Database** in SSMS.
2. Click **New Query**.
3. Paste the copied script.
4. Execute the query.

You can verify if the tables were successfully created by:

* Viewing tables in **SSMS**, or
* Checking the **MonsterASP.NET database panel**.

5. After the migration is complete, **disable Remote Access** for security.

---

# Website Deployment Guide

## 1. Creating the Website

1. Open the **Website Panel** in MonsterASP.NET.
2. Click **Add Website**.
3. Choose the **Free Plan**.
4. Enter your **domain name**.
5. Click **Continue**.
6. Select the **EU Datacenter**.
7. Create the website.

---

## 2. Preparing Deployment

1. Once the website is running, open **Settings/Manage**.
2. Scroll down and click **Deploy (FTP / WebDeploy)**.
3. Enable deployment.
4. Download the **Publish Profile**.

---

## 3. Configuring the Database Connection

1. Go to the **Database Tab**.

2. Open **Settings/Manage**.

3. Find **Local Access**.

4. Copy the **connection credentials**.

5. In **Visual Studio 2022**, open:

```
appsettings.json
```

6. Paste the connection string.

### Example Connection String

```json
"DefaultConnection": "Server=db41068.public.databaseasp.net;Database=db41068;User Id=db41068;Password=J!o9@7Hrh=6A;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=120;"
```

---

# Publishing the Application

1. Open the project in **Visual Studio 2022**.
2. Navigate to the top menu:

```
Build → Publish (Project Name)
```

3. Select **Import Profile**.
4. Import the **Publish Profile** downloaded earlier.
5. Click **Publish**.

Deployment may take several minutes.

---

# Verification

After deployment is complete:

1. Open the **website domain** created earlier.
2. Verify that:

   * The website loads correctly.
   * The database connection works.
   * Application features operate as expected.

---

# Deployment Status

If all steps are completed correctly, the ASP.NET Core application should now be **fully deployed and accessible online via MonsterASP.NET**.

