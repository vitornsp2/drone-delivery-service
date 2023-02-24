using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneDelivery.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace DroneDelivery.Domain.Interfaces
{
    public interface IDeliveryService
    {
        Task<MemoryStream> GetTrips(IFormFile file);
    }
}