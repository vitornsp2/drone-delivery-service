using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDelivery.Domain.Responses
{
    public class DroneDeliveryResponse
    {
        public string? Message { get; set; }
        public string[]? Errors { get; set; }
    }
}