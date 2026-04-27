namespace FleetFactory.Shared.Results
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        //use for successful responses
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        //use for error resposnes
        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default //empty value for the type
            };
        }
    }
}

//sample 
// { 
//   "success": true,
//   "message": "Vehicle created successfully",
//   "data": {}
// }