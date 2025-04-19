using Databases.Interfaces;
using NLog;

namespace Databases.Interfaces;

public interface ITable
{
    /// <summary>
    /// insert a record into a dictionary table using userId as the primary key
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="record"></param>
    /// <returns></returns>
    int Insert(int userId, IRecord record);
    
    /// <summary>
    /// update a record into a dictionary table using userId as the primary key
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="record"></param>
    /// <returns></returns>
    int Update(int userId, IRecord record);
    
    /// <summary>
    /// delete a record from a dictionary table by userId 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    bool Delete(int userId);
    
    /// <summary>
    /// get a record from a dictionary table using userId as the primary key and possibly a rowId as a secondary key
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="rowId"></param>
    /// <returns></returns>
    IRecord? Get(int userId, long? rowId = null);
}