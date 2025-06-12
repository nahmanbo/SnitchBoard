using MySql.Data.MySqlClient;

namespace SnitchBoard.Models;

public class DabManager
{
    private readonly MySqlConnection _connection;

    //====================================
    public DabManager(string databaseName)
    {
        string connectionString = $"server=127.0.0.1;user id=root;password=;database={databaseName};port=3306;";
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
    }

    //--------------------------------------------------------------
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
    public List<Dictionary<string, object>> Select(string query, Dictionary<string, object>? parameters = null)
    {
        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

        using (MySqlCommand command = new MySqlCommand(query, _connection))
        {
            foreach (KeyValuePair<string, object> param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();

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
    public void Close()
    {
        _connection.Close();
    }
}
