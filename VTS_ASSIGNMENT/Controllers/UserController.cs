using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VTS_ASSIGNMENT.Models;
using VTS_ASSIGNMENT.Repository;

namespace VTS_ASSIGNMENT.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ResultModel> GetUsers()
        {
            try
            {
                var data = await userRepository.GetUsersAsync();
                return LibFuncs.getResponse(data);
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }

        [HttpPost]
        public async Task<ResultModel> AddUser(User user)
        {
            try
            {
                int response = await userRepository.AddUserAsync(user);
                return LibFuncs.getSavedResponse(response > 0, response);
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }

        [HttpPut]
        public async Task<ResultModel> UpdateUser(User user, int userId)
        {
            try
            {
                bool response = await userRepository.UpdateUserAsync(userId, user);
                return LibFuncs.getUpdatedResponse(response, userId);
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }

        [HttpPost]
        public async Task<ResultModel> UploadProfilePhoto(int userId)
        {
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    string filePath ="";
                    bool result = false;
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            Exception ex1 = new Exception("Please Upload image of type .jpg,.gif,.png.");
                            return LibFuncs.getExceptionResponse(ex1, "UploadProfilePhoto");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            Exception ex2 = new Exception("Please Upload a file upto 1 mb.");
                            return LibFuncs.getExceptionResponse(ex2, "UploadProfilePhoto");
                        }
                        else
                        {
                            filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName + extension);

                            postedFile.SaveAs(filePath);
                            result = await userRepository.UploadProfileImageAsync(userId, filePath);

                        }
                    }
                    return LibFuncs.getSavedResponse(result, userId);
                }
                Exception ex = new Exception("Please Upload a image.");
                return LibFuncs.getExceptionResponse(ex, "UploadProfilePhoto");
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }
    }
}
