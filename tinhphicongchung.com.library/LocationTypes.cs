using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;

namespace tinhphicongchung.com.library
{
    public class LocationTypes
    {
        #region Properties

        public string ActBy { get; set; }
        public int LocationTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public int LocationId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public LocationTypes()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public LocationTypes(string connection)
        {
            _connectionString = connection;
        }

        ~LocationTypes()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods
        public async Task<Tuple<string, string>> Insert()
        {
            try
            {
                return await InsertOrUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<string, string>> Update()
        {
            try
            {
                return await InsertOrUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<string, string>> InsertOrUpdate()
        {
            string actionStatus = string.Empty, actionMessage = string.Empty;
            Tuple<string, string> resultVar = Tuple.Create(actionStatus, actionMessage);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@LocationTypeId", this.LocationTypeId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("LocationTypes_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.LocationTypeId = param.Get<int>("LocationTypeId");
                    actionStatus = param.Get<string>("ActionStatus");
                    actionMessage = param.Get<string>("ActionMessage");

                    resultVar = Tuple.Create(actionStatus, actionMessage);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<string, string>> Delete()
        {
            string actionStatus = string.Empty, actionMessage = string.Empty;
            Tuple<string, string> resultVar = Tuple.Create(actionStatus, actionMessage);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@LocationTypeId", this.LocationTypeId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("LocationTypes_Delete", param, commandType: CommandType.StoredProcedure);
                    
                    actionStatus = param.Get<string>("ActionStatus");
                    actionMessage = param.Get<string>("ActionMessage");

                    resultVar = Tuple.Create(actionStatus, actionMessage);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<LocationTypes> Get()
        {
            LocationTypes resultVar = new LocationTypes();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<LocationTypes>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, orderByClauseId, pageIndex, pageSize);
                if (tuple != null && tuple.Item1 != null && tuple.Item1.Count > 0) resultVar = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<List<LocationTypes>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<LocationTypes> locationTypesList = null;
            Tuple<List<LocationTypes>, int> resultVar = Tuple.Create(locationTypesList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@LocationTypeId", this.LocationTypeId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (!string.IsNullOrEmpty(this.CreatedBy))
                        param.Add("@CreatedBy", this.CreatedBy, DbType.String);
                    if (!string.IsNullOrEmpty(this.UpdatedBy))
                        param.Add("@UpdatedBy", this.UpdatedBy, DbType.String);
                    param.Add("@SearchByDateType", searchByDateType, DbType.Byte);
                    if (dateFrom != DateTime.MinValue)
                        param.Add("@DateFrom", dateFrom, DbType.DateTime);
                    if (dateTo != DateTime.MinValue)
                        param.Add("@DateTo", dateTo, DbType.DateTime);
                    param.Add("@OrderByClauseId", orderByClauseId, DbType.Int32);
                    param.Add("@PageSize", pageSize, DbType.Int32);
                    param.Add("@PageIndex", pageIndex, DbType.Int32);
                    param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    
                    locationTypesList = await connection
                        .QueryAsync<LocationTypes>("LocationTypes_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<LocationTypes>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(locationTypesList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<LocationTypes>> GetBy(byte landTypeId, int streetId)
        {
            List<LocationTypes> resultVar = new List<LocationTypes>();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@LandTypeId", landTypeId, DbType.Byte);
                    param.Add("@StreetId", streetId, DbType.Int32);

                    resultVar = await connection
                        .QueryAsync<LocationTypes>("LocationTypes_GetBy", param, commandType: CommandType.StoredProcedure)
                        as List<LocationTypes>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<LocationTypes>> GetList(byte statusId = 0)
        {
            List<LocationTypes> resultVar = new List<LocationTypes>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = "SELECT [LocationTypeId], [Name], CONCAT([Name],' - ', [Description]) AS [Description],[StatusId],[DisplayOrder] FROM [dbo].[LocationTypes] ORDER BY [DisplayOrder],[Name]";

                    if(statusId > 0)
                    {
                        query = string.Format("SELECT [LocationTypeId], [Name], CONCAT([Name],' - ', [Description]) AS [Description],[StatusId],[DisplayOrder] FROM [dbo].[LocationTypes] WHERE [StatusId] = {0} ORDER BY [DisplayOrder],[Name]", statusId);
                    }

                    resultVar = await connection.QueryAsync<LocationTypes>(query) as List<LocationTypes>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        #endregion

        #region Static Methods

        public static async Task<List<LocationTypes>> Static_GetList(byte statusId = 0)
        {
            LocationTypes locationTypes = new LocationTypes();
            return await locationTypes.GetList(statusId);
        }

        public static async Task<List<LocationTypes>> Static_GetBy(byte landTypeId, int streetId)
        {
            LocationTypes locationTypes = new LocationTypes();
            return await locationTypes.GetBy(landTypeId, streetId);
        }

        public static LocationTypes Static_Get(int locationTypeId, List<LocationTypes> list)
        {
            LocationTypes resultVar = new LocationTypes();
            try
            {
                if (locationTypeId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.LocationTypeId == locationTypeId) ?? new LocationTypes();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            return resultVar;
        }

        public static async Task<LocationTypes> Static_GetById(string actBy, int locationTypeId)
        {
            LocationTypes locationTypes = new LocationTypes
            {
                ActBy = actBy,
                LocationTypeId = locationTypeId
            };
            return await locationTypes.Get();
        }

        #endregion
    }
}
