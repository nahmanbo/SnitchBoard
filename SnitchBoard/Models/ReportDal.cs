namespace SnitchBoard.Models;

public class ReportDal
{
    private readonly DabManager _dbHelper = new("SnitchBoard");

    //====================================
    // Inserts a person if they don't already exist
    public void InsertPerson(string firstName, string lastName)
    {
        string query = "INSERT IGNORE INTO people (first_name, last_name) VALUES (@first_name, @last_name)";
        Dictionary<string, object> parameters = new()
        {
            { "@first_name", firstName },
            { "@last_name", lastName }
        };

        _dbHelper.Insert(query, parameters);
    }

    //--------------------------------------------------------------
    // Gets person ID by first and last name
    public int GetPersonId(string firstName, string lastName)
    {
        string query = "SELECT id FROM people WHERE first_name = @first_name AND last_name = @last_name";
        Dictionary<string, object> parameters = new()
        {
            { "@first_name", firstName },
            { "@last_name", lastName }
        };

        List<Dictionary<string, object>> result = _dbHelper.Select(query, parameters);

        if (result.Count > 0 && result[0].ContainsKey("id"))
            return Convert.ToInt32(result[0]["id"]);

        return -1;
    }

    //--------------------------------------------------------------
    // Checks whether a person with the given ID exists
    public bool PersonExistsById(int id)
    {
        string query = "SELECT 1 FROM people WHERE id = @id LIMIT 1";
        Dictionary<string, object> parameters = new()
        {
            { "@id", id }
        };

        List<Dictionary<string, object>> result = _dbHelper.Select(query, parameters);

        if (result.Count > 0)
            return true;

        Console.WriteLine("Person doesn't exist");
        return false;
    }

    //--------------------------------------------------------------
    // Checks if the reported person is considered dangerous
    public bool IsDangerousReported(int reportedId)
    {
        string query =
            "SELECT COUNT(*) AS total_reports, " +
            "SUM(CASE WHEN r.report_time >= NOW() - INTERVAL 15 MINUTE THEN 1 ELSE 0 END) AS recent_reports " +
            "FROM reports r " +
            "WHERE r.reported_id = @id;";

        Dictionary<string, object> parameters = new()
        {
            { "@id", reportedId }
        };

        List<Dictionary<string, object>> result = _dbHelper.Select(query, parameters);

        if (result.Count == 0)
            return false;

        int totalReports = Convert.ToInt32(result[0]["total_reports"]);
        int recentReports = Convert.ToInt32(result[0]["recent_reports"]);

        return totalReports > 3 || recentReports >= 3;
    }

    //--------------------------------------------------------------
    // Checks if the reporter is considered good
    public bool IsGoodReporter(int reporterId)
    {
        string query =
            "SELECT COUNT(*) AS report_count, " +
            "AVG(CHAR_LENGTH(text)) AS avg_length " +
            "FROM reports r " +
            "WHERE r.reporter_id = @id;";

        Dictionary<string, object> parameters = new()
        {
            { "@id", reporterId }
        };

        List<Dictionary<string, object>> result = _dbHelper.Select(query, parameters);

        if (result.Count == 0)
            return false;

        int reportCount = Convert.ToInt32(result[0]["report_count"]);
        double avgLength = Convert.ToDouble(result[0]["avg_length"]);

        return reportCount >= 10 || avgLength >= 30;
    }

    //--------------------------------------------------------------
    // Inserts a new report record
    public void InsertReport(int reporterId, int reportedId, string reportText)
    {
        string query = "INSERT INTO reports (reporter_id, reported_id, text) VALUES (@r1, @r2, @text)";
        Dictionary<string, object> parameters = new()
        {
            { "@r1", reporterId },
            { "@r2", reportedId },
            { "@text", reportText }
        };

        _dbHelper.Insert(query, parameters);
    }

    //--------------------------------------------------------------
    // Updates boolean status field for a person
    public void UpdateStatusToTrue(int personId, string columnName)
    {
        string query = $"""
            INSERT INTO people_statuses (person_id, {columnName}) VALUES (@personId, TRUE)
            ON DUPLICATE KEY UPDATE {columnName} = TRUE;
        """;

        Dictionary<string, object> parameters = new()
        {
            { "@personId", personId }
        };

        _dbHelper.Insert(query, parameters);
    }

    //--------------------------------------------------------------
    // Adds a report, either by reporter ID or by name
    public void AddReport(Report report)
    {
        int reporterId = -1;

        if (report.ReporterIdNumber != null)
        {
            reporterId = int.Parse(report.ReporterIdNumber);
        }
        else if (report.ReporterName != null)
        {
            InsertPerson(report.ReporterName.Value.FirstName, report.ReporterName.Value.LastName);
            reporterId = GetPersonId(report.ReporterName.Value.FirstName, report.ReporterName.Value.LastName);
        }

        InsertPerson(report.ReportedName.FirstName, report.ReportedName.LastName);
        int reportedId = GetPersonId(report.ReportedName.FirstName, report.ReportedName.LastName);

        if (reporterId != -1 && reportedId != -1)
        {
            InsertReport(reporterId, reportedId, report.Text);
            UpdateStatusToTrue(reporterId, "is_reporter");
            UpdateStatusToTrue(reportedId, "is_reported");

            if (IsDangerousReported(reportedId))
                UpdateStatusToTrue(reportedId, "is_dangerous_reported");

            if (IsGoodReporter(reporterId))
                UpdateStatusToTrue(reporterId, "is_good_reporter");
        }
        else
        {
            Console.WriteLine("Failed to retrieve person IDs for report.");
        }
    }

    //--------------------------------------------------------------
    // Prints all dangerous reported individuals
    public void PrintAllDangerousReported()
    {
        string query =
            "SELECT p.first_name, p.last_name " +
            "FROM people p " +
            "JOIN people_statuses ps ON p.id = person_id " +
            "WHERE ps.is_dangerous_reported = TRUE;";

        List<Dictionary<string, object>> results = _dbHelper.Select(query);

        Console.WriteLine("Dangerous Report:");
        foreach (var row in results)
        {
            Console.WriteLine(row["first_name"] + " " + row["last_name"]);
        }
    }

    //--------------------------------------------------------------
    // Prints all good reporters
    public void PrintAllGoodReporter()
    {
        string query =
            "SELECT p.first_name, p.last_name " +
            "FROM people p " +
            "JOIN people_statuses ps ON p.id = ps.person_id " +
            "WHERE ps.is_good_reported = TRUE;";

        List<Dictionary<string, object>> results = _dbHelper.Select(query);

        Console.WriteLine("Good Reporter:");
        foreach (var row in results)
        {
            Console.WriteLine(row["first_name"] + " " + row["last_name"]);
        }
    }

    //--------------------------------------------------------------
    // Closes the DB connection
    public void Close()
    {
        _dbHelper.Close();
    }
}
