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
    public class Lands
    {
        #region Properties

        public string ActBy { get; set; }
        public int LandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ConstructionLevelId { get; set; }
        public string ConstructionLevelName { get; set; }
        public string ConstructionLevelDescription { get; set; }
        public double Price { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Lands()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Lands(string connection)
        {
            _connectionString = connection;
        }

        ~Lands()
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
                    param.Add("@ConstructionLevelId", this.ConstructionLevelId, DbType.Int32);
                    param.Add("@Price", this.Price, DbType.Decimal);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@LandId", this.LandId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Lands_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.LandId = param.Get<int>("LandId");
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
                    param.Add("@LandId", this.LandId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Lands_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Lands> Get(byte joinConstructionLevels = 0)
        {
            Lands resultVar = new Lands();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Lands>, int> tuple;
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

        public async Task<Tuple<List<Lands>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, byte joinConstructionLevels, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Lands> landsList = null;
            Tuple<List<Lands>, int> resultVar = Tuple.Create(landsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@LandId", this.LandId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@ConstructionLevelId", this.ConstructionLevelId, DbType.Int32);
                    param.Add("@Price", this.Price, DbType.Decimal);
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
                    
                    landsList = await connection
                        .QueryAsync<Lands>("Lands_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Lands>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(landsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Lands>> GetList(byte statusId = 0)
        {
            List<Lands> resultVar = new List<Lands>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = "SELECT A.[LandId],A.[Name],CASE WHEN ISNULL(A.[Description], '') = '' THEN A.[Name] ELSE CONCAT(B.[Name], ': ' , A.[Description]) END AS [Description],A.[ConstructionLevelId],A.[Price] FROM [dbo].[Lands] A INNER JOIN [dbo].[ConstructionLevels] B ON A.[ConstructionLevelId] = B.[ConstructionLevelId] ORDER BY A.[DisplayOrder],A.[Name]";

                    if(statusId > 0)
                    {
                        query = string.Format("SELECT A.[LandId],A.[Name],CASE WHEN ISNULL(A.[Description], '') = '' THEN A.[Name] ELSE CONCAT(B.[Name], ': ' , A.[Description]) END AS [Description],A.[ConstructionLevelId],A.[Price] FROM [dbo].[Lands] A INNER JOIN [dbo].[ConstructionLevels] B ON A.[ConstructionLevelId] = B.[ConstructionLevelId] WHERE A.[StatusId] = {0} ORDER BY A.[DisplayOrder],A.[Name]", statusId);
                    }    

                    resultVar = await connection.QueryAsync<Lands>(query) as List<Lands>;
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

        public static async Task<List<Lands>> Static_GetList(byte statusId = 0)
        {
            Lands lands = new Lands();
            return await lands.GetList(statusId);
        }

        public static Lands Static_Get(byte landId, List<Lands> list)
        {
            Lands resultVar = new Lands();
            try
            {
                if (landId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.LandId == landId) ?? new Lands();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<Lands> Static_GetById(string actBy, int landId, byte joinConstructionLevels = 0)
        {
            Lands lands = new Lands
            {
                ActBy = actBy,
                LandId = landId
            };
            return await lands.Get(joinConstructionLevels);
        }

        #endregion
    }
}
