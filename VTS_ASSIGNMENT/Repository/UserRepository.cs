using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using VTS_ASSIGNMENT.Models;

namespace VTS_ASSIGNMENT.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<int> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(int UserID, User user);
        Task<bool> UploadProfileImageAsync(int UserID, string filePath);
    }
    public class UserRepository:IUserRepository
    {
        readonly string CON_STRING;
        
        public UserRepository(string ConnectionString)
        {
            CON_STRING = ConnectionString;
        }

        #region IUserRepository implementation
        public async Task<List<User>> GetUsersAsync()
        {
            List<User> lstData = new List<User>();

            using (SqlConnection con = new SqlConnection(CON_STRING))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PROC_GET_USER";

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (await dr.ReadAsync())
                    {
                        lstData.Add(new User
                        {
                            UserID = dr.GetInt32(0),
                            Name = dr.GetString(1),
                            MobileNumber = dr.GetSafeString(2),
                            Organization = dr.GetSafeString(3),
                            Address = dr.GetSafeString(4),
                            Emailaddress = dr.GetSafeString(5),
                            Location = dr.GetSafeString(6),
                            Photopath = dr.GetSafeString(7),
                            UpdatedBy = dr.GetSafeInt(8),
                            lastUpdatedDateTime = dr.GetDateTime(9)
                            
                        });
                    }
                }
                con.Close();
            }

            return lstData;
        }

        public async Task<int> AddUserAsync(User user)
        {
            int newUserID = -1;
            try
            {
                object objReturn = null;
                using (SqlConnection con = new SqlConnection(CON_STRING))
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_ADD_USER";

                    cmd.Parameters.Add("NAME", SqlDbType.VarChar, 200).Value = user.Name;
                    cmd.Parameters.Add("MOBILE_NUMBER", SqlDbType.VarChar, 15).Value = user.MobileNumber;
                    cmd.Parameters.Add("ORGANIZATION", SqlDbType.VarChar, 400).Value = user.Organization;
                    cmd.Parameters.Add("ADDRESS", SqlDbType.VarChar, 400).Value = user.Address;
                    cmd.Parameters.Add("EMAIL_ADDRESS", SqlDbType.VarChar, 100).Value = user.Emailaddress;
                    cmd.Parameters.Add("LOCATION", SqlDbType.VarChar, 200).Value = user.Location;

                    cmd.Parameters.Add("UPDATED_BY", SqlDbType.Int).Value = user.UpdatedBy;

                    await con.OpenAsync();
                    objReturn = await cmd.ExecuteScalarAsync();
                    con.Close();
                }
                newUserID = Convert.ToInt32(objReturn);
            }
            catch
            {
                throw;
            }
            return newUserID;
        }

        public async Task<bool> UpdateUserAsync(int UserID, User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CON_STRING))
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_UPDATE_USER";

                    cmd.Parameters.Add("USER_ID", SqlDbType.Int).Value = UserID;

                    cmd.Parameters.Add("NAME", SqlDbType.VarChar, 200).Value = user.Name;
                    cmd.Parameters.Add("MOBILE_NUMBER", SqlDbType.VarChar, 15).Value = user.MobileNumber;
                    cmd.Parameters.Add("ORGANIZATION", SqlDbType.VarChar, 400).Value = user.Organization;
                    cmd.Parameters.Add("ADDRESS", SqlDbType.VarChar, 400).Value = user.Address;
                    cmd.Parameters.Add("EMAIL_ADDRESS", SqlDbType.VarChar, 100).Value = user.Emailaddress;
                    cmd.Parameters.Add("LOCATION", SqlDbType.VarChar, 200).Value = user.Location;
                    cmd.Parameters.Add("UPDATED_BY", SqlDbType.Int).Value = user.UpdatedBy;

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
            }
            catch
            {
                throw;
            }
            return true;
        }

        public async Task<bool> UploadProfileImageAsync(int UserID, string filePath)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CON_STRING))
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_UPDATE_USER_PROFILE_IMAGE";

                    cmd.Parameters.Add("USER_ID", SqlDbType.Int).Value = UserID;
                    cmd.Parameters.Add("PHOTO_PATH", SqlDbType.VarChar, 400).Value = filePath;

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
            }
            catch
            {
                throw;
            }
            return true;
        }
        #endregion
    }
}