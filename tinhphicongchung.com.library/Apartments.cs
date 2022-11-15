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
    public class Apartments
    {
        #region Properties

        public string ActBy { get; set; }
        public int ApartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Cấp công trình
        /// </summary>
        public int ConstructionLevelId { get; set; }

        public string ConstructionLevelName { get; set; }
        public string ConstructionLevelDescription { get; set; }

        /// <summary>
        /// Đơn giá nhà Chung cư
        /// </summary>
        public double ApartmentUnitPrice { get; set; }

        /// <summary>
        /// Đơn giá nhà Đa năng
        /// </summary>
        public double MultiPurposeHouseUnitPrice { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Apartments()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Apartments(string connection)
        {
            _connectionString = connection;
        }

        ~Apartments()
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
                    param.Add("@ApartmentUnitPrice", this.ApartmentUnitPrice, DbType.Decimal);
                    param.Add("@MultiPurposeHouseUnitPrice", this.MultiPurposeHouseUnitPrice, DbType.Decimal);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@ApartmentId", this.ApartmentId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Apartments_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.ApartmentId = param.Get<int>("ApartmentId");
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
                    param.Add("@ApartmentId", this.ApartmentId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Apartments_Delete", param, commandType: CommandType.StoredProcedure);
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

        public async Task<Apartments> Get(byte joinConstructionLevels = 0)
        {
            Apartments resultVar = new Apartments();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Apartments>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, orderByClauseId, joinConstructionLevels, pageIndex, pageSize);
                if (tuple.Item1 != null && tuple.Item1.Count > 0) resultVar = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<List<Apartments>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, byte joinConstructionLevels, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Apartments> apartmentsList = null;
            Tuple<List<Apartments>, int> resultVar = Tuple.Create(apartmentsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@ApartmentId", this.ApartmentId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@ConstructionLevelId", this.ConstructionLevelId, DbType.Int32);
                    param.Add("@ApartmentUnitPrice", this.ApartmentUnitPrice, DbType.Decimal);
                    param.Add("@MultiPurposeHouseUnitPrice", this.MultiPurposeHouseUnitPrice, DbType.Decimal);
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
                    
                    apartmentsList = await connection
                        .QueryAsync<Apartments>("Apartments_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Apartments>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(apartmentsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Apartments>> GetList(byte statusId = 0)
        {
            List<Apartments> resultVar = new List<Apartments>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = "SELECT A.[ApartmentId],A.[Name],CONCAT(B.[Name], ': ' , A.[Description]) AS [Description],A.[ConstructionLevelId],A.[ApartmentUnitPrice],A.[MultiPurposeHouseUnitPrice] FROM [dbo].[Apartments] A INNER JOIN [dbo].[ConstructionLevels] B ON A.[ConstructionLevelId] = B.[ConstructionLevelId] ORDER BY [DisplayOrder],[Name]";
                    if(statusId > 0)
                    {
                        query = string.Format("SELECT A.[ApartmentId],A.[Name],CONCAT(B.[Name], ': ' , A.[Description]) AS [Description],A.[ConstructionLevelId],A.[ApartmentUnitPrice],A.[MultiPurposeHouseUnitPrice] FROM [dbo].[Apartments] A INNER JOIN [dbo].[ConstructionLevels] B ON A.[ConstructionLevelId] = B.[ConstructionLevelId] WHERE A.[StatusId] = {0} ORDER BY A.[DisplayOrder],A.[Name]", statusId);
                    }

                    resultVar = await connection.QueryAsync<Apartments>(query) as List<Apartments>;
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

        public static async Task<List<Apartments>> Static_GetList(byte statusId = 0)
        {
            Apartments apartments = new Apartments();
            return await apartments.GetList(statusId);
        }

        public static Apartments Static_Get(byte apartmentId, List<Apartments> list)
        {
            Apartments resultVar = new Apartments();
            try
            {
                if (apartmentId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.ApartmentId == apartmentId) ?? new Apartments();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<Apartments> Static_GetById(string actBy, int apartmentId, byte joinConstructionLevels = 0)
        {
            Apartments apartments = new Apartments
            {
                ActBy = actBy,
                ApartmentId = apartmentId
            };
            return await apartments.Get(joinConstructionLevels);
        }

        #endregion
    }
}
