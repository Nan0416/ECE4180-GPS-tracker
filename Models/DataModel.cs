
namespace ece4180.gpstracker.controllers{
    public class Location{
        public int tripId;
        public double lat_;
        public double long_;   
        public int timeStamp;

    }

    public class Trip{
        public int tripId;
        public int status;
        public int startTime;
        public int endTime;
        public double start_lat_;
        public double start_long_;
         public double end_lat_;
        public double end_long_;
    }
}