using System.Collections.Generic;
namespace ece4180.gpstracker.Models{
    public class Location{
        public long timeStamp {get; set;}
        public int tripId {get; set;}
        
        public double lat_ {get; set;}
        public double long_ {get; set;}
        

    }

    public class Trip{
        public int tripId {get; set;}
        public int devicenum {get;set;}
        public int status {get; set;}
        public long startTime {get; set;}
        public long endTime {get; set;}
        public double start_lat_ {get; set;}
        public double start_long_ {get; set;}
        public double end_lat_ {get; set;}
        public double end_long_ {get; set;}
        public List<Location> Locations {get; set;}
    }
    public class ModelPrinter{
        public static string Print(Trip trip){
            string mesg = $"{trip.tripId}, ${trip.status}, ${trip.startTime}";
            return mesg;
        }
    }
}