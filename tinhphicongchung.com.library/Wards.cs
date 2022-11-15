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
    public class Wards
    {
        #region Properties

        public string ActBy { get; set; }
        public int WardId { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceDescription { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictDescription { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Wards()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Wards(string connection)
        {
            _connectionString = connection;
        }

        ~Wards()
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
                    if (!string.IsNullOrEmpty(this.Prefix))
                        param.Add("@Prefix", StringHelper.InjectionString(this.Prefix), DbType.String);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@ProvinceId", this.ProvinceId, DbType.Int32);
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@WardId", this.WardId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Wards_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.WardId = param.Get<int>("WardId");
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
                    param.Add("@WardId", this.WardId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Wards_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Wards> Get(byte isJoin = 0)
        {
            Wards resultVar = new Wards();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Wards>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, orderByClauseId, isJoin, pageIndex, pageSize);
                if (tuple != null && tuple.Item1 != null && tuple.Item1.Count > 0) resultVar = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<List<Wards>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, byte isJoin, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Wards> wardsList = null;
            Tuple<List<Wards>, int> resultVar = Tuple.Create(wardsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@WardId", this.WardId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@ProvinceId", this.ProvinceId, DbType.Int32);
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32);
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
                    param.Add("@IsJoin", isJoin, DbType.Byte);
                    param.Add("@PageSize", pageSize, DbType.Int32);
                    param.Add("@PageIndex", pageIndex, DbType.Int32);
                    param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    
                    wardsList = await connection
                        .QueryAsync<Wards>("Wards_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Wards>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(wardsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Wards>> GetList()
        {
            List<Wards> resultVar = new List<Wards>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    resultVar = await connection.QueryAsync<Wards>("SELECT * FROM [dbo].[Wards] ORDER BY [DisplayOrder]") as List<Wards>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Wards>> GetListByDistrict(int districtId, byte statusId = 0)
        {
            List<Wards> resultVar = new List<Wards>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = string.Format("SELECT [WardId],[Prefix],CASE WHEN [dbo].[NullOrEmpty]([Prefix])> 0  THEN CONCAT([Prefix], ' ', [Name]) ELSE [Name] END AS [Name],[Description],[ProvinceId],[DistrictId],[StatusId],[DisplayOrder],[CreatedBy],[CrDateTime],[UpdatedBy],[UpdDateTime] FROM [dbo].[Wards] WHERE [DistrictId] = {0} ORDER BY [DisplayOrder],[Name]", districtId);

                    if(statusId > 0)
                    {
                        query = String.Format("SELECT [WardId],[Prefix],CASE WHEN [dbo].[NullOrEmpty]([Prefix])> 0  THEN CONCAT([Prefix], ' ', [Name]) ELSE [Name] END AS [Name],[Description],[ProvinceId],[DistrictId],[StatusId],[DisplayOrder],[CreatedBy],[CrDateTime],[UpdatedBy],[UpdDateTime] FROM [dbo].[Wards] WHERE ([DistrictId] = {0}) AND ([StatusId] = {1}) ORDER BY [DisplayOrder],[Name]", districtId, statusId);
                    }
                    
                    resultVar = await connection.QueryAsync<Wards>(query) as List<Wards>;
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

        public static async Task<List<Wards>> Static_GetList()
        {
            Wards wards = new Wards();
            return await wards.GetList();
        }

        public static Wards Static_Get(int wardId, List<Wards> list)
        {
            Wards resultVar = new Wards();
            try
            {
                if (wardId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.WardId == wardId);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<List<Wards>> Static_GetListByDistrict(int district, byte statusId = 0)
        {
            Wards districts = new Wards();
            return await districts.GetListByDistrict(district, statusId);
        }
        public static async Task<Wards> Static_GetById(string actBy, int wardId, byte isJoin = 0)
        {
            Wards wards = new Wards
            {
                ActBy = actBy,
                WardId = wardId
            };
            return await wards.Get(isJoin);
        }

        #endregion
    }
}
