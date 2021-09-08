using Bogus;
using Bogus.Extensions.Sweden;
using Garage3._0.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Data
{
    public class SeedData
    {
        private static Faker fake;

        internal static async Task InitAsync(IServiceProvider services)
        {
            using(var db = services.GetRequiredService<Garage3_0Context>())
            {
                if (await db.Member.AnyAsync()) return;

                fake = new Faker("sv"); 

                var members = GetMembers();
                var parkingSpots = GetParkingSpots();
                var vehicleTypes = GetVehicleTypes();
                var vehicles = GetVehicles();
                await db.AddRangeAsync(members);
                await db.AddRangeAsync(parkingSpots);
                await db.AddRangeAsync(vehicleTypes);

                foreach (var vehicle in vehicles)
                {
                    var r = fake.Random.Int(1, 50);
                    vehicle.Member = members[r];
                    int randomVehicleType = fake.Random.Int(0, 8);
                    vehicle.VehicleType = vehicleTypes[randomVehicleType];
                }

                await db.AddRangeAsync(vehicles);
                var parkeds = GetParkeds(vehicles, parkingSpots);
                await db.AddRangeAsync(parkeds);

                await db.SaveChangesAsync();
            }
        }

        //Todo: Name = name.VehicleType... Function. Create list from database. Random assignment. FK removed
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
        //Todo: Check method. Either Unparked or Park only size 1. Then solve ParkingSpace
        private static List<Parked> GetParkeds(List<Vehicle> vehicles, List<ParkingSpot> parkingSpots)
        {
            var parkeds = new List<Parked>();
            
            foreach (var vehicle in vehicles)
            {
                foreach (var parkingSpot in parkingSpots)
                {
                    //Earlier if fake.Random.Int(0,2) == 0, giving approximately 50 % Parkeds)
                    if (vehicle.VehicleType.ParkingSize == 1)
                    {
                        var parked = new Parked
                        {
                            ParkingSpot = parkingSpot,
                            Vehicle = vehicle
                        };
                        parkeds.Add(parked);
                    }
                }
            }
            return parkeds;
        }
        
        private static List<ParkingSpot> GetParkingSpots()
        {
            var parkingSpots = new List<ParkingSpot>();

            for (int i = 0; i < 100; i++)
            {
                var parkingSpot = new ParkingSpot { };
                parkingSpots.Add(parkingSpot);
            }
            return parkingSpots;
        }

        private static List<Vehicle> GetVehicles()
        {
            var vehicles = new List<Vehicle>();

            for (int i = 0; i < 50; i++)
            {
                //Todo: Check output
                var regNo1 = fake.Lorem.Letter(3);     //Chars('\0', '\uffff', 3);
                var regNo2 = fake.Random.Number(0, 999);
                var color = fake.Commerce.Color();
                //Todo: Google if Model can easily be dependant on Manufacturer 
                var brand = fake.Vehicle.Manufacturer();
                var model = fake.Vehicle.Model();
                var wheels = fake.Random.Even(2, 4);
                //Todo: Check Date output mask. Build DateRandomizer. .Parse if not desired format
                var arrTime = fake.Date.Recent(7);

                var vehicle = new Vehicle
                {
                    RegNo = $"{regNo1}{regNo2}",
                    Color = color,
                    Brand = brand,
                    Model = model,
                    Wheels = wheels,
                    ArrivalTime = arrTime,
                };
                vehicles.Add(vehicle);
            }
            return vehicles;
        }

        private static List<Member> GetMembers()
        {
            var members = new List<Member>();

            for (int i = 0; i < 50; i++)
            {
                var pNo = fake.Person.Personnummer();
                var fName = fake.Name.FirstName();
                var lName = fake.Name.LastName();
                var regTime = fake.Date.Recent(14);
                //Todo: Compare to CheckPassword validation
                var pw = fake.Internet.Password(8);
                //Todo: Control distribution? Qeue? Stack? No randomization. List of 50 mshl, for loop to create and the pop
                var mshl = fake.Random.Number(0, 3);

                var member = new Member
                {
                    PersonNo = pNo,
                    FirstName = fName,
                    LastName = lName,
                    Email = fake.Internet.Email($"{fName} {lName}"),
                    RegistrationTime = regTime,
                    Password = pw,
                    //Todo: Check how casting turns out
                    MembershipLevel = (MembershipLevels)mshl
                };
                members.Add(member);
            }
            return members;
        }
    }
}
