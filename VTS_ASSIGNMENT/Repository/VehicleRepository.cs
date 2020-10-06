using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VTS_ASSIGNMENT.Models;

namespace VTS_ASSIGNMENT.Repository
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetVehiclesAsync();
        Task<List<Vehicle>> GetSearchVehiclesAsync(int PageSize, int PageNo, VehicleSearchModel searchData);
        Task<int> AddVehicleAsync(Vehicle vehicle);
        Task<bool> UpdateVehicleAsync(int VehicleID, Vehicle vehicle);
    }

    public class VehicleRepository:IVehicleRepository
    {
        readonly string CON_STRING;
        public VehicleRepository(string ConnectionString)
        {
            CON_STRING = ConnectionString;
        }


        #region IVehicleRepository implementation

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            List<Vehicle> lstData = new List<Vehicle>();

            using (SqlConnection con = new SqlConnection(CON_STRING))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PROC_GET_VEHICLES";

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (await dr.ReadAsync())
                    {
                        lstData.Add(new Vehicle
                        {
                            VehicleID = dr.GetSafeInt(0),
                            DeviceID = dr.GetSafeInt(1),
                            UserID = dr.GetSafeInt(2),

                            VehicleNumber = dr.GetSafeString(3),
                            VehicleType = dr.GetSafeString(4),
                            ChassisNumber = dr.GetSafeString(5),
                            EngineNumber = dr.GetSafeString(6),
                            ManufacturingYear = dr.GetSafeString(7),
                            LoadCarryingCapacity = dr.GetSafeDouble(8),
                            MakeOfVehicle = dr.GetSafeString(9),
                            ModelNumber = dr.GetSafeString(10),
                            BodyType = dr.GetSafeString(11),
                            OrganisationName = dr.GetSafeString(12)
                        });
                    }
                }
                con.Close();
            }

            return lstData;
        }
        public async Task<List<Vehicle>> GetSearchVehiclesAsync(int PageSize, int PageNo, VehicleSearchModel SearchData)
        {
            List<Vehicle> lstData = new List<Vehicle>();

            using (SqlConnection con = new SqlConnection(CON_STRING))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PROC_GET_VEHICLES_FOR_SEARCH";

                cmd.Parameters.Add("VEHICLE_NUMBER", SqlDbType.VarChar, 12).Value = SearchData.VehicleNumber;
                cmd.Parameters.Add("VEHICLE_TYPE", SqlDbType.VarChar, 10).Value = SearchData.VehicleType;
                cmd.Parameters.Add("CHASSIS_NUMBER", SqlDbType.VarChar, 30).Value = SearchData.ChassisNumber;
                cmd.Parameters.Add("ENGINE_NUMBER", SqlDbType.VarChar, 30).Value = SearchData.EngineNumber;
                cmd.Parameters.Add("MANUFACTURING_YEAR", SqlDbType.VarChar, 4).Value = SearchData.ManufacturingYear;

                cmd.Parameters.Add("USER_ID", SqlDbType.Int).Value = SearchData.UserID;

                cmd.Parameters.Add("PAGESIZE", SqlDbType.Int).Value = PageSize;
                cmd.Parameters.Add("PAGENO", SqlDbType.Int).Value = PageNo;

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (await dr.ReadAsync())
                    {
                        lstData.Add(new Vehicle
                        {
                            VehicleID = dr.GetSafeInt(0),
                            DeviceID = dr.GetSafeInt(1),
                            UserID = dr.GetSafeInt(2),

                            VehicleNumber = dr.GetSafeString(3),
                            VehicleType = dr.GetSafeString(4),
                            ChassisNumber = dr.GetSafeString(5),
                            EngineNumber = dr.GetSafeString(6),
                            ManufacturingYear = dr.GetSafeString(7),
                            LoadCarryingCapacity = dr.GetSafeDouble(8),
                            MakeOfVehicle = dr.GetSafeString(9),
                            ModelNumber = dr.GetSafeString(10),                 
                            BodyType = dr.GetSafeString(11),
                            OrganisationName = dr.GetSafeString(12)
                        });
                    }
                }
                con.Close();
            }

            return lstData;
        }
        public async Task<int> AddVehicleAsync(Vehicle vehicle)
        {
            int Id = -1;
            try
            {
                object objReturn = null;
                using (SqlConnection con = new SqlConnection(CON_STRING))
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_ADD_VEHICLE";

                    cmd.Parameters.Add("VEHICLE_NUMBER", SqlDbType.VarChar, 12).Value = vehicle.VehicleNumber;
                    cmd.Parameters.Add("VEHICLE_TYPE", SqlDbType.VarChar, 10).Value = vehicle.VehicleType;
                    cmd.Parameters.Add("CHASSIS_NUMBER", SqlDbType.VarChar, 30).Value = vehicle.ChassisNumber;
                    cmd.Parameters.Add("ENGINE_NUMBER", SqlDbType.VarChar, 30).Value = vehicle.EngineNumber;
                    cmd.Parameters.Add("MANUFACTURING_YEAR", SqlDbType.Char, 4).Value = vehicle.ManufacturingYear;
                    cmd.Parameters.Add("LOAD_CARRYING_CAPACITY", SqlDbType.Float).Value = vehicle.LoadCarryingCapacity;
                    cmd.Parameters.Add("MAKE_OF_VEHICLE", SqlDbType.VarChar, 30).Value = vehicle.MakeOfVehicle;
                    cmd.Parameters.Add("MODEL_NUMBER", SqlDbType.VarChar, 30).Value = vehicle.ModelNumber;
                    cmd.Parameters.Add("BODY_TYPE", SqlDbType.VarChar, 20).Value = vehicle.BodyType;
                    cmd.Parameters.Add("ORGANISATION_NAME", SqlDbType.VarChar, 100).Value = vehicle.OrganisationName;

                    cmd.Parameters.Add("DEVICE_ID", SqlDbType.Int).Value = vehicle.DeviceID;
                    cmd.Parameters.Add("USER_ID", SqlDbType.Int).Value = vehicle.UserID;

                    cmd.Parameters.Add("UPDATED_BY", SqlDbType.Int).Value = vehicle.UpdatedBy;

                    await con.OpenAsync();
                    objReturn = await cmd.ExecuteScalarAsync();
                    con.Close();
                }
                Id = Convert.ToInt32(objReturn);
            }
            catch
            {
                throw;
            }
            return Id;
        }

        public async Task<bool> UpdateVehicleAsync(int VehicleID, Vehicle vehicle)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CON_STRING))
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_UPDATE_VEHICLE";

                    cmd.Parameters.Add("VEHICLE_ID", SqlDbType.Int).Value = VehicleID;

                    cmd.Parameters.Add("VEHICLE_NUMBER", SqlDbType.VarChar, 12).Value = vehicle.VehicleNumber;
                    cmd.Parameters.Add("VEHICLE_TYPE", SqlDbType.VarChar, 10).Value = vehicle.VehicleType;
                    cmd.Parameters.Add("CHASSIS_NUMBER", SqlDbType.VarChar, 30).Value = vehicle.ChassisNumber;
                    cmd.Parameters.Add("ENGINE_NUMBER", SqlDbType.VarChar, 30).Value = vehicle.EngineNumber;
                    cmd.Parameters.Add("MANUFACTURING_YEAR", SqlDbType.Char, 4).Value = vehicle.ManufacturingYear;
                    cmd.Parameters.Add("LOAD_CARRYING_CAPACITY", SqlDbType.Float).Value = vehicle.LoadCarryingCapacity;
                    cmd.Parameters.Add("MAKE_OF_VEHICLE", SqlDbType.VarChar, 30).Value = vehicle.MakeOfVehicle;
                    cmd.Parameters.Add("MODEL_NUMBER", SqlDbType.VarChar, 30).Value = vehicle.ModelNumber;
                    cmd.Parameters.Add("BODY_TYPE", SqlDbType.VarChar, 20).Value = vehicle.BodyType;
                    cmd.Parameters.Add("ORGANISATION_NAME", SqlDbType.VarChar, 100).Value = vehicle.OrganisationName;

                    cmd.Parameters.Add("DEVICE_ID", SqlDbType.Int).Value = vehicle.DeviceID;
                    cmd.Parameters.Add("USER_ID", SqlDbType.Int).Value = vehicle.UserID;

                    cmd.Parameters.Add("UPDATED_BY", SqlDbType.Int).Value = vehicle.UpdatedBy;


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