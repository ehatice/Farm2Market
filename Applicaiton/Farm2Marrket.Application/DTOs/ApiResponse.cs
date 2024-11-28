using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public ApiResponse(T data, bool succeeded, string error = null)
        {
            Data = data;
            Succeeded = succeeded;
            Error = error;
        }
        public static ApiResponse<T> Success(T data) => new ApiResponse<T>(data, true);
        public static ApiResponse<T> Failure(string error) => new ApiResponse<T>(default, false, error);
    }
}
