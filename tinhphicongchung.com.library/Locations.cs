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
    public class Locations
    {
        #region Properties

        public string ActBy { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LocationTypeId { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public int StreetId { get; set; }
        public string StreetName { get; set; }
        public string StreetDescription { get; set; }
        public byte LandTypeId { get; set; }
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

        public Locations()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Locations(string connection)
        {
            _connectionString = connection;
        }

        ~Locations()
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
                    param.Add("@LocationTypeId", this.LocationTypeId, DbType.Int32);
                    param.Add("@ProvinceId", this.ProvinceId, DbType.Int32);
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32);
                    param.Add("@WardId", this.WardId, DbType.Int32);
                    param.Add("@StreetId", this.StreetId, DbType.Int32);
                    param.Add("@LandTypeId", this.LandTypeId, DbType.Byte);
                    param.Add("@Price", this.Price, DbType.Decimal);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@LocationId", this.LocationId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Locations_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.LocationId = param.Get<int>("LocationId");
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
                    param.Add("@LocationId", this.LocationId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("Locations_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<Locations> Get(byte isJoin = 0)
        {
            Locations resultVar = new Locations();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Locations>, int> tuple;
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

        public async Task<Tuple<List<Locations>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, byte isJoin, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Locations> locationsList = null;
            Tuple<List<Locations>, int> resultVar = Tuple.Create(locationsList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@LocationId", this.LocationId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@LocationTypeId", this.LocationTypeId, DbType.Int32);
                    param.Add("@ProvinceId", this.ProvinceId, DbType.Int32);
                    param.Add("@DistrictId", this.DistrictId, DbType.Int32);
                    param.Add("@WardId", this.WardId, DbType.Int32);
                    param.Add("@StreetId", this.StreetId, DbType.Int32);
                    param.Add("@LandTypeId", this.LandTypeId, DbType.Byte);
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
                    param.Add("@IsJoin", isJoin, DbType.Byte);
                    param.Add("@PageSize", pageSize, DbType.Int32);
                    param.Add("@PageIndex", pageIndex, DbType.Int32);
                    param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    
                    locationsList = await connection
                        .QueryAsync<Locations>("Locations_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<Locations>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(locationsList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<Locations>> GetList()
        {
            List<Locations> resultVar = new List<Locations>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    resultVar = await connection.QueryAsync<Locations>("SELECT * FROM [dbo].[Locations] ORDER BY [DisplayOrder],[LocationId] DESC") as List<Locations>;
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

        public static async Task<List<Locations>> Static_GetList()
        {
            Locations locations = new Locations();
            return await locations.GetList();
        }

        public static Locations Static_Get(byte locationId, List<Locations> list)
        {
            Locations resultVar = new Locations();
            try
            {
                if (locationId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.LocationId == locationId) ?? new Locations();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<Locations> Static_GetById(string actBy, int locationId, byte isJoin = 0)
        {
            Locations locations = new Locations
            {
                ActBy = actBy,
                LocationId = locationId
            };
            return await locations.Get(isJoin);
        }

        #endregion
    }
}
