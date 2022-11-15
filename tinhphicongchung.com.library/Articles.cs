using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;

namespace tinhphicongchung.com.library
{
    public class Articles
    {
        #region Properties

        public string ActBy { get; set; }
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ArticleContent { get; set; }
        public string ImagePath { get; set; }
        public string ArticleUrl { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public DateTime PublishTime { get; set; }
        public int? DisplayOrder { get; set; }
        public byte ReviewStatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime RevDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Articles()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Articles(string connection)
        {
            _connectionString = connection;
        }

        ~Articles()
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
                    if (!string.IsNullOrEmpty(this.Title))
                        param.Add("@Title", StringHelper.InjectionString(this.Title), DbType.String);
                    if (!string.IsNullOrEmpty(this.Summary))
                        param.Add("@Summary", StringHelper.InjectionString(this.Summary), DbType.String);
                    if (!string.IsNullOrEmpty(this.ArticleContent))
                        param.Add("@ArticleContent", StringHelper.InjectionString(this.ArticleContent), DbType.String);
                    if (!string.IsNullOrEmpty(this.ImagePath))
                        param.Add("@ImagePath", StringHelper.InjectionString(this.ImagePath), DbType.String);
                    if (!string.IsNullOrEmpty(this.ArticleUrl))
                        param.Add("@ArticleUrl", StringHelper.InjectionString(this.ArticleUrl), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaTitle))
                        param.Add("@MetaTitle", StringHelper.InjectionString(this.MetaTitle), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaDescription))
                        param.Add("@MetaDescription", StringHelper.InjectionString(this.MetaDescription), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaKeyword))
                        param.Add("@MetaKeyword", StringHelper.InjectionString(this.MetaKeyword), DbType.String);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@ReviewStatusId", this.ReviewStatusId, DbType.Byte);
                    param.Add("@ArticleId", this.ArticleId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Articles_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.ArticleId = param.Get<int>("ArticleId");
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
                    param.Add("@ArticleId", this.ArticleId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Articles_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Articles> Get()
        {
            Articles resultVar = new Articles();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Articles>, int> tuple;
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

        public async Task<Tuple<List<Articles>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Articles> articlesList = null;
            Tuple<List<Articles>, int> resultVar = Tuple.Create(articlesList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@ArticleId", this.ArticleId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Title))
                        param.Add("@Title", StringHelper.InjectionString(this.Title), DbType.String);
                    if (!string.IsNullOrEmpty(this.Summary))
                        param.Add("@Summary", StringHelper.InjectionString(this.Summary), DbType.String);
                    if (!string.IsNullOrEmpty(this.ArticleContent))
                        param.Add("@ArticleContent", StringHelper.InjectionString(this.ArticleContent), DbType.String);
                    if (!string.IsNullOrEmpty(this.ImagePath))
                        param.Add("@ImagePath", StringHelper.InjectionString(this.ImagePath), DbType.String);
                    if (!string.IsNullOrEmpty(this.ArticleUrl))
                        param.Add("@ArticleUrl", StringHelper.InjectionString(this.ArticleUrl), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaTitle))
                        param.Add("@MetaTitle", StringHelper.InjectionString(this.MetaTitle), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaDescription))
                        param.Add("@MetaDescription", StringHelper.InjectionString(this.MetaDescription), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaKeyword))
                        param.Add("@MetaKeyword", StringHelper.InjectionString(this.MetaKeyword), DbType.String);
                    param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@ReviewStatusId", this.ReviewStatusId, DbType.Byte);
                    if (!string.IsNullOrEmpty(this.CreatedBy))
                        param.Add("@CreatedBy", this.CreatedBy, DbType.String);
                    if (!string.IsNullOrEmpty(this.UpdatedBy))
                        param.Add("@UpdatedBy", this.UpdatedBy, DbType.String);
                    if (!string.IsNullOrEmpty(this.ReviewedBy))
                        param.Add("@ReviewedBy", StringHelper.InjectionString(this.ReviewedBy), DbType.String);
                    param.Add("@SearchByDateType", searchByDateType, DbType.Byte);
                    if (dateFrom != DateTime.MinValue)
                        param.Add("@DateFrom", dateFrom, DbType.DateTime);
                    if (dateTo != DateTime.MinValue)
                        param.Add("@DateTo", dateTo, DbType.DateTime);
                    param.Add("@OrderByClauseId", orderByClauseId, DbType.Int32);
                    param.Add("@PageSize", pageSize, DbType.Int32);
                    param.Add("@PageIndex", pageIndex, DbType.Int32);
                    param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    
                    articlesList = await connection
                        .QueryAsync<Articles>("Articles_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Articles>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(articlesList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<string, string>> UpdateManyReviewStatus(string articleId)
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
                    if (!string.IsNullOrEmpty(articleId))
                        param.Add("@ArticleId", articleId, DbType.String);
                    param.Add("@ReviewStatusId", this.ReviewStatusId, DbType.Byte);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Articles_UpdateManyReviewStatus", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Tuple<string, string>> DeleteMultiple(string articleId)
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
                    if (!string.IsNullOrEmpty(articleId))
                        param.Add("@ArticleId", articleId, DbType.String);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("Articles_DeleteMultiple", param, commandType: CommandType.StoredProcedure);

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

        #endregion

        #region Static Methods

        public static async Task<Articles> Static_GetById(string actBy, int articleId)
        {
            Articles articles = new Articles
            {
                ActBy = actBy,
                ArticleId = articleId
            };

            return await articles.Get();
        }

        public static async Task<Articles> Static_GetViewById(int articleId)
        {
            Articles articles = new Articles
            {
                ArticleId = articleId,
                ReviewStatusId = ConstantHelper.ReviewStatusIdApproved
            };

            return await articles.Get();
        }

        public static async Task<Articles> Static_GetViewByUrl(string articleUrl)
        {
            Articles articles = new Articles
            {
                ArticleUrl = articleUrl,
                ReviewStatusId = ConstantHelper.ReviewStatusIdApproved
            };

            return await articles.Get();
        }

        public static async Task<Tuple<string, string>> Static_UpdateManyReviewStatus(string actBy, string articleId, byte reviewStatusId)
        {
            Articles articles = new Articles
            {
                ActBy = actBy,
                ReviewStatusId = reviewStatusId
            };

            return await articles.UpdateManyReviewStatus(articleId);
        }

        public static async Task<Tuple<string, string>> Static_DeleteMultiple(string actBy, string articleId)
        {
            Articles articles = new Articles
            {
                ActBy = actBy
            };

            return await articles.DeleteMultiple(articleId);
        }

        #endregion
    }
}
