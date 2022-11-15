using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;

namespace tinhphicongchung.com.library
{
    public class Pages
    {
        #region Properties

        public string ActBy { get; set; }
        public int PageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Hotline1 { get; set; }
        public string Hotline2 { get; set; }
        public string Hotline3 { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string CanonicalTag { get; set; }
        public string SeoFooter { get; set; }
        public string H1Tag { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Youtube { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Pages()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Pages(string connection)
        {
            _connectionString = connection;
        }

        ~Pages()
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
                    if (!string.IsNullOrEmpty(this.Email))
                        param.Add("@Email", StringHelper.InjectionString(this.Email), DbType.String);
                    if (!string.IsNullOrEmpty(this.Tel))
                        param.Add("@Tel", StringHelper.InjectionString(this.Tel), DbType.String);
                    if (!string.IsNullOrEmpty(this.Fax))
                        param.Add("@Fax", StringHelper.InjectionString(this.Fax), DbType.String);
                    if (!string.IsNullOrEmpty(this.Hotline1))
                        param.Add("@Hotline1", StringHelper.InjectionString(this.Hotline1), DbType.String);
                    if (!string.IsNullOrEmpty(this.Hotline2))
                        param.Add("@Hotline2", StringHelper.InjectionString(this.Hotline2), DbType.String);
                    if (!string.IsNullOrEmpty(this.Hotline3))
                        param.Add("@Hotline3", StringHelper.InjectionString(this.Hotline3), DbType.String);
                    if (!string.IsNullOrEmpty(this.Address))
                        param.Add("@Address", StringHelper.InjectionString(this.Address), DbType.String);
                    if (!string.IsNullOrEmpty(this.ImagePath))
                        param.Add("@ImagePath", StringHelper.InjectionString(this.ImagePath), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaTitle))
                        param.Add("@MetaTitle", StringHelper.InjectionString(this.MetaTitle), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaDescription))
                        param.Add("@MetaDescription", StringHelper.InjectionString(this.MetaDescription), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaKeyword))
                        param.Add("@MetaKeyword", StringHelper.InjectionString(this.MetaKeyword), DbType.String);
                    if (!string.IsNullOrEmpty(this.CanonicalTag))
                        param.Add("@CanonicalTag", StringHelper.InjectionString(this.CanonicalTag), DbType.String);
                    if (!string.IsNullOrEmpty(this.SeoFooter))
                        param.Add("@SeoFooter", StringHelper.InjectionString(this.SeoFooter), DbType.String);
                    if (!string.IsNullOrEmpty(this.H1Tag))
                        param.Add("@H1Tag", StringHelper.InjectionString(this.H1Tag), DbType.String);
                    if (!string.IsNullOrEmpty(this.Facebook))
                        param.Add("@Facebook", StringHelper.InjectionString(this.Facebook), DbType.String);
                    if (!string.IsNullOrEmpty(this.Twitter))
                        param.Add("@Twitter", StringHelper.InjectionString(this.Twitter), DbType.String);
                    if (!string.IsNullOrEmpty(this.LinkedIn))
                        param.Add("@LinkedIn", StringHelper.InjectionString(this.LinkedIn), DbType.String);
                    if (!string.IsNullOrEmpty(this.Youtube))
                        param.Add("@Youtube", StringHelper.InjectionString(this.Youtube), DbType.String);
                    param.Add("@PageId", this.PageId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Pages_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.PageId = param.Get<int>("PageId");
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

        public async Task<Pages> Get()
        {
            Pages resultVar = new Pages();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Pages>, int> tuple;
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

        public async Task<Tuple<List<Pages>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Pages> pagesList = null;
            Tuple<List<Pages>, int> resultVar = Tuple.Create(pagesList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@PageId", this.PageId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    if (!string.IsNullOrEmpty(this.Email))
                        param.Add("@Email", StringHelper.InjectionString(this.Email), DbType.String);
                    if (!string.IsNullOrEmpty(this.Tel))
                        param.Add("@Tel", StringHelper.InjectionString(this.Tel), DbType.String);
                    if (!string.IsNullOrEmpty(this.Fax))
                        param.Add("@Fax", StringHelper.InjectionString(this.Fax), DbType.String);
                    if (!string.IsNullOrEmpty(this.Hotline1))
                        param.Add("@Hotline1", StringHelper.InjectionString(this.Hotline1), DbType.String);
                    if (!string.IsNullOrEmpty(this.Hotline2))
                        param.Add("@Hotline2", StringHelper.InjectionString(this.Hotline2), DbType.String);
                    if (!string.IsNullOrEmpty(this.Hotline3))
                        param.Add("@Hotline3", StringHelper.InjectionString(this.Hotline3), DbType.String);
                    if (!string.IsNullOrEmpty(this.Address))
                        param.Add("@Address", StringHelper.InjectionString(this.Address), DbType.String);
                    if (!string.IsNullOrEmpty(this.ImagePath))
                        param.Add("@ImagePath", StringHelper.InjectionString(this.ImagePath), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaTitle))
                        param.Add("@MetaTitle", StringHelper.InjectionString(this.MetaTitle), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaDescription))
                        param.Add("@MetaDescription", StringHelper.InjectionString(this.MetaDescription), DbType.String);
                    if (!string.IsNullOrEmpty(this.MetaKeyword))
                        param.Add("@MetaKeyword", StringHelper.InjectionString(this.MetaKeyword), DbType.String);
                    if (!string.IsNullOrEmpty(this.CanonicalTag))
                        param.Add("@CanonicalTag", StringHelper.InjectionString(this.CanonicalTag), DbType.String);
                    if (!string.IsNullOrEmpty(this.SeoFooter))
                        param.Add("@SeoFooter", StringHelper.InjectionString(this.SeoFooter), DbType.String);
                    if (!string.IsNullOrEmpty(this.H1Tag))
                        param.Add("@H1Tag", StringHelper.InjectionString(this.H1Tag), DbType.String);
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
                    
                    pagesList = await connection
                        .QueryAsync<Pages>("Pages_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Pages>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(pagesList, rowCount);
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

        public static async Task<Pages> Static_GetById(string actBy, int pageId)
        {
            Pages pages = new Pages
            {
                ActBy = actBy,
                PageId = pageId
            };

            return await pages.Get();
        }

        #endregion
    }
}
