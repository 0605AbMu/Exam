using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace StorageBroker
{
    public class DBWrapper<T> : DataBase
    {
        public DBWrapper(string connectionString) : base(connectionString)
        {
        }

        public virtual async Task Delete(string condition)
        {
            SqlCommand sqlCommand = new SqlCommand($"DELETE FROM {this.tableName} WHERE {condition}");
            await ExecuteQueryNonResultAsync(sqlCommand);
        }

        public virtual async Task CreateTable(bool isOldTableDropped = true)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            SqlCommand sqlCommand =
                new SqlCommand(
                    $"{(isOldTableDropped ? $"DROP TABLE IF EXISTS {this.tableName}; \n" : "")}CREATE TABLE {this.tableName} ({String.Join(",\n", props.Select(x => DotnetTypeConvertToSQLTypeQuery(x)))})");
            await ExecuteQueryNonResultAsync(sqlCommand);
        }

        public virtual async Task DropTable()
        {
            SqlCommand sqlCommand = new($"DROP TABLE IF EXISTS {this.tableName};");
            await ExecuteQueryNonResultAsync(sqlCommand);
        }

        private string DotnetTypeConvertToSQLTypeQuery(PropertyInfo prop)
        {
            var IdentityAttr = prop.GetCustomAttribute<Identity>();
            var NotNullAttr = prop.GetCustomAttribute<NotNull>();
            var UniqueAttr = prop.GetCustomAttribute<Unique>();
            Console.WriteLine(UniqueAttr);
            return
                $"{prop.Name} {DonetTypeNamesToSqlTypeName(prop.PropertyType)} {(NotNullAttr != null ? "NOT NULL" : "")} {(IdentityAttr != null ? $"IDENTITY({IdentityAttr.start},{IdentityAttr.added})" : "")} {(UniqueAttr != null ? "UNIQUE" : "")}";
        }

        public virtual async Task<IList<T>> ExecuteCustomeQuery(string query)
        {
            return await ExecuteQueryAndReturnAsAsync<T>(new SqlCommand(query), typeof(T));
        }

        private string DonetTypeNamesToSqlTypeName(Type type)
        {
            //Special check for some types
            if (type.IsEnum)
                return "int";
            switch (type.FullName)
            {
                case "System.Int32":
                    return "int";
                    break;
                case "System.Int64":
                    return "bigint";
                    break;
                case "System.Int16":
                    return "smallint";
                    break;
                case "System.Byte":
                    return "tinyint";
                    break;
                case "System.Decimal":
                    return "decimal";
                    break;
                case "System.DateTime":
                    return "datetime";
                    break;
                case "System.Boolean":
                    return "bit";
                    break;
                case "System.String":
                default:
                    return $"nvarchar({byte.MaxValue})";
                    break;
            }
        }
    }
}