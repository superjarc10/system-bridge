# System Bridge

A simple order, shipment, and production progress tracking system.

The goal of the project is to provide a central system for managing customer orders, tracking their production progress, and organizing shipments.

## Features

* Customer management
* Product management
* Order creation and tracking
* Production progress tracking
* Shipment management
* Inventory tracking
* Centralized API and database
* Client application for interacting with the system

## Project Structure

The `src` directory contains the main application projects:

* `SystemBridge.Api` — Backend API
* `SystemBridge.Client` — Client application

## Requirements

* .NET SDK 8.0 or newer
* Visual Studio 2022, JetBrains Rider, or another .NET-compatible IDE

## Build

Clone the repository:

```bash
git clone https://github.com/superjarc10/system-bridge.git
cd system-bridge
```

Build the solution:

```bash
dotnet build
```

## Run

Start the API:

```bash
dotnet run --project src/SystemBridge.Api
```

Then start the client:

```bash
dotnet run --project src/SystemBridge.Client
```

Alternatively, open the solution in Visual Studio or Rider and run the projects from there.

## Development

The application is currently designed as a simple foundation for managing the flow:

```text
Customer
   ↓
Order
   ↓
Production
   ↓
Completed
   ↓
Shipment
```

The system can be extended with features such as automated order creation from emails, barcode scanning, warehouse management, inventory updates, and automated shipment planning.

## Status

This project is currently under development.
