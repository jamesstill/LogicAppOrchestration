using System.Data;
using System.Data.SqlClient;
using TargetApi.Models;

namespace TargetApi.Repository
{
    public interface IWidgetRepository
    {
        Task<IEnumerable<WidgetDto>> GetAllWidgets();

        Task<WidgetDto> GetWidget(long id);
    }

    public class WidgetRepository(IConfiguration configuration) : BaseRepository(configuration), IWidgetRepository
    {
        /// <summary>
        /// Get all widgets
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WidgetDto>> GetAllWidgets()
        {
            string procedureName = "dbo.spGetWidgets";

            using var cn = new SqlConnection(_configuration.GetConnectionString("PrimaryConnectionString"));

            var cm = new SqlCommand(procedureName, cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            List<WidgetDto> list = [];

            await cn.OpenAsync();
            SqlDataReader reader = await cm.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var item = new WidgetDto()
                    {
                        Id = reader.GetInt64(0),
                        Name = reader.GetString(1),
                        Shape = reader.GetString(2),
                        Color = reader.GetString(3)
                    };

                    list.Add(item);
                }
            }

            await reader.CloseAsync();

            if (cn.State == ConnectionState.Open)
            {
                await cn.CloseAsync();
            }

            return list;
        }

        /// <summary>
        /// Get a single widget by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WidgetDto> GetWidget(long id)
        {
            var list = await GetAllWidgets();
            return list.SingleOrDefault(i => i.Id == id) ?? new WidgetDto();
        }
    }
}
