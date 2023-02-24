using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Requests;
using DroneDelivery.Service;
using Microsoft.AspNetCore.Mvc;

namespace DroneDelivery.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DroneDeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;
    public DroneDeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessDeliveries([FromForm] DroneDeliveryRequest DroneDeliveryRequest)
    {
        var memoryStream = await _deliveryService.GetTrips(DroneDeliveryRequest.DeliveriesFile);
        return File(memoryStream.ToArray(), "text/plain", "Output.txt");
    }
}
