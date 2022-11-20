using Dapper;
using DapperORM.App.Database;
using DapperORM.App.Library;
using DapperORM.App.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperORM.App.Services
{
    public class DataAccessService<T, TInput> : IDataAccessService<T, TInput>
    {
        private readonly IDBContext dbContext;
        public string ItemName { get; } // Must be provided from constructor. e.g. book
        public string ItemPluralName { get; } // (Optional) Calculate from ItemName if not provided. e.g. book -> books
        public string TableName { get; } // (Optional) Calculate from ItemPluralName or ItemName if not provided. e.g. book -> Books

        public DataAccessService(
            IDBContext dbContext,
            string ItemName,
            string ItemPluralName = null,
            string TableName = null
        )
        {
            this.dbContext = dbContext;
            this.ItemName = ItemName;
            this.ItemPluralName = string.IsNullOrEmpty(ItemPluralName) ? $"{ItemName}s": ItemPluralName;
            this.TableName = string.IsNullOrEmpty(TableName) ? $"{this.ItemPluralName.First().ToString().ToUpper()}{this.ItemPluralName[1..]}" : TableName;
        }

        public async Task<InstanceCUDMessage<int>> Save(IEnumerable<TInput> newItems, string storedProcedureName = null, string spInputParamName = null, string sqlInputTypeName = null)
        {

            // Guess SP name, input param name and input type name if not provided
            string spNameToUse = string.IsNullOrEmpty(storedProcedureName) ?
                    $"[P_Mutation_Save{TableName}]":
                    storedProcedureName;

            string inputParamNameToUse = string.IsNullOrEmpty(spInputParamName) ?
                    $"@input{TableName}" :
                    sqlInputTypeName;

            string inputTypeToUse = string.IsNullOrEmpty(sqlInputTypeName) ?
                    $"[Input{TableName}]" :
                    sqlInputTypeName;

            var itemTable = DataTableUtility.FromObjects(newItems);
            var parameters = new DynamicParameters();
            parameters.Add(inputParamNameToUse, itemTable.AsTableValuedParameter(inputTypeToUse));

            IEnumerable<int> newIds;
            try
            {
                newIds = await dbContext.Connection.QueryAsync<int>(
                    spNameToUse,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception e)
            {
                return new InstanceCUDMessage<int>(
                    Ok: false,
                    NumAffected: 0,
                    Message: $"Failed to save {ItemPluralName}: {e.Message}",
                    Instances: null
                );
            }

            return new InstanceCUDMessage<int>(
                Ok: true,
                NumAffected: newIds.Count(),
                Message: $"Successfully saved {itemTable.Rows.Count} {ItemPluralName}.",
                Instances: newIds
            );
        }

        public async Task<IEnumerable<T>> GetByID(IEnumerable<int> ids)
        {
            IEnumerable<T> items;

            try
            {
                items = await dbContext.Connection.QueryAsync<T>(
                    "[P_Query_GetItems]",
                    new
                    {
                        tableName = TableName,
                        ids = string.Join(',', ids),
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to retrieve {ItemPluralName}: {e.Message}");
                return null;
            }

            return items;
        }

        public async Task<IEnumerable<T>> GetByDBName(IEnumerable<string> dbnames)
        {
            IEnumerable<T> items;

            var quotedDBNames = (
                from dbname in dbnames
                select $"'{dbname}'"
            );

            try
            {
                items = await dbContext.Connection.QueryAsync<T>(
                    "[P_Query_GetItems]",
                    new
                    {
                        tableName = TableName,
                        dbNames = string.Join(',', quotedDBNames),
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to retrieve {ItemPluralName}: {e.Message}");
                return null;
            }

            return items;
        }

        public async Task<CUDMessage> DeleteByID(IEnumerable<int> ids)
        {
            long rowsAffected = -1;
            try
            {
                rowsAffected = await dbContext.Connection.ExecuteAsync(
                    "[P_Mutation_DeleteItems]",
                    new
                    {
                        tableName = TableName,
                        ids = string.Join(',', ids),
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception e)
            {
                return new CUDMessage(
                    Ok: false,
                    NumAffected: rowsAffected,
                    Message: $"Failed to delete {ItemPluralName}: {e.Message}"
                );
            }

            return new CUDMessage(
                Ok: true,
                NumAffected: rowsAffected,
                Message: "yes"
            );
        }

        #region Aliases

        public async Task<InstanceCUDMessage<int>> Save(TInput newItem, string storedProcedureName = null, string spInputParamName = null, string sqlInputTypeName = null)
        {
            return await Save(
                new List<TInput>() { newItem },
                storedProcedureName,
                spInputParamName,
                sqlInputTypeName
            );
        }

        public async Task<T> GetByID(int id)
        {
            return (await GetByID(new List<int>() { id })).FirstOrDefault();
        }

        public async Task<T> GetByDBName(string dbname)
        {
            return (await GetByDBName(new List<string>() { dbname })).FirstOrDefault();
        }

        public async Task<CUDMessage> DeleteByID(int id)
        {
            return await DeleteByID(new List<int>() { id });
        }

        #endregion

    }
}
