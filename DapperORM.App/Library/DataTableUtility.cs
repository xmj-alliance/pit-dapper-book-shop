using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DapperORM.App.Library
{
    public class DataTableUtility
    {
        public static DataTable FromObjects<T>(IEnumerable<T> srcObjects)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            var dataTable = new DataTable
            {
                TableName = type.FullName
            };

            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(
                    new DataColumn(
                        info.Name,
                        Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType
                    )
                );
            }

            foreach (T entity in srcObjects)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static DataTable FromValues<T>(IEnumerable<T> srcValues, string columnName)
        {
            Type type = typeof(T);

            var dataTable = new DataTable
            {
                TableName = $"{columnName} - {type.FullName}" 
            };

            dataTable.Columns.Add(columnName);

            foreach (T entity in srcValues)
            {
                dataTable.Rows.Add(entity);
            }

            return dataTable;

        }
    }
}