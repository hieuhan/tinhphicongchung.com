using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;
using System.Security.Cryptography;

namespace tinhphicongchung.com.library
{
    public class VehicleDepreciation
    {
        #region Properties

        public string ActBy { get; set; }
        public int VehicleDepreciationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public double Value { get; set; }
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

        public VehicleDepreciation()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public VehicleDepreciation(string connection)
        {
            _connectionString = connection;
        }

        ~VehicleDepreciation()
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
                    param.Add("@Price", this.Price, DbType.Decimal);
                    param.Add("@StatusId", this.StatusId, DbType.Byte);
                    if (this.DisplayOrder.HasValue)
                        param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                    param.Add("@VehicleDepreciationId", this.VehicleDepreciationId, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("VehicleDepreciation_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                    
                    this.VehicleDepreciationId = param.Get<int>("VehicleDepreciationId");
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
                    param.Add("@VehicleDepreciationId", this.VehicleDepreciationId, DbType.Int32);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                    
                    await connection.ExecuteAsync("VehicleDepreciation_Delete", param, commandType: CommandType.StoredProcedure);
                    
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

        public async Task<VehicleDepreciation> Get()
        {
            VehicleDepreciation resultVar = new VehicleDepreciation();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<VehicleDepreciation>, int> tuple;
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

        public async Task<Tuple<List<VehicleDepreciation>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<VehicleDepreciation> vehicleDepreciationList = null;
            Tuple<List<VehicleDepreciation>, int> resultVar = Tuple.Create(vehicleDepreciationList, rowCount);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@VehicleDepreciationId", this.VehicleDepreciationId, DbType.Int32);
                    if (!string.IsNullOrEmpty(this.Name))
                        param.Add("@Name", StringHelper.InjectionString(this.Name), DbType.String);
                    if (!string.IsNullOrEmpty(this.Description))
                        param.Add("@Description", StringHelper.InjectionString(this.Description), DbType.String);
                    param.Add("@MinValue", this.MinValue, DbType.Int32);
                    param.Add("@MaxValue", this.MaxValue, DbType.Int32);
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
                    param.Add("@PageSize", pageSize, DbType.Int32);
                    param.Add("@PageIndex", pageIndex, DbType.Int32);
                    param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    
                    vehicleDepreciationList = await connection
                        .QueryAsync<VehicleDepreciation>("VehicleDepreciation_GetPage", param, commandType: CommandType.StoredProcedure)
                        as List<VehicleDepreciation>;

                    rowCount = param.Get<int>("RowCount");

                    resultVar = Tuple.Create(vehicleDepreciationList, rowCount);
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<VehicleDepreciation>> GetList()
        {
            List<VehicleDepreciation> resultVar = new List<VehicleDepreciation>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    resultVar = await connection.QueryAsync<VehicleDepreciation>("SELECT * FROM [dbo].[VehicleDepreciation] ORDER BY [DisplayOrder],[Name]") as List<VehicleDepreciation>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            };
            
            return resultVar;
        }

        public async Task<VehicleDepreciation> GetBy()
        {
            VehicleDepreciation resultVar = new VehicleDepreciation();

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

                    List<VehicleDepreciation> vehicleDepreciationList = await connection
                        .QueryAsync<VehicleDepreciation>("VehicleDepreciation_GetBy", param, commandType: CommandType.StoredProcedure) as List<VehicleDepreciation>;

                    if (vehicleDepreciationList != null && vehicleDepreciationList.Count > 0)
                    {
                        resultVar = vehicleDepreciationList[0];
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

        public async Task<Tuple<string, string, double>> VehicleSalesContract(double amount, DateTime firstVehicleRegistrationDate)
        {
            double notarizationFee = 0;
            string actionStatus = string.Empty, actionMessage = string.Empty;
            Tuple<string, string, double> resultVar = Tuple.Create(actionStatus, actionMessage, notarizationFee);

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@Amount", amount, DbType.Double);
                    param.Add("@FirstVehicleRegistrationDate", firstVehicleRegistrationDate, DbType.Date);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("VehicleSalesContract", param, commandType: CommandType.StoredProcedure);

                    notarizationFee = param.Get<double>("NotarizationFee");
                    actionStatus = param.Get<string>("ActionStatus");
                    actionMessage = param.Get<string>("ActionMessage");

                    resultVar = Tuple.Create(actionStatus, actionMessage, notarizationFee);
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

        public static async Task<List<VehicleDepreciation>> Static_GetList()
        {
            VehicleDepreciation vehicleDepreciation = new VehicleDepreciation();
            return await vehicleDepreciation.GetList();
        }

        public static async Task<VehicleDepreciation> Static_GetBy(string actBy, double value)
        {
            VehicleDepreciation vehicleDepreciation = new VehicleDepreciation
            {
                ActBy = actBy,
                Value = value
            };
            return await vehicleDepreciation.GetBy();
        }

        public static async Task<Tuple<string, string, double>> Static_VehicleSalesContract(double amount, DateTime firstVehicleRegistrationDate)
        {
            VehicleDepreciation vehicleDepreciation = new VehicleDepreciation();

            return await vehicleDepreciation.VehicleSalesContract(amount, firstVehicleRegistrationDate);
        }

        public static VehicleDepreciation Static_Get(byte vehicleDepreciationId, List<VehicleDepreciation> list)
        {
            VehicleDepreciation resultVar = new VehicleDepreciation();
            try
            {
                if (vehicleDepreciationId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.VehicleDepreciationId == vehicleDepreciationId) ?? new VehicleDepreciation();
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }
            
            return resultVar;
        }

        public static async Task<VehicleDepreciation> Static_GetById(string actBy, int vehicleDepreciationId)
        {
            VehicleDepreciation vehicleDepreciation = new VehicleDepreciation
            {
                ActBy = actBy,
                VehicleDepreciationId = vehicleDepreciationId
            };
            return await vehicleDepreciation.Get();
        }

        #endregion
    }
}
