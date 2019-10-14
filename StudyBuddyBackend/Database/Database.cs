using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace StudyBuddyBackend.Database
{
    public sealed class Database
    {
        private readonly string _connectionString;
        private readonly ILogger<Database> _logger;

        public Database(ILogger<Database> logger)
        {
            _logger = logger;
            try
            {
                string mysqlConfigFile = File.ReadAllText("Database/mysqlconfig.json");
                dynamic mysqlConfig = JsonConvert.DeserializeObject(mysqlConfigFile);
                _connectionString =
                    $"data source={mysqlConfig["data_source"]};" +
                    $"database={mysqlConfig["database"]};" +
                    $"user id={mysqlConfig["username"]};" +
                    $"password={mysqlConfig["password"]}";
            }
            catch (FileNotFoundException e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private MySqlConnection OpenConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            return ExecuteQuery(query, new Dictionary<string, object>());
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            var rowEntries = new List<Dictionary<string, object>>();
            try
            {
                using var conn = OpenConnection();
                using var cmd = new MySqlCommand(query, conn);
                foreach (var (key, value) in parameters)
                {
                    cmd.Parameters.AddWithValue(key, value);
                }

                using var reader = cmd.ExecuteReader();
                var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var rowEntry = new Dictionary<string, object>();
                        foreach (string column in columns)
                        {
                            object cell = reader[column];
                            rowEntry.Add(column, cell);
                        }

                        rowEntries.Add(rowEntry);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return rowEntries;
        }

        public void ExecuteNonQuery(string query)
        {
            ExecuteNonQuery(query, new Dictionary<string, object>());
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            using var conn = OpenConnection();
            using var cmd = new MySqlCommand(query, conn);
            foreach (var (key, value) in parameters)
            {
                cmd.Parameters.AddWithValue(key, value);
            }

            cmd.ExecuteNonQuery();
        }
    }
}
