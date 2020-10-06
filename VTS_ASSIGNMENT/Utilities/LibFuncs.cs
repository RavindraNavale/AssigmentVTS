
using System;
using VTS_ASSIGNMENT.Models;

namespace VTS_ASSIGNMENT
{
    //Common Functions for Appropriate Response.
    static class LibFuncs
    {
        public static ResultModel getResponse(Object data, string msg = "")
        {
            try
            {
                using (ResultModel result = new ResultModel())
                {
                    result.Id = 0;
                    result.Status = "Success";
                    result.StatusCode = 200;
                    result.Msg = msg == "" ? "Record has been fetched successfully..!!" : msg;
                    result.Data = data;
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static ResultModel getSavedResponse(bool blnResultFlag, int savedId, Object data = null)
        {
            try
            {
                using (ResultModel result = new ResultModel())
                {
                    result.Id = blnResultFlag == true ? savedId : -1;
                    result.StatusCode = blnResultFlag == true ? 200 : 1003;
                    result.Status = blnResultFlag == true ? "Success" : "Error";
                    result.Msg = blnResultFlag == true ? "Record has been saved..!!" : "Record can not be saved..!!";
                    result.Data = blnResultFlag == true ? data : null;
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static ResultModel getUpdatedResponse(bool blnResultFlag, int updatedId, Object data = null)
        {
            try
            {
                using (ResultModel result = new ResultModel())
                {
                    result.Id = blnResultFlag == true ? updatedId : -1;
                    result.StatusCode = blnResultFlag == true ? 200 : 1003;
                    result.Status = blnResultFlag == true ? "Success" : "Error";
                    result.Msg = blnResultFlag == true ? "Record has been updated..!!" : "Record can not be updated..!!";
                    result.Data = blnResultFlag == true ? data : null;
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static ResultModel getExceptionResponse(Exception ex, string strMethodName)
        {

            ApiLog.WriteLog(ex.Message + " : " + Convert.ToString(ex.InnerException), strMethodName);
            using (ResultModel result = new ResultModel())
            {
                result.Id = -1;
                result.Status = "Error";
                result.StatusCode = ex.Source == "DBAPI" ? 1002 : 1001;
                result.Msg = ex.Message;
                result.ErrorMsg = ex.Message;
                return result;
            }
        }
    }
}