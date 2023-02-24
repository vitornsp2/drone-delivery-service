using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DroneDelivery.Domain.Validations
{
    public class FileContentValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IFormFile? file = (IFormFile?)value;
            if (file != null)
            {
                if (file.Length <= 0)
                    return new ValidationResult($"The file is empty.");
            }
            else
                return new ValidationResult($"The file was not uploaded into the request.");
            return ValidationResult.Success;
        }
    }
}