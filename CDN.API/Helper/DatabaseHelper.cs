using CDN.API.ObjectMember;
using System.Data;
using System.Data.SqlClient;

namespace CDN.API.Helper
{
    public class DatabaseHelper
    {
        public string DBConnString { get; set; }

        public DatabaseHelper(string strDBConnectionStr)
        {
            this.DBConnString = strDBConnectionStr;
        }

        public long ExecuteStoreProcedure(string strSPName, out DataSet dsResult, out string strExceptionMessage, DataTable dtblInput = null)
        {
            long lngResult = 0;
            dsResult = null;
            strExceptionMessage = null;

            try
            {
                if (string.IsNullOrWhiteSpace(strSPName))
                    throw new Exception("Store procedure name is null or empty");

                using (SqlConnection sConn = new SqlConnection(this.DBConnString))
                {
                    sConn.Open();

                    using (SqlCommand sCommand = new SqlCommand(strSPName, sConn))
                    {
                        sCommand.CommandType = CommandType.StoredProcedure;

                        if (dtblInput != null && dtblInput.Columns != null && dtblInput.Rows != null && dtblInput.Rows.Count > 0)
                        {
                            foreach (DataColumn dc in dtblInput.Columns) 
                                sCommand.Parameters.AddWithValue($"@{dc.ColumnName}", dtblInput.Rows[0][dc.ColumnName]);
                        }

                        dsResult = new DataSet();
                        SqlDataAdapter sAdapter = new SqlDataAdapter(sCommand);
                        sAdapter.Fill(dsResult);
                    }
                }
            }
            catch (SqlException sEx)
            {
                lngResult = sEx.ErrorCode;
                strExceptionMessage = sEx.Message;
            }
            catch (Exception ex)
            {
                lngResult = -99;
                strExceptionMessage = ex.Message;
            }

            return lngResult;
        }
    
        public long ExecuteStoreProcedureWithTVP(string strSPName, DataSet dsTVP, out DataSet dsResult, out string strExceptionMessage)
        {
            long lngResult = 0;

            dsResult = null;
            strExceptionMessage = string.Empty;

            try
            {
                #region Pre-checking
                if (string.IsNullOrWhiteSpace(strSPName))
                    throw new Exception("Store procedure name is null or empty");
                if (dsTVP == null)
                    throw new Exception("TVP dataset is null");
                if (dsTVP.Tables == null)
                    throw new Exception("TVP tables is null");
                if (dsTVP.Tables.Count == 0)
                    throw new Exception("There is no TVP tables exists");
                #endregion

                using (SqlConnection sConn = new SqlConnection(this.DBConnString))
                {
                    sConn.Open();

                    using (SqlCommand sCommand = new SqlCommand(strSPName, sConn))
                    {
                        sCommand.CommandType = CommandType.StoredProcedure;

                        foreach (DataTable dtblTVP in dsResult.Tables)
                        {
                            SqlParameter sParam = new SqlParameter()
                            {
                                ParameterName = $"@{dtblTVP.TableName}",
                                SqlDbType = SqlDbType.Structured,
                                Value = dtblTVP
                            };

                            sCommand.Parameters.Add(sParam);
                        }

                        SqlDataAdapter sAdapter = new SqlDataAdapter(sCommand);
                        sAdapter.Fill(dsResult);
                    }
                }
            }
            catch (SqlException sEx)
            {
                lngResult = sEx.ErrorCode;
                strExceptionMessage = sEx.Message;
            }
            catch (Exception ex)
            {
                lngResult = -99;
                strExceptionMessage = ex.Message;
            }

            return lngResult;
        }
    }
}
