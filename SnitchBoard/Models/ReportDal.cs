using MySql.Data.MySqlClient;
namespace SnitchBoard.Models;

public class ReportDal
{
    private const string ConnectionString = "server=127.0.0.1;user id=root;password=;database=SnitchBoard;port=3306;";
    private MySqlConnection _connection = new MySqlConnection(ConnectionString);

    //====================================
    public ReportDal()
    {
        OpenConnection();
    }

    //--------------------------------------------------------------
    public void OpenConnection()
    {
        try
        {
            _connection.Open();
            Console.WriteLine("Connection opened");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection failed: " + ex.Message);
        }
    }

    //--------------------------------------------------------------
    public void CloseConnection()
    {
        try
        {
            _connection.Close();
            Console.WriteLine("Connection closed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to close connection: " + ex.Message);
        }
    }

    //--------------------------------------------------------------
    public void InsertIntoTable(string sqlQuery, Dictionary<string, object> parameters)
    {
        try
        {
            MySqlCommand command = new MySqlCommand(sqlQuery, _connection);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to insert: " + ex.Message);
        }
    }

    //--------------------------------------------------------------
    public List<Dictionary<string, object>> SelectFromTable(string sqlQuery, Dictionary<string, object> parameters)
    {
        List<Dictionary<string, object>> resultRows = new List<Dictionary<string, object>>();

        try
        {
            MySqlCommand command = new MySqlCommand(sqlQuery, _connection);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object value = reader.GetValue(i);
                    row.Add(columnName, value);
                }

                resultRows.Add(row);
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to read: " + ex.Message);
        }

        return resultRows;
    }

    //--------------------------------------------------------------
    public int GetPersonIdByName(string firstName, string lastName)
    {
        string query = "SELECT id FROM people WHERE first_name = @first AND last_name = @last";

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@first", firstName);
        parameters.Add("@last", lastName);

        List<Dictionary<string, object>> result = SelectFromTable(query, parameters);

        if (result.Count > 0 && result[0].ContainsKey("id"))
        {
            object value = result[0]["id"];
            return Convert.ToInt32(value);
        }

        return -1; 
    }

    //--------------------------------------------------------------
    private void InsertPerson(string firstName, string lastName)
    {
        string query = "INSERT IGNORE INTO people (first_name, last_name) VALUES (@first, @last);";

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@first", firstName);
        parameters.Add("@last", lastName);

        InsertIntoTable(query, parameters);
    }

    //--------------------------------------------------------------
    private void InsertReport(int reporterId, int reportedId, string reportText)
    {
        string query = "INSERT INTO reports (reporter_id, partner_id, text) VALUES (@reporterId, @reportedId, @text);";

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@reporterId", reporterId);
        parameters.Add("@reportedId", reportedId);
        parameters.Add("@text", reportText);

        InsertIntoTable(query, parameters);
    }

    //--------------------------------------------------------------
    public void AddReportByNames(string[] reportData)
    {
        (string reporterFirstName, string reporterLastName) = SplitFullName(reportData[0]);
        (string reportedFirstName, string reportedLastName) = SplitFullName(reportData[1]);
        string reportText = reportData[2];

        InsertPerson(reporterFirstName, reporterLastName);
        InsertPerson(reportedFirstName, reportedLastName);

        int reporterId = GetPersonIdByName(reporterFirstName, reporterLastName);
        int reportedId = GetPersonIdByName(reportedFirstName, reportedLastName);

        if (reporterId != -1 && reportedId != -1)
        {
            InsertReport(reporterId, reportedId, reportText);
        }
        else
        {
            Console.WriteLine("Failed to retrieve person IDs for report.");
        }
    }
    
    //--------------------------------------------------------------
    private (string, string) SplitFullName(string fullName)
    {
        try
        {
            string[] parts = fullName.Trim().Split(' ');
            string firstName = parts[0];
            string lastName = string.Join(" ", parts.Skip(1));
            return (firstName, lastName);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to split: " + ex.Message);
            throw;
        }
    }
}