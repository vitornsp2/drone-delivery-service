using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDelivery.Domain.Models
{
    public class DeliveryDataParametersDto
    {
        public List<Location>? LocationAtypicalDataOnLeft { get; set; }
        public List<Location>? LocationSubGroupTypicalData { get; set; }
        public List<Location>? LocationAtypicalDataOnRigth { get; set; }
    }
}