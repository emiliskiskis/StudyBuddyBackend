using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace StudyBuddyBackend.Database.Repositories
{
    /// <summary>
    ///     An interface describing an object-relational map with a MySQL table.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <typeparam name="TX">Entity primary key type.</typeparam>
    public interface ICrudRepository<T, in TX>
    {
        /// <summary>
        ///     Get all rows from the table.
        /// </summary>
        /// <returns>List of all rows in table as entity objects.</returns>
        List<T> ReadAll();

        /// <summary>
        ///     Creates a new entry in the table, if does not exist yet.
        /// </summary>
        /// <param name="el">Entity to be added.</param>
        void Create(T el);

        /// <summary>
        ///     Gets the entity by primary key from the table.
        /// </summary>
        /// <param name="id">Primary key value of element.</param>
        /// <returns>An Optional with Value set to entity object if exists in table, else Optional with Value unset.</returns>
        Optional<T> Read(TX id);

        /// <summary>
        ///     Updates the element entry in the table, if exists.
        /// </summary>
        /// <param name="el">Entity with values to be updated and the primary key.</param>
        void Update(T el);

        /// <summary>
        ///     Deletes the element entry in the table, if exists.
        /// </summary>
        /// <param name="id">Primary key value of element.</param>
        void Delete(TX id);
    }
}