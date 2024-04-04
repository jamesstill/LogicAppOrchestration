using System.Data.SqlClient;
using System.Data;
using TargetApi.Models;

namespace TargetApi.Repository
{
    public interface IPayloadRepository
    {
        Task<ReturnDto> PostPayload(PayloadDto payload);
    }

    public class PayloadRepository(IConfiguration configuration) : BaseRepository(configuration), IPayloadRepository
    {
        public async Task<ReturnDto> PostPayload(PayloadDto payload)
        {
            const string procedureName = "dbo.spPayloadDriver";

            SqlParameter[] parameters =
            [
                new SqlParameter { ParameterName = "@Payload", SqlDbType = SqlDbType.NVarChar, Size = -1, Value = payload.PayloadJSON },
                new SqlParameter { ParameterName = "@PayloadType", SqlDbType = SqlDbType.NVarChar, Size = 100, Value = payload.PayloadType },
                new SqlParameter { ParameterName = "@PayloadAction", SqlDbType = SqlDbType.NVarChar, Size = 100, Value = payload.PayloadAction },
                new SqlParameter { ParameterName = "@ExecutedBy", SqlDbType = SqlDbType.BigInt, Value = 999 },
                new SqlParameter { ParameterName = "@ReturnJSON", SqlDbType = SqlDbType.NVarChar, Size = -1, Direction = ParameterDirection.Output }
            ];

            List<SqlParameter> outputParameters = await ExecuteSqlCommand(procedureName, parameters);

            // deserialize output parameter into DTO
            SqlParameter parameter = outputParameters.First(p => p.ParameterName == "@ReturnJSON") ?? new SqlParameter();
            string returnJSON = parameter.Value.ToString() ?? string.Empty;
            return new ReturnDto { ReturnJSON = returnJSON };
        }
    }
}
