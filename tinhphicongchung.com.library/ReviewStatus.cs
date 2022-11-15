using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using tinhphicongchung.com.helper;
using System.Threading.Tasks;

namespace tinhphicongchung.com.library
{
    public class ReviewStatus
    {
        #region Properties

        public string ActBy { get; set; }
        public byte ReviewStatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public ReviewStatus()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public ReviewStatus(string connection)
        {
            _connectionString = connection;
        }

        ~ReviewStatus()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public async Task<List<ReviewStatus>> GetList()
        {
            List<ReviewStatus> resultVar = new List<ReviewStatus>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    resultVar = await connection.QueryAsync<ReviewStatus>("SELECT * FROM [dbo].[ReviewStatus] ORDER BY [DisplayOrder]") as List<ReviewStatus>;
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

        public static async Task<List<ReviewStatus>> Static_GetList()
        {
            ReviewStatus reviewStatus = new ReviewStatus();
            return await reviewStatus.GetList();
        }

        public static ReviewStatus Static_Get(byte reviewStatusId, List<ReviewStatus> list)
        {
            ReviewStatus resultVar = new ReviewStatus();
            try
            {
                if (reviewStatusId > 0 && list != null && list.Count > 0)
                {
                    resultVar = list.FirstOrDefault(i => i.ReviewStatusId == reviewStatusId) ?? new ReviewStatus();
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
