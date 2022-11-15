using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using tinhphicongchung.com.helper;
using System.Collections.Generic;

namespace tinhphicongchung.com.library
{
    public class HitCounter
    {
        #region Properties

        public string ActBy { get; set; }
        public int HitCounterId { get; set; }
        public long Value { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public HitCounter()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public HitCounter(string connection)
        {
            _connectionString = connection;
        }

        ~HitCounter()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public void Update()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrEmpty(this.ActBy))
                        param.Add("@ActBy", this.ActBy, DbType.String);
                    param.Add("@HitCounterId", this.HitCounterId, DbType.Int32);

                    connection.Execute("HitCounter_Update_Quick", param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HitCounter> GetAsync()
        {
            HitCounter resultVar = new HitCounter();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    List<HitCounter> hitCounterList = await connection.QueryAsync<HitCounter>("SELECT TOP 1 * FROM [dbo].[HitCounter] ORDER BY [HitCounterId]") as List<HitCounter>;

                    if (hitCounterList.IsAny())
                    {
                        resultVar = hitCounterList[0];
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

        public HitCounter Get()
        {
            HitCounter resultVar = new HitCounter();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    List<HitCounter> hitCounterList = connection.Query<HitCounter>("SELECT TOP 1 * FROM [dbo].[HitCounter] ORDER BY [HitCounterId]") as List<HitCounter>;

                    if (hitCounterList.IsAny())
                    {
                        resultVar = hitCounterList[0];
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

        #endregion

        #region Static Methods

        public static void Static_Update()
        {
            HitCounter hitCounter = new HitCounter();
            hitCounter.Update();
        }

        public static async Task<HitCounter> Static_GetAsync()
        {
            HitCounter hitCounter = new HitCounter();
            return await hitCounter.GetAsync();
        }
        
        public static HitCounter Static_Get()
        {
            HitCounter hitCounter = new HitCounter();
            return hitCounter.Get();
        }

        #endregion
    }
}
