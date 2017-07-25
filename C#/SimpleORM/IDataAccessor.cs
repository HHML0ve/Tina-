using System.Collections.Generic;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    public interface IDataAccessor
    {
        /// <summary>
        /// Query the mapped table corresponding to TEntity with given condition
        /// 
        /// NOTE: How to map TEntity with specific table in SQL db ?
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition">The sql WHERE clause</param>
        /// <returns>The list of TEntity</returns>
        List<TEntity> Query<TEntity>(string condition) where TEntity : class, new();

        /// <summary>
        /// Create a new record based on the TEntity, returns the value 
        /// of the primary key (identity column)
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Create<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Update the TEntity by its primary key, return the row effected,
        /// return 1 means the entity has been updated successfully, otherwise,
        /// the operation has failed.
        /// 
        /// NOTE: the TEntity must have a primary key property, how to achieve that?
        ///       The primary key cannot be updated.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Delete the TEntity by its primary key, return the row effected,
        /// return 1 means the entity has been deleted successfully, otherwise,
        /// the operation has failed.
        /// 
        /// NOTE: the TEntity must have a primary key property, how to achieve that?
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}
