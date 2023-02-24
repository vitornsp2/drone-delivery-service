using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneDelivery.Domain.Exceptions;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Domain.Validations.models;
using Microsoft.AspNetCore.Http;
using DroneDelivery.Domain.Extensions;
using System.Text;

namespace DroneDelivery.Service
{
    public class DeliveryService : IDeliveryService
    {
        public async Task<MemoryStream> GetTrips(IFormFile file)
        {
            var data = await GetDataFromFileAsync(file);
            var drones = CreateTrips(data);
            return await GenerateDroneOutput(drones);
        }

        #region Private Methods

        private List<Drone> CreateTrips((List<Drone>, List<Location>) data)
        {
            var drones = data.Item1.OrderBy(d => d.MaximumWeight).ToList();
            var locations = data.Item2;
            int maxWeight = drones.Max(d => d.MaximumWeight);

            //Remove any packages that cant be carried
            List<Location> undeliverables = locations.Where(p => p.Weight > maxWeight).ToList();
            locations.RemoveAll(p => p.Weight > maxWeight);

            //Goes through the list of drones ordered from smallest weight limit to greatest and finds the most packages a the drone can carry.
            while (locations.Count() > 0)
            {
                foreach (Drone drone in drones)
                {
                    var locationsForDroneLst = ClosestWeightList(locations, drone.MaximumWeight);
                    foreach (Location package in locationsForDroneLst)
                    {
                        locations.Remove(package);
                    }
                    if (locationsForDroneLst.Count() > 0)
                    {
                        drone.Trips.Add(locationsForDroneLst);
                    }
                    if (locations.Count() == 0)
                    {
                        break;
                    }
                }
            }
            return drones;
        }

        private async Task<MemoryStream> GenerateDroneOutput(List<Drone> drones)
        {
            MemoryStream stream = new();
            foreach (Drone drone in drones)
            {
                int tripNum = 1;
                await stream.WriteAsync(Encoding.UTF8.GetBytes($"{drone.Name}\n\r"));
                foreach (var trip in drone.Trips)
                {
                    await stream.WriteAsync(Encoding.UTF8.GetBytes($"Trip #{tripNum}\n\r"));
                    foreach (var location in trip)
                    {
                        var comma = location.Name == trip.Last().Name ? string.Empty : ",";
                        await stream.WriteAsync(Encoding.UTF8.GetBytes($"{location.Name}{comma}"));
                    }
                    await stream.WriteAsync(Encoding.UTF8.GetBytes($"\n\r"));
                    tripNum += 1;
                }
            }

            return stream;
        }

        private static List<Location> ClosestWeightList(List<Location> packages, int maxWeight)
        {
            var target = Enumerable.Range(1, packages.Count)
                .SelectMany(p => packages.Combinations(p))
                .Where(p => p.Sum(x => x.Weight) <= maxWeight)
                .OrderByDescending(p => p.Count()).FirstOrDefault();

            return target != null ? target.ToList() : new List<Location>();
        }

        private async Task<(List<Drone>, List<Location>)> GetDataFromFileAsync(IFormFile file)
        {
            List<Drone> droneList = new();
            List<Location> locationList = new();
            int lineCount = 0;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (lineCount == 0)
                    {
                        ValidateMissingLines(line, "Drone");

                        var data = line.Split(",");
                        ValidatePairValue(data, "Drone");

                        for (int index = 0; index < data?.Length; index += 2)
                        {
                            var drone = new Drone()
                            {
                                Name = data[index],
                                MaximumWeight = Convert.ToInt32(RemoveCharaters(data[index + 1]))
                            };

                            var droneValidator = new DroneValidator();
                            var validationResult = await droneValidator.ValidateAsync(drone);

                            if (validationResult.Errors.Any())
                                throw new BadRequestException("Invalid Drone", validationResult);

                            droneList.Add(drone);
                        }
                    }
                    else
                    {
                        ValidateMissingLines(line, "Location");
                        var data = line.Split(",");
                        ValidatePairValue(data, "Location");
                        var location = new Location()
                        {
                            Name = data[0],
                            Weight = Convert.ToInt32(RemoveCharaters(data[1]))
                        };

                        var locationValidator = new LocationValidator();
                        var validationResult = await locationValidator.ValidateAsync(location);

                        if (validationResult.Errors.Any())
                            throw new BadRequestException("Invalid Location", validationResult);

                        locationList.Add(location);
                    }
                    lineCount++;
                }
            }
            return (droneList, locationList);
        }

        public void ValidateMissingLines(string data, string objectType)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new BadRequestException($"{objectType}'s data is missing or a white line has been found.");
        }

        public void ValidatePairValue(IEnumerable<string> data, string objectType)
        {
            if (data.Count() % 2 != 0)
                throw new BadRequestException($"A {objectType}´s name or weigth is missing.");
        }

        public string RemoveCharaters(string value)
        {
            return value.Replace("[", string.Empty).Replace("]", string.Empty).Trim();
        }

        #endregion
    }
}