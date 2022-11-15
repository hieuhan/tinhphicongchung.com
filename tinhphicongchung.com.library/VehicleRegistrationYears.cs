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
    public class VehicleRegistrationYears
    {
        #region Properties

        public string ActBy { get; set; }
        public int VehicleRegistrationYearId { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public VehicleRegistrationYears()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public VehicleRegistrationYears(string connection)
        {
            _connectionString = connection;
        }

        ~VehicleRegistrationYears()
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
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@Value", this.Value, DbType.Int32);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@VehicleRegistrationYearId", this.VehicleRegistrationYearId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("VehicleRegistrationYears_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.VehicleRegistrationYearId = param.Get<int>("VehicleRegistrationYearId");
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
                    param.Add("@VehicleRegistrationYearId", this.VehicleRegistrationYearId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("VehicleRegistrationYears_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<VehicleRegistrationYears> Get()
        {
            VehicleRegistrationYears resultVar = new VehicleRegistrationYears();
            string keywords = string.Empty;
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<VehicleRegistrationYears>, int> tuple;
            try
            {
                tuple = await GetPage(keywords, dateFrom, dateTo, searchByDateType, orderByClauseId, pageIndex, pageSize);
                if (tuple != null && tuple.Item1 != null && tuple.Item1.Count > 0) resultVar = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<List<VehicleRegistrationYears>, int>> GetPage(string keywords, DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<VehicleRegistrationYears> vehicleRegistrationYearsList = null;
            Tuple<List<VehicleRegistrationYears>, int> resultVar = Tuple.Create(vehicleRegistrationYearsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    if (!string.IsNullOrEmpty(keywords))
                        param.Add("@Keywords", keywords.InjectionString(), DbType.String);
                    param.Add("@VehicleRegistrationYearId", this.VehicleRegistrationYearId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@Value", this.Value, DbType.Int32);
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

                    vehicleRegistrationYearsList = await connection
                        .QueryAsync<VehicleRegistrationYears>("VehicleRegistrationYears_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<VehicleRegistrationYears>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(vehicleRegistrationYearsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<VehicleRegistrationYears>> GetList(byte statusId = 0)
        {
            List<VehicleRegistrationYears> resultVar = new List<VehicleRegistrationYears>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = "SELECT * FROM [dbo].[VehicleRegistrationYears] ORDER BY [DisplayOrder],[Value]";

                    if (statusId > 0)
                    {
                        query = string.Format("SELECT * FROM [dbo].[VehicleRegistrationYears] WHERE [StatusId] = {0} ORDER BY [DisplayOrder],[Value]", statusId);
                    }

                    resultVar = await connection.QueryAsync<VehicleRegistrationYears>(query) as List<VehicleRegistrationYears>;
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

        public static async Task<List<VehicleRegistrationYears>> Static_GetList(byte statusId = 0)
        {
            VehicleRegistrationYears vehicleRegistrationYears = new VehicleRegistrationYears();
            return await vehicleRegistrationYears.GetList(statusId);
        }

        public static VehicleRegistrationYears Static_Get(byte vehicleRegistrationYearId, List<VehicleRegistrationYears> list)
        {
            VehicleRegistrationYears resultVar = new VehicleRegistrationYears();
            try
            {
                if (vehicleRegistrationYearId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.VehicleRegistrationYearId == vehicleRegistrationYearId) ?? new VehicleRegistrationYears();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<VehicleRegistrationYears> Static_GetById(string actBy, int vehicleRegistrationYearId)
        {
            VehicleRegistrationYears vehicleRegistrationYears = new VehicleRegistrationYears
            {
                ActBy = actBy,
                VehicleRegistrationYearId = vehicleRegistrationYearId
            };

            return await vehicleRegistrationYears.Get();
        }

        #endregion
    }
}
