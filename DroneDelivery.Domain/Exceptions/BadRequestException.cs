using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace DroneDelivery.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public int StatusCode { get; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }

        public BadRequestException(string message) : base(message) { StatusCode = 400; }

        public BadRequestException(string message, ValidationResult validationResult) : base(message) 
        { 
            StatusCode = 400;
            ValidationErrors = validationResult.ToDictionary();
        }
    }
}