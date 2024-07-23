# DepartmentReminderApp

## Overview

DepartmentReminderApp is an application designed to manage departments and sub-departments with a multi-level hierarchy and set reminders that trigger email notifications. The application uses ASP.NET Core MVC, Entity Framework Core for data access, and FluentEmail for sending emails. The scheduling of email notifications is handled using Quartz.NET.

## Features

### Department Module
- **Multi-level Hierarchy**: Departments can contain multiple sub-departments, and sub-departments can further contain sub-departments.
- **Department Management**: Create, edit, delete departments, and view department hierarchy.
- **Department Details**: Display a list of all sub-departments within the selected department/sub-department and a list of all parent departments up to the top-level for the selected department/sub-department.

### Reminder Module
- **Reminder Management**: Set reminders with a title and specific date-time for sending an email notification.
- **Email Notifications**: Reminders trigger email notifications at the specified date-time using Quartz.NET.

## Setup and Configuration

### Prerequisites
- .NET SDK (version 8.0 or higher)
- SQL Server (or another compatible database)
- SMTP server for sending emails (e.g., Mailtrap)