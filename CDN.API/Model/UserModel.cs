using CDN.API.Helper;
using System.Data;
using CDN.API.ObjectMember;
using System.Data.SqlClient;

namespace CDN.API.Model
{
    public class UserModel
    {
        public bool GetUser(int iPageSize, int iPageNo, out UserData uData, out string strExceptionMessage)
        {
            bool blnIsSuccess = true;
            DataSet dsResult = null;

            uData = null;
            strExceptionMessage = null;

            try
            {
                #region Input Table
                DataTable dtblInput = new DataTable();
                dtblInput.Columns.Add("PageNumber", typeof(int));
                dtblInput.Columns.Add("PageSize", typeof(int));

                DataRow drInput = dtblInput.NewRow();
                drInput["PageNumber"] = iPageNo;
                drInput["PageSize"] = iPageSize;

                dtblInput.Rows.Add(drInput);
                #endregion

                DatabaseHelper dbHelper = new DatabaseHelper("Data Source=DESKTOP-R066Q43;User ID=sa;Password=@dmon@1;Initial Catalog=CDN;Connect Timeout=30;Encrypt=False;");
                long lngResult = dbHelper.ExecuteStoreProcedure("UspUserSel", out dsResult, out strExceptionMessage, dtblInput);

                if (lngResult != 0)
                    throw new Exception($"Failed to extract user data. Status = {lngResult}. Message = {strExceptionMessage}");

                uData = new UserData();
                uData.PageSize = iPageSize;
                uData.PageNo = iPageNo;
                
                if (dsResult != null && dsResult.Tables != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    uData.TotalRecord = (int)dsResult.Tables[0].Rows[0]["TotalRecordCount"];

                    if (dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows != null && dsResult.Tables[1].Rows.Count > 0)
                    {
                        uData.UserDataList = new List<User>();

                        foreach (DataRow drResult in dsResult.Tables[1].Rows)
                        {
                            User uSr = new User();
                            uSr.Id = drResult["Id"].ToString();
                            uSr.Username = drResult["Username"].ToString();
                            uSr.Mail = drResult["Mail"].ToString();
                            uSr.PhoneNumber = drResult["PhoneNumber"].ToString();
                            uSr.Skillsets = drResult["Skillsets"].ToString();
                            uSr.Hobby = drResult["Hobby"].ToString();

                            uData.UserDataList.Add(uSr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                blnIsSuccess = false;
                strExceptionMessage = ex.Message;
            }

            return blnIsSuccess;
        }
    
        public bool ManipulateUserData(User userData, MaintainAction mAction, out string strExceptionMessage)
        {
            bool blnIsSuccess = true;
            DataSet dsResult = null;

            strExceptionMessage = null;

            try
            {
                if (userData == null)
                    throw new Exception("User data is null");

                DataTable dtblInput = new DataTable();
                dtblInput.Columns.Add("Id", typeof(string));
                dtblInput.Columns.Add("Username", typeof(string));
                dtblInput.Columns.Add("Mail", typeof(string));
                dtblInput.Columns.Add("PhoneNumber", typeof(string));
                dtblInput.Columns.Add("Skillsets", typeof(string));
                dtblInput.Columns.Add("Hobby", typeof(string));
                dtblInput.Columns.Add("Action", typeof(int));

                DataRow drInput = dtblInput.NewRow();
                drInput["Id"] = !string.IsNullOrWhiteSpace(userData.Id) ? userData.Id : System.Guid.NewGuid().ToString();
                drInput["Username"] = userData.Username;
                drInput["Mail"] = userData.Mail;
                drInput["PhoneNumber"] = userData.PhoneNumber;
                drInput["Skillsets"] = userData.Skillsets;
                drInput["Hobby"] = userData.Hobby;
                drInput["Action"] = (int)mAction;
                dtblInput.Rows.Add(drInput);

                DatabaseHelper dbHelper = new DatabaseHelper("Data Source=DESKTOP-R066Q43;User ID=sa;Password=@dmon@1;Initial Catalog=CDN;Connect Timeout=30;Encrypt=False;");
                long lngResult = dbHelper.ExecuteStoreProcedure("UspUserMaintain", out dsResult, out strExceptionMessage, dtblInput);

                if (lngResult != 0)
                    throw new Exception($"Failed to extract user data. Status = {lngResult}. Message = {strExceptionMessage}");
            }
            catch (Exception ex)
            {
                blnIsSuccess = false;
                strExceptionMessage = ex.Message;
            }

            return blnIsSuccess;
        }
    }

    public enum MaintainAction
    {
        Update = 0,
        Delete = 1,
        Insert = 2
    }
}
