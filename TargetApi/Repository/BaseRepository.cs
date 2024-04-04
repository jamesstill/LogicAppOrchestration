using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace TargetApi.Repository
{
    public class BaseRepository(IConfiguration configuration)
    {
        protected readonly IConfiguration _configuration = configuration;

        protected async Task<List<SqlParameter>> ExecuteSqlCommand(string procedureName, SqlParameter[] parameters)
        {
            using var cn = new SqlConnection(_configuration.GetConnectionString("PrimaryConnectionString"));

            var cm = new SqlCommand(procedureName, cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                cm.Parameters.AddRange(parameters);

                await cn.OpenAsync();
                await cm.ExecuteNonQueryAsync();
                await cn.CloseAsync();

                // return all output parameters
                return cm.Parameters
                    .Cast<SqlParameter>()
                    .Where(p => p.Direction == ParameterDirection.Output)
                    .ToList();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    await cn.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Given output parameters that holds a JSON output for a database entity, 
        /// deserializes into a POCO list and return it.
        /// </summary>
        /// <param name="outputParameters"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected static List<T> DeserializeDbEntityList<T>(List<SqlParameter> outputParameters, string parameterName) where T : class
        {
            List<T> list = [];
            SqlParameter parameter = outputParameters.Find(p => p.ParameterName == parameterName) ?? new SqlParameter();
            string payload = parameter.Value.ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(payload))
            {
                var options = SerializerOptions.GetJsonSerializerOptions();
                list = JsonSerializer.Deserialize<List<T>>(payload, options)!;
            }

            return list;
        }
    }
}
