using MySql.Data.MySqlClient;

namespace SnitchBoard.Models;

public class DabManager
{
    private readonly MySqlConnection _connection;

    //====================================
    // Constructor: initializes and opens DB connection
    public DabManager(string databaseName)
    {
        string connectionString = $"server=127.0.0.1;user id=root;password=;database={databaseName};port=3306;";
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
    }

    //--------------------------------------------------------------
    // Executes an INSERT query with parameters
    public void Insert(string query, Dictionary<string, object> parameters)
    {
        using (MySqlCommand command = new MySqlCommand(query, _connection))
        {
            foreach (KeyValuePair<string, object> param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            command.ExecuteNonQuery();
        }
    }

    //--------------------------------------------------------------
    // Executes a SELECT query and returns results as list of dictionaries
    public List<Dictionary<string, object>> Select(string query, Dictionary<string, object>? parameters = null)
    {
        List<Dictionary<string, object>> results = new();

        using (MySqlCommand command = new MySqlCommand(query, _connection))
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Dictionary<string, object> row = new();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object value = reader.GetValue(i);
                        row[columnName] = value;
                    }

                    results.Add(row);
                }
            }
        }

        return results;
    }

    //--------------------------------------------------------------
    // Closes the DB connection
    public void Close()
    {
        _connection.Close();
    }
}
