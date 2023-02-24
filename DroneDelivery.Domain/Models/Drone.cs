using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDelivery.Domain.Models
{
    public class Drone : BaseModel
    {
		  public int MaximumWeight { get; set; }
          public List<List<Location>> Trips { get; set; }

          public Drone()
          {
            Trips = new List<List<Location>>();
          }
    }
}