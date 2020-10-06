using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VTS_ASSIGNMENT.Models;
using VTS_ASSIGNMENT.Repository;

namespace VTS_ASSIGNMENT.Controllers
{
    public class VehicleController : ApiController
    {
        private readonly IVehicleRepository vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            this.vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public async Task<ResultModel> GetVehicles()
        {
            try
            {
                var data = await vehicleRepository.GetVehiclesAsync();
                return LibFuncs.getResponse(data);
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }

        [HttpPost]
        public async Task<ResultModel> GetSearchedvehicle(VehicleSearchModel SearchData, int PageSize = 10, int PageNo = 1)
        {
            try
            {
                var data = await vehicleRepository.GetSearchVehiclesAsync(PageSize, PageNo, SearchData);
                return LibFuncs.getResponse(data);
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }

        [HttpPost]
        public async Task<ResultModel> AddVehicle(Vehicle vehicle)
        {
            try
            {
                int response = await vehicleRepository.AddVehicleAsync(vehicle);
                return LibFuncs.getSavedResponse(response > 0, response);
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }

        [HttpPut]
        public async Task<ResultModel> UpdateVehicle(int id, [FromBody] Vehicle vehicle)
        {
            try
            {
                bool response = await vehicleRepository.UpdateVehicleAsync(id, vehicle);
                return LibFuncs.getUpdatedResponse(response, id);
            }
            catch (Exception ex)
            {
                var st = new StackTrace();
                return LibFuncs.getExceptionResponse(ex, st.GetFrame(0).GetMethod().DeclaringType.FullName);
            }
        }

    }
}
