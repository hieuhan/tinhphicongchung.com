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
    public class Streets
    {
        #region Properties

        public string ActBy { get; set; }
        public int StreetId { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public string WardDescription { get; set; }
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

        public Streets()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Streets(string connection)
        {
            _connectionString = connection;
        }

        ~Streets()
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
                    param.Add("@WardId", this.WardId, DbType.Int32);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@StreetId", this.StreetId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Streets_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.StreetId = param.Get<int>("StreetId");
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
                    param.Add("@StreetId", this.StreetId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Streets_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Streets> Get(byte isJoin = 0)
        {
            Streets resultVar = new Streets();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Streets>, int> tuple;
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

        public async Task<Tuple<List<Streets>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, byte isJoin, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Streets> streetsList = null;
            Tuple<List<Streets>, int> resultVar = Tuple.Create(streetsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@StreetId", this.StreetId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@ProvinceId", this.ProvinceId, DbType.Int32);
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32);
                    param.Add("@WardId", this.WardId, DbType.Int32);
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
                    
                    streetsList = await connection
                        .QueryAsync<Streets>("Streets_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Streets>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(streetsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Streets>> GetListByWard(int wardId, byte statusId = 0)
        {
            List<Streets> resultVar = new List<Streets>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = string.Format("SELECT [StreetId],[Prefix],CASE WHEN [dbo].[NullOrEmpty]([Prefix]) > 0 THEN CONCAT([Prefix], ' ', [Name]) ELSE [Name] END AS [Name],[Description],[ProvinceId],[DistrictId],[WardId],[StatusId],[DisplayOrder],[CreatedBy],[CrDateTime],[UpdatedBy],[UpdDateTime] FROM [dbo].[Streets] WHERE [WardId] = {0} ORDER BY [DisplayOrder],[Name]", wardId);

                    if(statusId > 0)
                    {
                        query = string.Format("SELECT [StreetId],[Prefix],CASE WHEN [dbo].[NullOrEmpty]([Prefix]) > 0 THEN CONCAT([Prefix], ' ', [Name]) ELSE [Name] END AS [Name],[Description],[ProvinceId],[DistrictId],[WardId],[StatusId],[DisplayOrder],[CreatedBy],[CrDateTime],[UpdatedBy],[UpdDateTime] FROM [dbo].[Streets] WHERE ([WardId] = {0}) AND ([StatusId] = {1}) ORDER BY [DisplayOrder],[Name]", wardId, statusId);
                    }    

                    resultVar = await connection.QueryAsync<Streets>(query) as List<Streets>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Streets>> GetListByWardDisplay(int wardId)
        {
            List<Streets> resultVar = new List<Streets>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = string.Format("SELECT [StreetId],[Prefix],CASE WHEN [dbo].[NullOrEmpty]([Prefix]) > 0 THEN CONCAT([Prefix], ' ', [Name]) ELSE [Name] END AS [Name],[Description],[ProvinceId],[DistrictId],[WardId],[StatusId],[DisplayOrder],[CreatedBy],[CrDateTime],[UpdatedBy],[UpdDateTime] FROM [dbo].[Streets] WHERE ([WardId] = {0}) AND ([StreetId] IN (SELECT [StreetId] FROM [dbo].[Locations] WHERE [StatusId] = {1})) ORDER BY [DisplayOrder],[Name]", wardId, ConstantHelper.StatusIdActivated);

                    resultVar = await connection.QueryAsync<Streets>(query) as List<Streets>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Streets>> GetList()
        {
            List<Streets> resultVar = new List<Streets>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    resultVar = await connection.QueryAsync<Streets>("SELECT * FROM [dbo].[Streets] ORDER BY [DisplayOrder],[Name]") as List<Streets>;
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

        public static async Task<List<Streets>> Static_GetList()
        {
            Streets streets = new Streets();
            return await streets.GetList();
        }

        public static async Task<List<Streets>> Static_GetListByWard(int wardId, byte statusId = 0)
        {
            Streets streets = new Streets();
            return await streets.GetListByWard(wardId, statusId);
        }

        public static async Task<List<Streets>> Static_GetListByWardDisplay(int wardId)
        {
            Streets streets = new Streets();
            return await streets.GetListByWardDisplay(wardId);
        }

        public static Streets Static_Get(byte streetId, List<Streets> list)
        {
            Streets resultVar = new Streets();
            try
            {
                if (streetId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.StreetId == streetId) ?? new Streets();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<Streets> Static_GetById(string actBy, int streetId, byte isJoin = 0)
        {
            Streets streets = new Streets
            {
                ActBy = actBy,
                StreetId = streetId
            };
            return await streets.Get(isJoin);
        }

        #endregion
    }
}
