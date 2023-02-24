using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DroneDelivery.Domain.Validations;
using Microsoft.AspNetCore.Http;

namespace DroneDelivery.Domain.Requests
{
    public class DroneDeliveryRequest
    {
        [Required]
        [DataType(DataType.Upload)]
        [FileNameValidation("Input")]
        [FileExtensionValidation(".txt")]
        [FileContentValidation]
        public IFormFile? DeliveriesFile { get; set; }
    }
}