using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;
using Dapper;

namespace tinhphicongchung.com.library
{
    public class LandTypes
    {
        #region Properties

        public string ActBy { get; set; }
        public byte LandTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public LandTypes()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public LandTypes(string connection)
        {
            _connectionString = connection;
        }

        ~LandTypes()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public async Task<List<LandTypes>> GetList()
        {
            List<LandTypes> resultVar = new List<LandTypes>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    resultVar = await connection.QueryAsync<LandTypes>("SELECT * FROM [dbo].[LandTypes] ORDER BY [DisplayOrder],[LandTypeId]") as List<LandTypes>;
                }
            }
            catch (Exception ex)
            {
                resultVar = null;
                throw ex;
            }

            return resultVar;
        }

        public async Task<List<LandTypes>> GetListDisplay()
        {
            List<LandTypes> resultVar = new List<LandTypes>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    resultVar = await connection.QueryAsync<LandTypes>(string.Format("SELECT * FROM [dbo].[LandTypes] WHERE [LandTypeId] IN (SELECT [LandTypeId] FROM [dbo].[Locations] WHERE [StatusId] = {0}) ORDER BY [DisplayOrder],[Name]", ConstantHelper.StatusIdActivated)) as List<LandTypes>;
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

        public static async Task<List<LandTypes>> Static_GetList()
        {
            LandTypes landTypes = new LandTypes();
            return await landTypes.GetList();
        }

        public static async Task<List<LandTypes>> Static_GetListDisplay()
        {
            LandTypes landTypes = new LandTypes();
            return await landTypes.GetListDisplay();
        }

        public static LandTypes Static_Get(byte landTypeId, List<LandTypes> list)
        {
            LandTypes resultVar = new LandTypes();
            try
            {
                if (landTypeId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.LandTypeId == landTypeId) ?? new LandTypes();
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
    }
}
