namespace SnitchBoard.Models;

public class DabManager
{
    private readonly DbHelper _dbHelper = new DbHelper();

    //====================================
    public void InsertPerson(string firstName, string lastName)
    {
        string query = "INSERT IGNORE INTO people (first_name, last_name) VALUES (@first, @last)";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@first", firstName },
            { "@last", lastName }
        };

        _dbHelper.Insert(query, parameters);
    }

    //--------------------------------------------------------------
    public int GetPersonId(string firstName, string lastName)
    {
        string query = "SELECT id FROM people WHERE first_name = @first AND last_name = @last";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@first", firstName },
            { "@last", lastName }
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
        string query = "INSERT INTO reports (reporter_id, partner_id, text) VALUES (@r1, @r2, @text)";
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
        string query = $"UPDATE status SET {columnName} = true WHERE person_id = @id";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@id", personId }
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
