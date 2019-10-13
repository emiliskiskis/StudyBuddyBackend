using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Attributes;

namespace StudyBuddyBackend.Database
{
    public abstract class CrudRepository<T, U> where T : new()
    {
        private readonly Database database;
        private readonly string primaryField;

        protected CrudRepository(Database database)
        {
            this.database = database;
            this.primaryField = (from prop in typeof(T).GetProperties()
                                 where Attribute.IsDefined(prop, typeof(IdAttribute))
                                 select prop).ToList()[0].Name.ToLower();
        }

        private string GetTableName()
        {
            return typeof(T).Name.Replace("Repository", "") + "s";
        }

        protected T Convert(object o)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(o));
        }

        protected List<T> ConvertAll(List<Dictionary<string, object>> d)
        {
            var list = new List<T>();
            foreach (var el in d)
            {
                list.Add(Convert(el));
            }

            return list;
        }

        public List<T> GetAll()
        {
            return ConvertAll(database.ExecuteQuery("SELECT * FROM @table;",
                new Dictionary<string, object> { { "@table", GetTableName() } }));
        }

        public Optional<T> Get(U id)
        {
            try
            {
                return new Optional<T>(
                    Convert(database.ExecuteQuery("SELECT * FROM @table WHERE @primaryField = @id;",
                        new Dictionary<string, object>
                        {
                            {"@table", GetTableName()}, {"@primaryField", primaryField}, {"@id", id}
                        })[0])
                );
            }
            catch (Exception)
            {
                return new Optional<T>();
            }
        }
    }
}
