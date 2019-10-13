using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace StudyBuddyBackend.Database
{
    public sealed class Database
    {
        private readonly string connectionString;
        private readonly ILogger<Database> logger;

        public Database(ILogger<Database> logger) : this()
        {
            this.logger = logger;
        }

        private Database()
        {
            try
            {
                string mysqlConfigFile = File.ReadAllText("Database/mysqlconfig.json");
                dynamic mysqlConfig = JsonConvert.DeserializeObject(mysqlConfigFile);
                connectionString = string.Format("data source={0};database={1};user id={2};password={3}",
                    mysqlConfig["data_source"],
                    mysqlConfig["database"],
                    mysqlConfig["username"],
                    mysqlConfig["password"]
                );
            }
            catch (FileNotFoundException e)
            {
                logger.LogError(e.ToString());
            }
        }

        private MySqlConnection OpenConnection()
        {
            var connection = new MySqlConnection(connectionString);
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
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
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
                logger.LogError(e.ToString());
            }
            return rowEntries;
        }
    }
}
