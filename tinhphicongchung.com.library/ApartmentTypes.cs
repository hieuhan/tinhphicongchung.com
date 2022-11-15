using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;
using Dapper;

namespace tinhphicongchung.com.library
{
    public class ApartmentTypes
    {
        #region Properties

        public string ActBy { get; set; }
        public byte ApartmentTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public ApartmentTypes()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public ApartmentTypes(string connection)
        {
            _connectionString = connection;
        }

        ~ApartmentTypes()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public async Task<List<ApartmentTypes>> GetList()
        {
            List<ApartmentTypes> resultVar = new List<ApartmentTypes>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    resultVar = await connection.QueryAsync<ApartmentTypes>("SELECT * FROM [dbo].[ApartmentTypes] ORDER BY [DisplayOrder],[ApartmentTypeId]") as List<ApartmentTypes>;
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

        public static async Task<List<ApartmentTypes>> Static_GetList()
        {
            ApartmentTypes apartmentTypes = new ApartmentTypes();
            return await apartmentTypes.GetList();
        }

        public static ApartmentTypes Static_Get(byte ApartmentTypeId, List<ApartmentTypes> list)
        {
            ApartmentTypes resultVar = new ApartmentTypes();
            try
            {
                if (ApartmentTypeId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.ApartmentTypeId == ApartmentTypeId) ?? new ApartmentTypes();
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
