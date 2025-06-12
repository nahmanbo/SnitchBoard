# SnitchBoard

**Author:** Nahman Ben Or  
**Description:** A console-based reporting system where agents can submit reports about people. The system flags good reporters and dangerous individuals based on database logic.

---

## 🚀 Key Features

- Submit reports by ID or name
- Track people reported and their status
- Identify "good reporters" and "dangerous reported"
- All data is stored in a MySQL database

---

## 🧠 Summary of Key Classes & Functions

### `Controller.cs`
- `Run()` – Main loop with console menu
- `HandleAddReport()` – Creates a new report
- `ShowMainMenu()` – Displays menu options
- `IsValidExistingId()` – Validates if reporter ID exists

### `Report.cs`
- `CreateReportWithValidId()` – Create a report from ID
- `CreateReportWithName()` – Create a report from reporter name
- `SplitFullName()` – Splits full name into first/last
- `ToString()` – Returns full report as a string

### `ReportDal.cs`
- `InsertPerson()` – Adds person if not exists
- `GetPersonId()` – Gets ID based on name
- `InsertReport()` – Inserts a report record
- `UpdateStatusToTrue()` – Updates person status
- `IsGoodReporter()` – Checks if reporter is “good”
- `IsDangerousReported()` – Checks if reported person is dangerous
- `PrintAllDangerousReported()` – Lists all dangerous people
- `PrintAllGoodReporter()` – Lists all good reporters

### `DabManager.cs`
- `Insert()` – Executes parameterized INSERT
- `Select()` – Executes SELECT and returns rows
- `Close()` – Closes DB connection

---

## 🏗️ Requirements

- .NET SDK
- MySQL Server
- Database name: `SnitchBoard`

Required tables:
- `people(id, first_name, last_name)`
- `reports(id, reporter_id, reported_id, text, report_time)`
- `people_statuses(person_id, is_reporter, is_reported, is_dangerous_reported, is_good_reporter)`

---

## 🛠️ How to Run

```bash
dotnet run
