using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;

namespace tinhphicongchung.com.library
{
    public class Seos
    {
        #region Properties

        public string ActBy { get; set; }
        public int SeoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string CanonicalTag { get; set; }
        public string ImagePath { get; set; }
        public string H1Tag { get; set; }
        public string SeoFooter { get; set; }
        public byte StatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Seos()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Seos(string connection)
        {
            _connectionString = connection;
        }

        ~Seos()
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
                    if (!string.IsNullOrEmpty(this.Path))
                        param.Add("@Path", StringHelper.InjectionString(this.Path), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaTitle))
                        param.Add("@MetaTitle", StringHelper.InjectionString(this.MetaTitle), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaDescription))
                        param.Add("@MetaDescription", StringHelper.InjectionString(this.MetaDescription), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaKeyword))
                        param.Add("@MetaKeyword", StringHelper.InjectionString(this.MetaKeyword), DbType.String);
                    if (!string.IsNullOrEmpty(this.CanonicalTag))
                        param.Add("@CanonicalTag", StringHelper.InjectionString(this.CanonicalTag), DbType.String);
                    if (!string.IsNullOrEmpty(this.ImagePath))
                        param.Add("@ImagePath", StringHelper.InjectionString(this.ImagePath), DbType.String);
                    if (!string.IsNullOrEmpty(this.H1Tag))
                        param.Add("@H1Tag", StringHelper.InjectionString(this.H1Tag), DbType.String);
                    if (!string.IsNullOrEmpty(this.SeoFooter))
                        param.Add("@SeoFooter", StringHelper.InjectionString(this.SeoFooter), DbType.String);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    param.Add("@SeoId", this.SeoId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Seos_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.SeoId = param.Get<int>("SeoId");
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
                    param.Add("@SeoId", this.SeoId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Seos_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Seos> Get()
        {
            Seos resultVar = new Seos();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Seos>, int> tuple;
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

        public async Task<Tuple<List<Seos>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Seos> seosList = null;
            Tuple<List<Seos>, int> resultVar = Tuple.Create(seosList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@SeoId", this.SeoId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    if (!string.IsNullOrEmpty(this.Path))
                        param.Add("@Path", StringHelper.InjectionString(this.Path), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaTitle))
                        param.Add("@MetaTitle", StringHelper.InjectionString(this.MetaTitle), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaDescription))
                        param.Add("@MetaDescription", StringHelper.InjectionString(this.MetaDescription), DbType.String);
                    if (!string.IsNullOrEmpty(this.CanonicalTag))
                        param.Add("@CanonicalTag", StringHelper.InjectionString(this.CanonicalTag), DbType.String);
                    if (!string.IsNullOrEmpty(this.H1Tag))
                        param.Add("@H1Tag", StringHelper.InjectionString(this.H1Tag), DbType.String);
                    if (!string.IsNullOrEmpty(this.SeoFooter))
                        param.Add("@SeoFooter", StringHelper.InjectionString(this.SeoFooter), DbType.String);
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
                    
                    seosList = await connection
                        .QueryAsync<Seos>("Seos_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Seos>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(seosList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Seos>> GetList(byte statusId = 0)
        {
            List<Seos> resultVar = new List<Seos>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = "SELECT * FROM [dbo].[Seos] ORDER BY [SeoId]";

                    if(statusId > 0)
                    {
                        query = string.Format("SELECT * FROM [dbo].[Seos] WHERE [StatusId] = {0} ORDER BY [SeoId]", statusId);
                    }

                    resultVar = await connection.QueryAsync<Seos>(query) as List<Seos>;
                }
            }
            catch (Exception)
            {
                resultVar = null;
                throw;
            }

            return resultVar;
        }

        #endregion

        #region Static Methods

        public static async Task<List<Seos>> Static_GetList(byte statusId = 0)
        {
            Seos seos = new Seos();
            return await seos.GetList(statusId);
        }

        public static async Task<Seos> Static_GetById(string actBy, int seoId)
        {
            Seos seos = new Seos
            {
                ActBy = actBy,
                SeoId = seoId
            };

            return await seos.Get();
        }

        #endregion
    }
}
