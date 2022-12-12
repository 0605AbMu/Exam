using System.Data.SqlClient;

namespace StorageBroker;

public class DataBaseTool
{
    private string connectionString = "";

    public DataBaseTool(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DataBaseTool(SqlConnectionStringBuilder connectionStringBuilder)
    {
        this.connectionString = connectionStringBuilder.ToString();
    }

    /// <summary>
    /// Commandni SQL uchun ishga tushiradi va qaytgan natijani berilgan toifaga cast qilib listini oladi
    /// </summary>
    /// <param name="command">command</param>
    /// <returns>Bajarilgan command natijasining berilgan turdagi list ko'rinishi</returns>
    public async Task<IList<T>> ExecuteQueryAndReturnAsAsync<T>(SqlCommand command, Type returnType)
    {
        IList<T> result = new List<T>();
        using SqlConnection connection = new SqlConnection(this.connectionString);
        command.Connection = connection;
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            T obj = (T) Activator.CreateInstance(returnType);
            if (obj == null)
                continue;

            var objProps = obj.GetType().GetProperties();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var propName = objProps.Where(x => x.Name.ToLowerInvariant() == reader.GetName(i).ToLowerInvariant())
                    .ElementAtOrDefault(0);
                if (propName == null)
                    continue;
                if (propName.GetType().Equals(reader.GetFieldType(i)))
                    continue;
                if (propName.PropertyType.IsEnum)
                    propName.SetValue(obj, reader.GetFieldValue<int>(i));
                else if (reader.GetValue(i).GetType() == typeof(DBNull))
                    propName.SetValue(obj, default);
                else
                    propName.SetValue(obj, Convert.ChangeType(reader.GetValue(i), propName.PropertyType));
            }

            result.Add(obj);
        }

        return result;
    }

    /// <summary>
    /// Commandni SQL uchun ishga tushiradi
    /// </summary>
    /// <param name="command">command</param>
    /// <returns>Non resulted Task</returns>
    public async Task ExecuteQueryNonResultAsync(SqlCommand command)
    {
        IList<Object> result = new List<Object>();
        using SqlConnection connection = new SqlConnection(connectionString);
        command.Connection = connection;
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
    }

    /// <summary>
    /// Query paramterlariga belgialngan qiymatlarni qo'shadi va yangi SQL command yaratadi
    /// </summary>
    /// <param name="query">Query</param>
    /// <param name="propNames">Obyekt property nomlari ro'yxati</param>
    /// <param name="obj">Berilayotgan obyekt</param>
    /// <returns>SQL Command</returns>
    public SqlCommand AssignValuesToQueryAndReturnsCommand(string query, string[] propNames, Object obj)
    {
        SqlCommand command = new(query);
        foreach (var item in propNames)
        {
            SqlParameter parameter = new SqlParameter("@" + item, obj.GetType().GetProperty(item).GetValue(obj, null));
            command.Parameters.Add(parameter);
        }

        return command;
    }

    /// <summary>
    /// Query paramterlariga belgialngan qiymatlarni qo'shadi va yangi SQL command yaratadi
    /// </summary>
    /// <typeparam name="T">Generic uchun</typeparam>
    /// <param name="query">Query</param>
    /// <param name="propNames">Obyekt property nomlari ro'yxati</param>
    /// <param name="obj">Berilayotgan obyekt</param>
    /// <returns>SQL Command</returns>
    public SqlCommand AssignValuesToQueryAndReturnsCommand<T>(string query, string[] propNames, T obj)
    {
        SqlCommand command = new(query);
        foreach (var item in propNames)
        {
            SqlParameter parameter = new SqlParameter("@" + item, obj.GetType().GetProperty(item).GetValue(obj, null));
            command.Parameters.Add(parameter);
        }

        return command;
    }

    /// <summary>
    /// Berilgan obyektning propertiylar nomi ro'yxatini olib beradi
    /// </summary>
    /// <param name="obj">Berilgan obyekt</param>
    /// <exception cref="Exception">Toifani cast qilish va boshqa xatolar</exception>
    /// <returns>Parameter nomlari ro'yxati</returns>
    public string[] GetPropNamesFromObject(Object obj)
    {
        return obj.GetType().GetProperties()
            .Where(x => (obj.GetType().GetProperty(x.Name).GetCustomAttributes(typeof(ExcludeFromSQL), true).Count() ==
                         0))
            .Select(x => x.Name).ToArray();
    }
}