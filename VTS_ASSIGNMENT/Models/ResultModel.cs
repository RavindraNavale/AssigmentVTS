using System;

namespace VTS_ASSIGNMENT.Models
{
    public class ResultModel : IDisposable
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Msg { get; set; }
        public Object Data { get; set; }
        public string ErrorMsg { get; set; }
        public int StatusCode { get; set; }

        public void Dispose()
        {

        }
    }
}