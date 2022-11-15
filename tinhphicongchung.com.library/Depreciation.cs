using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;

namespace tinhphicongchung.com.library
{
    public class Depreciation
    {
        #region Properties

        public string ActBy { get; set; }
        public int DepreciationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public double Value { get; set; }
        public double Price { get; set; }
        public int ConstructionLevelId { get; set; }
        public string ConstructionLevelName { get; set; }
        public string ConstructionLevelDescription { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Depreciation()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Depreciation(string connection)
        {
            _connectionString = connection;
        }

        ~Depreciation()
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
                    param.Add("@MinValue", this.MinValue, DbType.Int32);
                    param.Add("@MaxValue", this.MaxValue, DbType.Int32);
                    param.Add("@Price", this.Price, DbType.Double);
                    param.Add("@ConstructionLevelId", this.ConstructionLevelId, DbType.Int32);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@DepreciationId", this.DepreciationId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Depreciation_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.DepreciationId = param.Get<int>("DepreciationId");
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
                    param.Add("@DepreciationId", this.DepreciationId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Depreciation_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Depreciation> Get(byte joinConstructionLevels = 0)
        {
            Depreciation resultVar = new Depreciation();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Depreciation>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, orderByClauseId, joinConstructionLevels, pageIndex, pageSize);
                if (tuple != null && tuple.Item1 != null && tuple.Item1.Count > 0) resultVar = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<List<Depreciation>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, byte joinConstructionLevels, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Depreciation> depreciationList = null;
            Tuple<List<Depreciation>, int> resultVar = Tuple.Create(depreciationList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@DepreciationId", this.DepreciationId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@MinValue", this.MinValue, DbType.Int32);
                    param.Add("@MaxValue", this.MaxValue, DbType.Int32);
                    param.Add("@Price", this.Price, DbType.Decimal);
                    param.Add("@ConstructionLevelId", this.ConstructionLevelId, DbType.Int32);
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
                    param.Add("@JoinConstructionLevels", joinConstructionLevels, DbType.Byte);
                    param.Add("@PageSize", pageSize, DbType.Int32);
                    param.Add("@PageIndex", pageIndex, DbType.Int32);
                    param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    
                    depreciationList = await connection
                        .QueryAsync<Depreciation>("Depreciation_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Depreciation>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(depreciationList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            return resultVar;
        }

        public async Task<List<Depreciation>> GetList()
        {
            List<Depreciation> resultVar = new List<Depreciation>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    resultVar = await connection.QueryAsync<Depreciation>("SELECT * FROM [dbo].[Depreciation] ORDER BY [DisplayOrder],[Name]") as List<Depreciation>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public async Task<Depreciation> GetBy()
        {
            Depreciation resultVar = new Depreciation();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@Value", this.Value, DbType.Double);
                    param.Add("@ConstructionLevelId", this.ConstructionLevelId, DbType.Int32);

                    List<Depreciation> depreciationList = await connection
                        .QueryAsync<Depreciation>("Depreciation_GetPage", param, commandType: CommandType.StoredProcedure) as List<Depreciation>;

                    if(depreciationList != null && depreciationList.Count > 0)
                    {
                        resultVar = depreciationList[0];
                    }
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

        public static async Task<List<Depreciation>> Static_GetList()
        {
            Depreciation depreciation = new Depreciation();
            return await depreciation.GetList();
        }

        public static async Task<Depreciation> Static_GetBy(int constructionLevelId, double value)
        {
            Depreciation depreciation = new Depreciation
            {
                ConstructionLevelId = constructionLevelId,
                Value = value
            };

            return await depreciation.GetBy();
        }

        public static Depreciation Static_Get(byte depreciationId, List<Depreciation> list)
        {
            Depreciation resultVar = new Depreciation();
            try
            {
                if (depreciationId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.Find(i => i.DepreciationId == depreciationId) ?? new Depreciation();
                }
            }
            catch (Exception)
            {
                resultVar = null;
                throw;
            }
            
            return resultVar;
        }

        public static async Task<Depreciation> Static_GetById(string actBy, int depreciationId, byte joinConstructionLevels = 0)
        {
            Depreciation depreciation = new Depreciation
            {
                ActBy = actBy,
                DepreciationId = depreciationId
            };
            return await depreciation.Get(joinConstructionLevels);
        }

        #endregion
    }
}
