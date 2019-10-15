using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Attributes;

namespace StudyBuddyBackend.Database.Repositories
{
    public abstract class CrudRepository<T, TX> : ICrudRepository<T, TX>
    {
        private readonly Database _database;
        private readonly ILogger _logger;
        private readonly string _primaryField;
        private readonly List<KeyValuePair<PropertyInfo, string>> _propertyColumnList;
        private readonly string _table;

        protected CrudRepository(Database database, ILogger logger)
        {
            _logger = logger;
            _propertyColumnList = new List<KeyValuePair<PropertyInfo, string>>();
            foreach (var property in typeof(T).GetProperties())
            {
                _propertyColumnList.Add(new KeyValuePair<PropertyInfo, string>(property, GetColumnName(property)));
            }

            _database = database;
            _primaryField = (from prop in typeof(T).GetProperties()
                where Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute))
                select prop).ToList()[0].Name.ToLower();
            _table = typeof(T).Name.Replace("Repository", "") + "s";
            try
            {
                CreateTable();
            }
            catch (MySqlException e)
            {
                if ((int) e.Code == (int) MySqlErrorCode.TableExists)
                {
                    _logger.LogWarning(e.ToString());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
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
            _database.ExecuteNonQuery(query, GetValues(el));
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
            _database.ExecuteNonQuery(query, new Dictionary<string, object> {{"id", id}});
        }

        private void CreateTable()
        {
            string query = _propertyColumnList.Aggregate("CREATE TABLE Users(",
                (current, keyValuePair) =>
                    current +
                    $"{GetColumnName(keyValuePair.Key)} " +
                    $"{GetSqlPropertyType(keyValuePair.Key)} " +
                    $"{GetPropertyAttributes(keyValuePair.Key)},");
            query = query.Substring(0, query.Length - 1) + ");";
            _logger.LogDebug(query);
            _database.ExecuteNonQuery(query);
        }

        private void CompareTable()
        {
            var schema = _database.ExecuteQuery($"DESC {_table};");
            var matches = Regex.Matches(JsonConvert.SerializeObject(schema),
                "\"Type\":\"(.*?)\"").ToList();
            var types = matches.Select(t => t.Groups[1].Value).ToList();

            _logger.LogDebug(JsonConvert.SerializeObject(types));
        }

        private static string GetSqlPropertyType(PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                return Attribute.IsDefined(property, typeof(TextAttribute)) ? "text" : "varchar(255)";
            }

            if (property.PropertyType == typeof(int))
            {
                return "int";
            }

            if (property.PropertyType == typeof(DateTime))
            {
                return "datetime";
            }

            return "";
        }
        
        private static string GetPropertyAttributes(PropertyInfo property)
        {
            var attributes = "";

            if (Attribute.IsDefined(property, typeof(PrimaryKeyAttribute)))
            {
                attributes += " primary key not null";
                if (Attribute.IsDefined(property, typeof(AutoIncrementAttribute)))
                {
                    attributes += " auto_increment";
                }

                return attributes;
            }

            if (!Attribute.IsDefined(property, typeof(NullableAttribute)))
            {
                attributes += " not null";
            }

            return attributes;
        }

        private static T Convert(object o)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(o));
        }

        private static List<T> ConvertAll(IEnumerable<Dictionary<string, object>> d)
        {
            return d.Select(Convert).ToList();
        }

        private static string GetColumnName(PropertyInfo property)
        {
            if (Attribute.IsDefined(property, typeof(JsonPropertyAttribute)))
            {
                return (
                    (JsonPropertyAttribute)
                    typeof(T).GetProperty(property.Name)
                        ?.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                        .First()
                )?.PropertyName;
            }

            return property.Name.First().ToString().ToLower() + property.Name.Substring(1);
        }

        private string GetColumns()
        {
            return _propertyColumnList.Aggregate("", (current, keyValuePair) => current + $",{keyValuePair.Value}")
                .Substring(1);
        }

        private string GetValueParams()
        {
            return _propertyColumnList.Aggregate("", (current, keyValuePair) => current + $",@{keyValuePair.Value}")
                .Substring(1);
        }

        private static Dictionary<string, object> GetValues(T el)
        {
            return typeof(T).GetProperties()
                .ToDictionary(GetColumnName, property => property.GetValue(el));
        }
    }
}
