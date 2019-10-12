using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudyBuddyBackend
{
    public sealed class Database
    {
        private readonly string connectionString;
        public static Database Instance { get; } = new Database();

        private Database()
        {
            try
            {
                string mysqlConfigFile = File.ReadAllText("mysqlconfig.json");
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
                // TODO add logger
            }
        }

        private MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var rowEntries = new List<Dictionary<string, object>>();
            try
            {
                using var conn = OpenConnection();
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();
                List<string> columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
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
                // TODO add logger
            }
            return rowEntries;
        }
    }
}