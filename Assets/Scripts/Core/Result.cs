using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsFailure => !IsSuccess;

        public static Result Success()
        {
            return new Result
            {
                IsSuccess = true,
                ErrorCode = string.Empty,
                ErrorMessage = string.Empty
            };
        }

        public static Result Failure(string errorCode, string errorMessage)
        {
            return new Result
            {
                IsSuccess = false,
                ErrorCode = errorCode ?? string.Empty,
                ErrorMessage = errorMessage ?? string.Empty
            };
        }
    }
}
