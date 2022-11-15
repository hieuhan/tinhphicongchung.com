using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;
using Dapper;

namespace tinhphicongchung.com.library
{
    public class Contracts
    {
        #region Properties

        public double Amount { get; set; }
        public double AssetValue { get; set; }
        public double NotarizationFee { get; set; }
        public string ActionStatus { get; set; }
        public string ActionMessage { get; set; }
        
        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Contracts()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Contracts(string connection)
        {
            _connectionString = connection;
        }

        ~Contracts()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Hợp đồng vay tiền
        /// </summary>
        /// <returns></returns>
        public async Task<Contracts> LoanContract()
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@Amount", this.Amount, DbType.Double);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("LoanContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng sửa đổi, bổ sung có thay đổi giá trị tài sản
        /// </summary>
        /// <returns></returns>
        public async Task<Contracts> ContractAmendmentsAndSupplements()
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@Amount", this.Amount, DbType.Double);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractAmendmentsAndSupplements", param, commandType: CommandType.StoredProcedure);

                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng kinh tế, thương mại, đầu tư, kinh doanh
        /// </summary>
        /// <returns></returns>
        public async Task<Contracts> EconomicCommercialInvestmentBusinessContract()
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@Amount", this.Amount, DbType.Double);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("EconomicCommercialInvestmentBusinessContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng thuê Nhà
        /// </summary>
        /// <param name="rentalPeriod">Thời gian thuê nhà (tháng)</param>
        /// <returns></returns>
        public async Task<Contracts> HouseLeaseContract(int rentalPeriod)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@RentalPeriod", rentalPeriod, DbType.Int32);
                    param.Add("@Amount", this.Amount, DbType.Double);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("HouseLeaseContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng thế chấp
        /// </summary>
        /// <returns></returns>
        public async Task<Contracts> MortgageContract()
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@Amount", this.Amount, DbType.Double);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("MortgageContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng đặt cọc
        /// </summary>
        /// <returns></returns>
        public async Task<Contracts> DepositContract()
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@Amount", this.Amount, DbType.Double);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("DepositContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng tiền
        /// </summary>
        /// <returns></returns>
        public async Task<Contracts> CapitalContributionContract()
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@Amount", this.Amount, DbType.Double);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("CapitalContributionContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea">Diện tích nhà/m2</param>
        /// <param name="apartmentTypeId">Loại Nhà</param>
        /// <param name="year">Năm xây dựng</param>
        /// <param name="apartmentId">Cấp công trình</param>
        /// <returns></returns>
        public async Task<Contracts> ContractCapitalContributionByApartment(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@ApartmentTypeId", apartmentTypeId, DbType.Byte);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@ApartmentId", apartmentId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractCapitalContributionByApartment", param, commandType: CommandType.StoredProcedure);
                    
                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng mua bán căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="apartmentTypeId"></param>
        /// <param name="year"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<Contracts> PurchaseAndSaleApartmentContract(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@ApartmentTypeId", apartmentTypeId, DbType.Byte);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@ApartmentId", apartmentId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("PurchaseAndSaleApartmentContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng tặng cho căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="apartmentTypeId"></param>
        /// <param name="year"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<Contracts> ContractDonationApartment(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@ApartmentTypeId", apartmentTypeId, DbType.Byte);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@ApartmentId", apartmentId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractDonationApartment", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="apartmentTypeId"></param>
        /// <param name="year"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<Contracts> DocumentDivideInheritanceApartment(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@ApartmentTypeId", apartmentTypeId, DbType.Byte);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@ApartmentId", apartmentId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("DocumentDivideInheritanceApartment", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }


        /// <summary>
        /// Hợp đồng chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> LandUseRightTransferContract(double houseArea, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("LandUseRightTransferContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng mua bán Nhà và chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> ContractSaleHouseAndTransferLandUseRight(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LandId", landId, DbType.Int32);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractSaleHouseAndTransferLandUseRight", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng tặng cho Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> ContractDonationHouseLand(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LandId", landId, DbType.Int32);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractDonationHouseLand", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng tặng cho quyền sử dụng Đất
        /// </summary>
        /// <param name="landArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> ContractDonationLandUseRight(double landArea, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractDonationLandUseRight", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng chia tách Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> HouseLandSeparationContract(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LandId", landId, DbType.Int32);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("HouseLandSeparationContract", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng chia tách quyền sử dụng Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> ContractDivisionLandUseRight(double landArea, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractDivisionLandUseRight", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> ContractCapitalContributionByHouseLand(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LandId", landId, DbType.Int32);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractCapitalContributionByHouseLand", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng quyền sử dụng Đất
        /// </summary>
        /// <param name="landArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> ContractCapitalContributionByLandUseRight(double landArea, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("ContractCapitalContributionByLandUseRight", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là quyền sử dụng Đất
        /// </summary>
        /// <param name="landArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> DocumentDivisionInheritanceLandUseRight(double landArea, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("DocumentDivisionInheritanceLandUseRight", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<Contracts> DocumentDivisionInheritanceHouseLand(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts resultVar = new Contracts();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@HouseArea", houseArea, DbType.Double);
                    param.Add("@LandArea", landArea, DbType.Double);
                    param.Add("@LandId", landId, DbType.Int32);
                    param.Add("@Year", year, DbType.Int32);
                    param.Add("@LocationId", locationId, DbType.Int32);
                    param.Add("@AssetValue", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@NotarizationFee", dbType: DbType.Double, direction: ParameterDirection.Output);
                    param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                    await connection.ExecuteAsync("DocumentDivisionInheritanceHouseLand", param, commandType: CommandType.StoredProcedure);

                    resultVar.AssetValue = param.Get<double>("AssetValue");
                    resultVar.NotarizationFee = param.Get<double>("NotarizationFee");
                    resultVar.ActionStatus = param.Get<string>("ActionStatus");
                    resultVar.ActionMessage = param.Get<string>("ActionMessage");
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

        /// <summary>
        /// Hợp đồng vay tiền
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_LoanContract(double amount)
        {
            Contracts contracts = new Contracts
            {
                Amount = amount
            };

            return await contracts.LoanContract();
        }

        /// <summary>
        /// Hợp đồng sửa đổi, bổ sung có thay đổi giá trị tài sản
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractAmendmentsAndSupplements(double amount)
        {
            Contracts contracts = new Contracts
            {
                Amount = amount
            };

            return await contracts.ContractAmendmentsAndSupplements();
        }

        /// <summary>
        /// Hợp đồng kinh tế, thương mại, đầu tư, kinh doanh
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_EconomicCommercialInvestmentBusinessContract(double amount)
        {
            Contracts contracts = new Contracts
            {
                Amount = amount
            };

            return await contracts.EconomicCommercialInvestmentBusinessContract();
        }

        /// <summary>
        /// Hợp đồng thuê Nhà
        /// </summary>
        /// <param name="rentalPeriod">Thời gian thuê (tháng)</param>
        /// <param name="amount">Giá thuê/tháng</param>
        /// <returns></returns>
        public static async Task<Contracts> Static_HouseLeaseContract(int rentalPeriod, double amount)
        {
            Contracts contracts = new Contracts
            {
                Amount = amount
            };

            return await contracts.HouseLeaseContract(rentalPeriod);
        }

        /// <summary>
        /// Hợp đồng thế chấp
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_MortgageContract(double amount)
        {
            Contracts contracts = new Contracts
            {
                Amount = amount
            };

            return await contracts.MortgageContract();
        }

        /// <summary>
        /// Hợp đồng đặt cọc
        /// </summary>
        /// <param name="amount">Số tiền đặt cọc</param>
        /// <returns></returns>
        public static async Task<Contracts> Static_DepositContract(double amount)
        {
            Contracts contracts = new Contracts
            {
                Amount = amount
            };

            return await contracts.DepositContract();
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng tiền
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_CapitalContributionContract(double amount)
        {
            Contracts contracts = new Contracts
            {
                Amount = amount
            };

            return await contracts.CapitalContributionContract();
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea">Diện tích nhà/m2</param>
        /// <param name="apartmentTypeId">Loại Nhà</param>
        /// <param name="year">Năm xây dựng</param>
        /// <param name="apartmentId">Cấp công trình</param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractCapitalContributionByApartment(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractCapitalContributionByApartment(houseArea, apartmentTypeId, year, apartmentId);
        }

        /// <summary>
        /// Hợp đồng mua bán căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="apartmentTypeId"></param>
        /// <param name="year"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_PurchaseAndSaleApartmentContract(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts contracts = new Contracts();

            return await contracts.PurchaseAndSaleApartmentContract(houseArea, apartmentTypeId, year, apartmentId);
        }

        /// <summary>
        /// Hợp đồng tặng cho căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="apartmentTypeId"></param>
        /// <param name="year"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractDonationApartment(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractDonationApartment(houseArea, apartmentTypeId, year, apartmentId);
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là căn hộ Chung Cư
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="apartmentTypeId"></param>
        /// <param name="year"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_DocumentDivideInheritanceApartment(double houseArea, byte apartmentTypeId, int year, int apartmentId)
        {
            Contracts contracts = new Contracts();

            return await contracts.DocumentDivideInheritanceApartment(houseArea, apartmentTypeId, year, apartmentId);
        }

        /// <summary>
        /// Hợp đồng chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_LandUseRightTransferContract(double houseArea, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.LandUseRightTransferContract(houseArea, locationId);
        }

        /// <summary>
        /// Hợp đồng mua bán Nhà và chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractSaleHouseAndTransferLandUseRight(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractSaleHouseAndTransferLandUseRight(houseArea, landArea, landId, year, locationId);
        }

        /// <summary>
        /// Hợp đồng tặng cho Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractDonationHouseLand(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractDonationHouseLand(houseArea, landArea, landId, year, locationId);
        }

        /// <summary>
        /// Hợp đồng tặng cho quyền sử dụng Đất
        /// </summary>
        /// <param name="landArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractDonationLandUseRight(double landArea, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractDonationLandUseRight(landArea, locationId);
        }

        /// <summary>
        /// Hợp đồng chia tách Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_HouseLandSeparationContract(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.HouseLandSeparationContract(houseArea, landArea, landId, year, locationId);
        }

        /// <summary>
        /// Hợp đồng chia tách quyền sử dụng Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractDivisionLandUseRight(double landArea, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractDivisionLandUseRight(landArea, locationId);
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractCapitalContributionByHouseLand(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractCapitalContributionByHouseLand(houseArea, landArea, landId, year, locationId);
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng quyền sử dụng Đất
        /// </summary>
        /// <param name="landArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_ContractCapitalContributionByLandUseRight(double landArea, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.ContractCapitalContributionByLandUseRight(landArea, locationId);
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là quyền sử dụng Đất
        /// </summary>
        /// <param name="landArea"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_DocumentDivisionInheritanceLandUseRight(double landArea, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.DocumentDivisionInheritanceLandUseRight(landArea, locationId);
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là Nhà - Đất
        /// </summary>
        /// <param name="houseArea"></param>
        /// <param name="landArea"></param>
        /// <param name="landId"></param>
        /// <param name="year"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static async Task<Contracts> Static_DocumentDivisionInheritanceHouseLand(double houseArea, double landArea, int landId, int year, int locationId)
        {
            Contracts contracts = new Contracts();

            return await contracts.DocumentDivisionInheritanceHouseLand(houseArea, landArea, landId, year, locationId);
        }

        #endregion
    }
}
