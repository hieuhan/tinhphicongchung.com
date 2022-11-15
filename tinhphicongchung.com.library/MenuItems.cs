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
    public class MenuItems
    {
        #region Properties

        public string ActBy { get; set; }
        public int MenuItemId { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Target { get; set; }
        public int ParentId { get; set; }
        public string IconPath { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public string TreeOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public MenuItems()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public MenuItems(string connection)
        {
            _connectionString = connection;
        }

        ~MenuItems()
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
                    param.Add("@MenuId", this.MenuId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    if (!string.IsNullOrEmpty(this.Path))
                        param.Add("@Path", StringHelper.InjectionString(this.Path), DbType.String);
                    if (!string.IsNullOrEmpty(this.Target))
                        param.Add("@Target", StringHelper.InjectionString(this.Target), DbType.String);
                    param.Add("@ParentId", this.ParentId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.IconPath))
                        param.Add("@IconPath", StringHelper.InjectionString(this.IconPath), DbType.String);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@MenuItemId", this.MenuItemId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("MenuItems_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.MenuItemId = param.Get<int>("MenuItemId");
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
                    param.Add("@MenuItemId", this.MenuItemId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("MenuItems_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<MenuItems> Get()
        {
            MenuItems resultVar = new MenuItems();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<MenuItems>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, orderByClauseId, pageIndex, pageSize);
                if (tuple.Item1 != null && tuple.Item1.Count > 0) resultVar = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<Tuple<List<MenuItems>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<MenuItems> menuItemsList = null;
            Tuple<List<MenuItems>, int> resultVar = Tuple.Create(menuItemsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@MenuItemId", this.MenuItemId, DbType.Int32);
                    param.Add("@MenuId", this.MenuId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    if (!string.IsNullOrEmpty(this.Path))
                        param.Add("@Path", StringHelper.InjectionString(this.Path), DbType.String);
                    if (!string.IsNullOrEmpty(this.Target))
                        param.Add("@Target", StringHelper.InjectionString(this.Target), DbType.String);
                    param.Add("@ParentId", this.ParentId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.IconPath))
                        param.Add("@IconPath", StringHelper.InjectionString(this.IconPath), DbType.String);
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
                    
                    menuItemsList = await connection
                        .QueryAsync<MenuItems>("MenuItems_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<MenuItems>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(menuItemsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<MenuItems>> GetList(byte statusId = 0)
        {
            List<MenuItems> resultVar = new List<MenuItems>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = "SELECT * FROM [dbo].[MenuItems] ORDER BY [TreeOrder],[DisplayOrder]";

                    if(statusId > 0)
                    {
                        query = string.Format("SELECT * FROM [dbo].[MenuItems] WHERE [StatusId] = {0} ORDER BY [TreeOrder],[DisplayOrder]", statusId);
                    }

                    resultVar = await connection.QueryAsync<MenuItems>(query) as List<MenuItems>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<MenuItems>> GetParentList(int menuId, byte statusId = 0)
        {
            List<MenuItems> resultVar = new List<MenuItems>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string query = String.Format("SELECT * FROM [dbo].[MenuItems] WHERE ([MenuId]={0}) AND ((ISNULL([ParentId], 0) = 0) OR ([MenuItemId] IN (SELECT [ParentId] FROM [dbo].[MenuItems] WHERE ISNULL([ParentId], 0) <> 0)))  ORDER BY [TreeOrder],[DisplayOrder]", menuId);

                    if (statusId > 0)
                    {
                        query = string.Format("SELECT * FROM [dbo].[MenuItems] WHERE ([MenuId]={0}) AND ((ISNULL([ParentId], 0) = 0) OR ([MenuItemId] IN (SELECT [ParentId] FROM [dbo].[MenuItems] WHERE ISNULL([ParentId], 0) <> 0))) AND ([StatusId] = {1}) ORDER BY [TreeOrder],[DisplayOrder]", menuId, statusId);
                    }

                    resultVar = await connection.QueryAsync<MenuItems>(query) as List<MenuItems>;
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

        public static async Task<List<MenuItems>> Static_GetList(byte statusId = 0)
        {
            MenuItems menuItems = new MenuItems();
            return await menuItems.GetList(statusId);
        }

        public static async Task<List<MenuItems>> Static_GetParentList(int menuId, byte statusId = 0)
        {
            MenuItems menuItems = new MenuItems();
            return await menuItems.GetParentList(menuId, statusId);
        }

        public static MenuItems Static_Get(byte menuItemId, List<MenuItems> list)
        {
            MenuItems resultVar = new MenuItems();
            try
            {
                if (menuItemId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.MenuItemId == menuItemId) ?? new MenuItems();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<MenuItems> Static_GetById(string actBy, int menuItemId)
        {
            MenuItems menuItems = new MenuItems
            {
                ActBy = actBy,
                MenuItemId = menuItemId
            };

            return await menuItems.Get();
        }

        #endregion
    }
}
