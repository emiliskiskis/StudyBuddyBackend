using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Core.Attributes;
using Nullable = StudyBuddyBackend.Database.Core.Attributes.Nullable;

namespace StudyBuddyBackend.Database.Core
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
            _primaryField = typeof(T).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKey))).ToList()[0].Name.ToLower();
            _table = GetTableName(typeof(T));
            try
            {
                CreateTable();
            }
            catch (MySqlException e)
            {
                _logger.LogError(e.Number.ToString());
                if (e.Number == (int)MySqlErrorCode.TableExists)
                {
                    _logger.LogWarning(e.ToString());
                }
                else throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public List<T> ReadAll()
        {
            var list = _database.ExecuteQuery($"SELECT * FROM {_table};");
            return ConvertAll(list);
        }

        public void Create(T el)
        {
            string query = $"INSERT INTO {_table}({GetColumns()}) VALUES (" +
                           $"{GetValueParams()});";
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

        public void Update(TX id, IEnumerable<string> keys, T el)
        {
            var parameters = GetValues(el);
            if (!parameters.ContainsKey(_primaryField))
            {
                parameters.Add(_primaryField, id);
            }
            else
            {
                parameters[_primaryField] = id;
            }

            string query = $"UPDATE {_table} " +
                           GetUpdateValueParams(keys) + " " +
                           $"WHERE {_primaryField} = @{_primaryField};";
            _database.ExecuteNonQuery(query, parameters);
        }

        public void Delete(TX id)
        {
            string query = $"DELETE FROM {_table} WHERE ${_primaryField} = @id";
            _database.ExecuteNonQuery(query, new Dictionary<string, object> {{"id", id}});
        }

        private void CreateTable()
        {
            string query = _propertyColumnList.Aggregate($"CREATE TABLE {_table}(",
                (current, keyValuePair) =>
                    current +
                    $"{GetColumnName(keyValuePair.Key)} " +
                    $"{GetSqlPropertyType(keyValuePair.Key)} " +
                    $"{GetPropertyAttributes(keyValuePair.Key)},");
            query += GetPrimaryKeyConstraint(typeof(T)) + ",";
            query += GetForeignKeyConstraints(_propertyColumnList);
            query = query.Substring(0, query.Length - 1) + ");";
            _database.ExecuteNonQuery(query);
        }

        private void CompareTable()
        {
            var schema = _database.ExecuteQuery($"DESC {_table};");
            var matches = Regex.Matches(JsonConvert.SerializeObject(schema),
                "\"Type\":\"(.*?)\"").ToList();
            var types = matches.Select(t => t.Groups[1].Value).ToList();
        }
        
        private static string GetSqlPropertyType(PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                return Attribute.IsDefined(property, typeof(Text)) ? "TEXT" : "VARCHAR(255)";
            }

            if (property.PropertyType == typeof(int))
            {
                return "INT";
            }

            if (property.PropertyType == typeof(DateTime))
            {
                return "DATETIME";
            }

            return "";
        }

        private static string GetPropertyAttributes(PropertyInfo property)
        {
            var attributes = "";

            if (Attribute.IsDefined(property, typeof(PrimaryKey)))
            {
                attributes += " NOT NULL";
                if (Attribute.IsDefined(property, typeof(AutoIncrement)))
                {
                    attributes += " AUTO_INCREMENT";
                }

                return attributes.Substring(1);
            }

            if (!Attribute.IsDefined(property, typeof(Nullable)))
            {
                attributes += " NOT NULL";
            }

            if (Attribute.IsDefined(property, typeof(CurrentDateTime)))
            {
                attributes += " DEFAULT CURRENT_TIMESTAMP";
            }

            return attributes.Substring(1);
        }

        private static string GetPrimaryKeyConstraint(Type t)
        {
            string constraint = t.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKey)))
                .Aggregate(
                    "PRIMARY KEY (", (current, prop) => current + GetColumnName(prop) + ",");
            return constraint.Substring(0, constraint.Length - 1) + ")";
        }

        private string GetForeignKeyConstraints(List<KeyValuePair<PropertyInfo, string>> propertyColumnList)
        {
            string constraints = propertyColumnList.Aggregate("",
                (current, keyValuePair) => current + GetForeignKeyConstraint(keyValuePair.Key));
            return constraints.Length > 0 ? constraints.Substring(0, constraints.Length - 1) : "";
        }

        private string GetForeignKeyConstraint(PropertyInfo property)
        {
            if (Attribute.IsDefined(property, typeof(ForeignKey)))
            {
                var foreignKeyRepositoryType = ((ForeignKey)property
                    .GetCustomAttributes(typeof(ForeignKey), false)
                    .First())?.RepositoryType;

                var entityType =
                    (foreignKeyRepositoryType ?? throw new NullReferenceException()).GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICrudRepository<,>))
                    .GetGenericArguments()[0];

                return
                    $"FOREIGN KEY ({GetColumnName(property)}) REFERENCES {_database.DatabaseName}.{GetTableName(foreignKeyRepositoryType)}(" +
                    GetColumnName(entityType.GetProperties().First(prop =>
                        Attribute.IsDefined(prop, typeof(PrimaryKey)))) + "),";
            }

            return "";
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
                    property.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
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

        private static string GetUpdateValueParams(IEnumerable<string> keys)
        {
            string parameters = $"SET {keys.Aggregate("", (current, key) => current + $"{key} = @{key},")}";
            return parameters.Substring(0, parameters.Length - 1);
        }

        private static Dictionary<string, object> GetValues(T el)
        {
            return typeof(T).GetProperties()
                .ToDictionary(GetColumnName, property => property.GetValue(el));
        }

        private static string GetTableName(Type t)
        {
            return t.Name.Replace("Repository", "") + "s";
        }
    }
}
