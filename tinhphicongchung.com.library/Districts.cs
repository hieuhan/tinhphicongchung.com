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
    public class Districts
    {
        #region Properties

        public string ActBy { get; set; }
        public int DistrictId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Districts()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Districts(string connection)
        {
            _connectionString = connection;
        }

        ~Districts()
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
                    param.Add("@ProvinceId", this.ProvinceId, DbType.Int32);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Districts_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.DistrictId = param.Get<int>("DistrictId");
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
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Districts_Delete", param, commandType: CommandType.StoredProcedure);
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

        public async Task<Districts> Get()
        {
            Districts resultVar = new Districts();
            byte searchByDateType = 0, joinProvinces = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Districts>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, orderByClauseId, joinProvinces, pageIndex, pageSize);
                if (tuple != null && tuple.Item1 != null && tuple.Item1.Count > 0) resultVar = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<List<Districts>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, byte joinProvinces, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Districts> districtsList = null;
            Tuple<List<Districts>, int> resultVar = Tuple.Create(districtsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@ProvinceId", this.ProvinceId, DbType.Int32);
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
                    param.Add("@JoinProvinces", joinProvinces, DbType.Byte);
                    param.Add("@PageSize", pageSize, DbType.Int32);
                    param.Add("@PageIndex", pageIndex, DbType.Int32);
                    param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    
                    districtsList = await connection
                        .QueryAsync<Districts>("Districts_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Districts>;
                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(districtsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Districts>> GetList(byte statusId = 0)
        {
            List<Districts> resultVar;
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    
                    string query = "SELECT * FROM [dbo].[Districts] ORDER BY [DisplayOrder],[Name]";
                    
                    if(statusId > 0)
                    {
                        query = string.Format("SELECT * FROM [dbo].[Districts] WHERE [StatusId] = {0} ORDER BY [DisplayOrder],[Name]", statusId);
                    }

                    resultVar = await connection.QueryAsync<Districts>(query) as List<Districts>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
  
            return resultVar;
        }

        public async Task<List<Districts>> GetListByProvince(int provinceId, byte statusId = 0)
        {
            List<Districts> resultVar;
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = string.Format("SELECT * FROM [dbo].[Districts] WHERE [ProvinceId] = {0} ORDER BY [DisplayOrder]", provinceId);

                    if(statusId > 0)
                    {
                        query = string.Format("SELECT * FROM [dbo].[Districts] WHERE ([ProvinceId] = {0}) AND ([StatusId] = {1}) ORDER BY [DisplayOrder]", provinceId, statusId);
                    }

                    resultVar = await connection.QueryAsync<Districts>(query) as List<Districts>;
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

        public static async Task<List<Districts>> Static_GetList(byte statusId = 0)
        {
            Districts districts = new Districts();
            return await districts.GetList(statusId);
        }

        public static async Task<List<Districts>> Static_GetListByProvince(int provinceId, byte statusId = 0)
        {
            Districts districts = new Districts();
            return await districts.GetListByProvince(provinceId, statusId);
        }

        public static Districts Static_Get(byte districtId, List<Districts> list)
        {
            Districts resultVar = new Districts();
            try
            {
                if (districtId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.DistrictId == districtId);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public static async Task<Districts> Static_GetById(string actBy, int districtId)
        {
            Districts districts = new Districts
            {
                ActBy = actBy,
                DistrictId = districtId
            };
            return await districts.Get();
        }

        #endregion
    }
}
