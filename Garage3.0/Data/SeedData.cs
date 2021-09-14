using Bogus;
using Bogus.Extensions.Sweden;
using Garage3._0.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage3._0.Data
{
    public class SeedData
    {
        private static Faker fake;

        internal static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<Garage3_0Context>())
            {
                if (await db.Member.AnyAsync()) return;

                fake = new Faker();

                List<Member> membersList = GetMembers(50);
                var parkingSpots = GetParkingSpots(100);
                var vehicleTypes = GetVehicleTypes();
                await db.AddRangeAsync(membersList);
                await db.AddRangeAsync(parkingSpots);
                await db.AddRangeAsync(vehicleTypes);

                await db.SaveChangesAsync();

                var vehicles = GetVehicles(50, membersList, vehicleTypes);
                await db.AddRangeAsync(vehicles);

                await db.SaveChangesAsync();

                var parkeds = GetParkeds(vehicles, parkingSpots);
                await db.AddRangeAsync(parkeds);
                
                await db.SaveChangesAsync();
            }
        }

        private static List<VehicleType> GetVehicleTypes()
        {
            List<VehicleType> vehicleTypes = new List<VehicleType>
            {
                new VehicleType { Name = "Car", ParkingSize = 1 },
                new VehicleType { Name = "Motorcycle", ParkingSize = (1/3) },
                new VehicleType { Name = "Moped", ParkingSize = (1/3) },
                new VehicleType { Name = "Quadbike", ParkingSize = 1 },
                new VehicleType { Name = "Minivan", ParkingSize = 1 },
                new VehicleType { Name = "Van", ParkingSize = 1 },
                new VehicleType { Name = "Truck", ParkingSize = 2 },
                new VehicleType { Name = "Trailer", ParkingSize = 1 },
                new VehicleType { Name = "Bus", ParkingSize = 3 }
            };
            return vehicleTypes;
        }
        private static List<Parked> GetParkeds(List<Vehicle> vehicles, List<ParkingSpot> parkingSpots)
        {
            var parkeds = new List<Parked>();

            //var newVehicles = vehicles.FindAll(v => v.VehicleType.ParkingSize == 1);

            foreach (var vehicle in vehicles)
                {
                        var parked = new Parked
                        {
                            VehicleId = vehicle.Id,
                        };
                        parked.ParkingSpotId = vehicle.Id;
                        parkeds.Add(parked);
                }
            return parkeds;
        }

        private static List<ParkingSpot> GetParkingSpots(int amount)
        {
            var parkingSpots = new List<ParkingSpot>();

            for (int i = 0; i < amount; i++)
            {
                var parkingSpot = new ParkingSpot {
                    ParkingSpotNumber = i + 1
                };
                parkingSpots.Add(parkingSpot);
            }
            return parkingSpots;
        }

        private static List<Vehicle> GetVehicles(int amount, List<Member> membersList, List<VehicleType> vehicleTypes)
        {
            var vehicles = new List<Vehicle>();

            for (int i = 0; i < amount; i++)
            {
                var vehicle = new Vehicle
                {
                    RegNo = fake.Random.Replace("???###").ToUpper(),
                    VehicleType = fake.Random.ListItem<VehicleType>(vehicleTypes),
                    Member = fake.Random.ListItem<Member>(membersList),
                    Color = fake.Commerce.Color(),
                    Brand = fake.Vehicle.Manufacturer(),
                    Model = fake.Vehicle.Model(),
                    Wheels = fake.Random.Even(2, 4),
                    ArrivalTime = fake.Date.Recent(7),
                    IsParked = fake.Random.Bool()
                };
                vehicles.Add(vehicle);
            }


            foreach (var vehicle in vehicles)
            {
                vehicle.MemberId = vehicle.Member.Id;
            }

            return vehicles;
        }

        private static List<Member> GetMembers(int amount)
        {
            var membersList = new List<Member>();

            for (int i = 0; i < amount; i++)
            {
                int year = fake.Random.Int(1931, 2002);
                var month = fake.Random.Int(11, 12);
                var day = fake.Random.Int(10, 28);
                var pNo = new StringBuilder();
                pNo.Append(year);
                pNo.Append(month);
                pNo.Append(day);
                pNo.Append(fake.Random.Int(0001, 9999));
                var firstName = fake.Name.FirstName();
                var lastName = fake.Name.LastName();
                int mshl = fake.Random.Number(0, 3);

                var member = new Member
                {
                    PersonNo = pNo.ToString(),
                    FirstName = firstName,
                    LastName = lastName,
                    Email = fake.Internet.Email($"{firstName} {lastName}"),
                    RegistrationTime = fake.Date.Recent(14),
                    Password = fake.Internet.Password(8),
                    MembershipLevel = (MembershipLevels)mshl
                };
                membersList.Add(member);
            }
            return membersList;
        }
    }
}