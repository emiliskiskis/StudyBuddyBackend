using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Attributes;

namespace StudyBuddyBackend.Database.Repositories
{
    public abstract class CrudRepository<T, TX> : ICrudRepository<T, TX>
    {
        private readonly List<KeyValuePair<string, string>> _propertyColumnDict;
        private readonly Database _database;
        private readonly ILogger _logger;
        private readonly string _primaryField;
        private readonly string _table;

        protected CrudRepository(Database database, ILogger logger)
        {
            _logger = logger;
            _propertyColumnDict = new List<KeyValuePair<string, string>>();
            foreach (var property in typeof(T).GetProperties())
            {
                _propertyColumnDict.Add(new KeyValuePair<string, string>(property.Name, GetColumnName(property.Name)));
            }

            _database = database;
            _primaryField = (from prop in typeof(T).GetProperties()
                where Attribute.IsDefined(prop, typeof(IdAttribute))
                select prop).ToList()[0].Name.ToLower();
            _table = typeof(T).Name.Replace("Repository", "") + "s";
        }

        public List<T> ReadAll()
        {
            return ConvertAll(_database.ExecuteQuery($"SELECT * FROM {_table};"));
        }
        
        public void Create(T el)
        {
            string query = $"INSERT INTO {_table}({GetColumns()}) VALUES (" +
                           $"{GetValueParams()});";
            _logger.LogDebug(query);
            _logger.LogDebug(JsonConvert.SerializeObject(GetValues(el)));
            _database.ExecuteNonQuery(query,GetValues(el));
        }

        public Optional<T> Read(TX id)
        {
            try
            {
                return new Optional<T>(
                    Convert(_database.ExecuteQuery($"SELECT * FROM {_table} WHERE {_primaryField} = @id;",
                        new Dictionary<string, object> {{"id", id}})[0])
                );
            }
            catch (Exception)
            {
                return new Optional<T>();
            }
        }

        public void Update(T el)
        {
            throw new NotImplementedException();
        }

        public void Delete(TX id)
        {
            string query = $"DELETE FROM {_table} WHERE ${_primaryField} = @id";
            _database.ExecuteNonQuery(query, new Dictionary<string, object>()
            {
                {"id", id}
            });
        }

        private static T Convert(object o)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(o));
        }

        private static List<T> ConvertAll(IEnumerable<Dictionary<string, object>> d)
        {
            return (from u in d
                select Convert(u)).ToList();
        }

        private static string GetColumnName(string propertyName)
        {
            return Regex.Replace(propertyName, "([A-Z])", "_$1").ToLower().Substring(1);
        }

        private string GetColumns()
        {
            return _propertyColumnDict.Aggregate("", (current, keyValuePair) => current + $",{keyValuePair.Value}")
                .Substring(1);
        }

        private string GetValueParams()
        {
            return _propertyColumnDict.Aggregate("", (current, keyValuePair) => current + $",@{keyValuePair.Value}")
                .Substring(1);
        }

        private static Dictionary<string, object> GetValues(T el)
        {
            return typeof(T).GetProperties()
                .ToDictionary(property => GetColumnName(property.Name), property => property.GetValue(el));
        }
    }
}
