namespace SnitchBoard.Models;

public class DabManager
{
    private readonly DbHelper _dbHelper = new DbHelper();

    //====================================
    public void InsertPerson(string firstName, string lastName)
    {
        string query = "INSERT IGNORE INTO people (first_name, last_name) VALUES (@first_name, @last_name)";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@first_name", firstName },
            { "@last_name", lastName }
        };

        _dbHelper.Insert(query, parameters);
    }

    //--------------------------------------------------------------
    public int GetPersonId(string firstName, string lastName)
    {
        string query = "SELECT id FROM people WHERE first_name = @first_name AND last_name = @last_name";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@first_name", firstName },
            { "@last_name", lastName }
        };

        List<Dictionary<string, object>> result = _dbHelper.Select(query, parameters);

        if (result.Count > 0 && result[0].ContainsKey("id"))
        {
            return Convert.ToInt32(result[0]["id"]);
        }

        return -1;
    }

    //--------------------------------------------------------------
    public void InsertReport(int reporterId, int reportedId, string reportText)
    {
        string query = "INSERT INTO reports (reporter_id, reported_id, text) VALUES (@r1, @r2, @text)";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@r1", reporterId },
            { "@r2", reportedId },
            { "@text", reportText }
        };

        _dbHelper.Insert(query, parameters);
    }

    //--------------------------------------------------------------
    public void UpdateStatusToTrue(int personId, string columnName)
    {
        string query = $@"INSERT INTO people_statuses (person_id, {columnName}) VALUES (@personId, TRUE) ON DUPLICATE KEY UPDATE {columnName} = TRUE;";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@personId", personId }
        };

        _dbHelper.Insert(query, parameters);
    }
    
    //--------------------------------------------------------------
    public void AddReport(Report report)
    {
        InsertPerson(report.ReporterName.FirstName, report.ReporterName.LastName);
        InsertPerson(report.ReportedName.FirstName, report.ReportedName.LastName);

        int reporterId = GetPersonId(report.ReporterName.FirstName, report.ReporterName.LastName);
        int reportedId = GetPersonId(report.ReportedName.FirstName, report.ReportedName.LastName);

        if (reporterId != -1 && reportedId != -1)
        {
            InsertReport(reporterId, reportedId, report.Text);
            UpdateStatusToTrue(reporterId, "is_reporter");
            UpdateStatusToTrue(reportedId, "is_reported");
        }
        else
        {
            Console.WriteLine("Failed to retrieve person IDs for report.");
        }
    }

    //--------------------------------------------------------------
    public void Close()
    {
        _dbHelper.Close();
    }
}
