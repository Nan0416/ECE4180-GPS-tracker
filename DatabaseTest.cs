
using Microsoft.EntityFrameworkCore;
using System;
namespace ece4180.gpstracker.Models{
    public class DbTest{
        public static void Main(string [] args){
            using (TripContext db = new TripContext())
            {
                
                db.Trips.Add(new Trip { 
                    tripId = 19,  
                    status = 1,
                    startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()});
                int count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
            }
        }
    }
}