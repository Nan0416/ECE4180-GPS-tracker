
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
namespace ece4180.gpstracker.Models{
    public class DbTest{
        public static void __Main(string [] args){
            using (TripContext db = new TripContext(null))
            {
                TripAccessor ta = new TripAccessor(db);
                ta.GetAllTrips().ContinueWith(r => {
                    List<Trip> trips = r.Result;
                    foreach(Trip t in trips){
                        Console.WriteLine($"===> {t.tripId} {t.status} {t.startTime}");
                    }
                });
                // add new trip
                ta.CreateTrip(10)
                    .ContinueWith((r)=>{
                        Console.WriteLine(r.Result);
                    })
                    .Wait();
                // add new location 
                ta.AddLocation(2, 34.23, 123.323).Wait();
                // terminate a trip
                ta.TerminateTrip(1).Wait();
                // print locations of a trip
                ta.GetTripLocations(2).ContinueWith(r => {
                    List<Location> locs = r.Result;
                    if( locs != null){
                        //Console.WriteLine($"===> {t.tripId} {t.status} {t.startTime}");
                        foreach(Location loc in locs){
                            Console.WriteLine($"~~~~ {loc.timeStamp} {loc.lat_} {loc.long_}");
                        }
                    }
                });
                db.Locations.ToListAsync().ContinueWith(r => {
                    List<Location> i = r.Result;
                    Console.WriteLine(i.Count);
                });

            }
        }
    }
}