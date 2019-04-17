using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;
using System;
namespace ece4180.gpstracker.Models{
    public class TripAccessor{
        private readonly TripContext tc_;
        public TripAccessor(TripContext tc){
            tc_ = tc;
        }
        public async Task<int> NumberOfTrips(){
            Task<List<Trip>> trips = tc_.Trips.ToListAsync();
            int i = await trips.ContinueWith((trips_) => trips_.Result.Count);
            return i;
        }
        public async Task<List<Trip>> GetAllTrips(){
            return await tc_.Trips.ToListAsync();
        }
        public async Task<List<Location>> GetTripLocations(int tripId){
            return await tc_.Locations
                .Where(loc => loc.tripId == tripId)
                .OrderBy(loc => loc.timeStamp)
                .ToListAsync();
        }
        public async Task<int> CreateTrip(int devicenum){
            int i = await NumberOfTrips();
            if ( i >= 0x7fffffff){
                return -1;
            }
            int counter__ = 1;
            do{
                Trip trip = new Trip { 
                        tripId = i + counter__,  
                        devicenum = devicenum,
                        status = 1,
                        startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()
                    };
                try{
                    await tc_.Trips.AddAsync(trip);
                    await tc_.SaveChangesAsync();
                    break;
                }catch(InvalidOperationException){
                    counter__ ++;
                    if(counter__ > 5){
                        return -1;
                    }
                }
            }while(true);
            return i + counter__;
        }
        public async Task<int> AddLocation(int tripId, double lat_, double long_){
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
        
        public async Task<int> TerminateTrip(int tripId){
            Trip trip = await tc_.Trips.FirstOrDefaultAsync(trip_ => trip_.tripId == tripId);
            if(trip == null){
                return -1;
            }
            trip.status = 2;
            tc_.Trips.Update(trip);
            await tc_.SaveChangesAsync();
            return 0;
        }
    }
}