using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;
using System;
namespace ece4180.gpstracker.Models{

    public class ErrorJSONResult{
        public string reason {set;get;}
        public ErrorJSONResult(string reason_){
            reason = reason_;
        }
    }
    public static class TRIPSTATUS{
        public const int INVALID = -1;
        public const int ALL = 0;
        public const int RUNNING = 1; // running
        public const int TERMINATED = 2; // terminated
    }
    public class TripAccessor{
        private readonly TripContext tc_;
        public TripAccessor(TripContext tc){
            tc_ = tc;
        }
        private async Task<int> GetNextTripId(){
            List<Trip> trips = await tc_.Trips.OrderBy(t_ => t_.tripId).ToListAsync();
            int count = 1;
            foreach(Trip t in trips){
                if(t.tripId != count){
                    break;
                }
                count++;
                if(count == Int32.MaxValue){
                    return -1;
                }
            }
            return count;
        }
        public async Task<List<Trip>> GetTripStatus(){
            return await tc_.Trips.OrderBy(t_ => t_.startTime).ToListAsync();
        }
        public async Task<Trip> GetTripStatus(int tripId){
            return await tc_.Trips.Where(t_ => t_.tripId == tripId).FirstOrDefaultAsync();
        }
        public async Task<List<Location>> GetTripLocations(int tripId, int status, long since){
            Trip t = null;
            if(status == TRIPSTATUS.ALL){
                t = await tc_.Trips.Where(t_ => t_.tripId == tripId)
                    .SingleOrDefaultAsync();
            }else{
                t = await tc_.Trips.Where(t_ => t_.tripId == tripId)
                    .Where(t_ => t_.status == status)
                    .SingleOrDefaultAsync();
            }
            if(t == null){
                Console.WriteLine($"Cannot find the trip {tripId} with status {status}");
                return null;
            }
            if(since == -1){
                return await tc_.Locations
                    .Where(loc => loc.tripId == tripId)    
                    .OrderBy(loc => loc.timeStamp)
                    .ToListAsync();
            }else{
                return await tc_.Locations
                    .Where(loc => loc.tripId == tripId)
                    .Where(loc => loc.timeStamp >= since)
                    .OrderBy(loc => loc.timeStamp)
                    .ToListAsync();
            }
        }
        public async Task<int> CreateTrip(int devicenum){
            int tripId = await GetNextTripId();
            if(tripId == -1){
                return -1;
            }
            Trip trip = new Trip { 
                tripId = tripId,  
                devicenum = devicenum,
                status = 1,
                startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()
            };
            tc_.Trips.Add(trip);
            await tc_.SaveChangesAsync();
            return tripId;
        }
        /*Add location to the database along with its tripId 
        * Error Case
        * 1. invalid longtitude or/and latitude
        * 2. tripId does not exit
        * 3. trip is already terminated.
        */
        public async Task<int> AddLocation(int tripId, double lat_, double long_){
            //Console.WriteLine($"{tripId}/{lat_}{long_}");
            if(lat_ > 90 || lat_ < -90 || long_ > 180 || long_ < -180){
                Console.WriteLine("longtitude and/or latitude are invalid");
                return -1;
            }
            Trip t = await tc_.Trips.Where(t_ => t_.tripId == tripId).SingleOrDefaultAsync();
            if(t == null){
                Console.WriteLine($"TripId {tripId} does not exist");
                return -1;
            }
            if(t.status == TRIPSTATUS.TERMINATED){
                Console.WriteLine($"Trip {tripId} is already terminated");
                return -1;
            }
            Location location = new Location{
                tripId = tripId,
                timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds(),
                lat_ = lat_,
                long_ = long_
            };
            try{
                await tc_.Locations.AddAsync(location);
                await tc_.SaveChangesAsync();
            }catch(InvalidOperationException){
                return -1;
            }
            return 0;
        }
        /*Terminate a trip by setting its status to TRIPSTATUS.TERMINATED
        * Error case:
        * 1. tripId {tripId} does not exist.
         */
        public async Task<int> TerminateTrip(int tripId){
            Trip trip = await tc_.Trips.FirstOrDefaultAsync(trip_ => trip_.tripId == tripId);
            if(trip == null){
                return -1;
            }
            trip.status = TRIPSTATUS.TERMINATED;
            tc_.Trips.Update(trip);
            await tc_.SaveChangesAsync();
            return 0;
        }
        /*Delete a trip along with its locations */
        public async Task<int> DeleteTrip(int tripId){
            if(tripId == -1){
                // delete all
                List<Trip> trips = await tc_.Trips.ToListAsync();
                List<Location> locs = await tc_.Locations.ToListAsync();
                tc_.Trips.RemoveRange(trips);
                tc_.Locations.RemoveRange(locs);
                await tc_.SaveChangesAsync();
                return 0;
            }else{
                Trip t = await tc_.Trips.Where(t_ => t_.tripId == tripId).FirstOrDefaultAsync();
                if(t == null){
                    return 0;
                }
                tc_.Trips.Remove(t);
                List<Location> locs = await tc_.Locations.Where(loc => loc.tripId == tripId).ToListAsync();
                tc_.Locations.RemoveRange(locs);
                await tc_.SaveChangesAsync();
                return 0;
            }
        }
    }
}