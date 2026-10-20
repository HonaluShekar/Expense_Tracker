# C# Expense Tracker

## About the Project

The C# Expense Tracker is a console-based application built using C# and .NET 10. It helps users manage their income and expenses, search transactions, and understand their financial summaries.

## Features

- Add income and expense transactions.
- Automatically assign unique transaction IDs.
- Validate amounts, dates, descriptions, and categories.
- View, update, and delete transactions.
- Search transactions by description.
- Filter transactions by type, category, and date range.
- Calculate total income, expenses, and balance.
- View category-wise expense totals.
- Generate monthly financial summaries.
- Save and load transactions using JSON.
- Run automated unit tests using xUnit.

## Technologies Used

- C#
- .NET 10
- LINQ
- JSON serialization using System.Text.Json
- xUnit for unit testing
- Visual Studio Code

## Project Structure

- "Models" — Transaction model and enums.
- "Services" — Transaction operations, validation, filtering, and reports.
- "Persistence" — JSON data storage and retrieval.
- "Tests" — Automated unit tests.
- "Program.cs" — Console menu and application entry point.
- "transactions.json" — Transaction data storage.

## How to Run the Application

1. Install the .NET 10 SDK.
2. Open the project folder in Visual Studio Code.
3. Open a terminal in the project root.
4. Execute:

powershell
dotnet run


## How to Run Tests

Run the following command from the project root:

powershell
dotnet test Tests/ExpenseTracker.Tests.csproj


The test suite checks transaction operations, validation, searching, filtering, reports, and JSON persistence.

## Data Persistence

Transactions are stored in transactions.json. The application loads saved data when it starts and saves transaction changes through the JSON repository.

## Learning Outcomes

This project demonstrates object-oriented programming, collections, enums, exception handling, LINQ, JSON serialization, file handling, and unit testing in C#.