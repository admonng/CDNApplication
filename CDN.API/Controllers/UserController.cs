using CDN.API.Model;
using CDN.API.ObjectMember;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CDN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser(int iPageSize, int iPageNo)
        {
            UserData uData = null;
            string strExceptionMessage = null;

            try
            {
                UserModel usrModel = new UserModel();
                bool blnIsSuccess = usrModel.GetUser(iPageSize, iPageNo, out uData, out strExceptionMessage);

                if (!blnIsSuccess)
                    throw new Exception(strExceptionMessage);

                if (uData == null || uData.UserDataList == null || (uData.UserDataList != null && uData.UserDataList.Count == 0))
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(uData);
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] User user)
        {
            string strExceptionMessage = null;

            try
            {
                UserModel usrModel = new UserModel();
                bool blnIsSuccess = usrModel.ManipulateUserData(user, MaintainAction.Insert, out strExceptionMessage);

                if (!blnIsSuccess)
                    throw new Exception(strExceptionMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            string strExceptionMessage = null;

            try
            {
                UserModel usrModel = new UserModel();
                bool blnIsSuccess = usrModel.ManipulateUserData(user, MaintainAction.Update, out strExceptionMessage);

                if (!blnIsSuccess)
                    throw new Exception(strExceptionMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromBody] User user)
        {
            string strExceptionMessage = null;

            try
            {
                UserModel usrModel = new UserModel();
                bool blnIsSuccess = usrModel.ManipulateUserData(user, MaintainAction.Delete, out strExceptionMessage);

                if (!blnIsSuccess)
                    throw new Exception(strExceptionMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
