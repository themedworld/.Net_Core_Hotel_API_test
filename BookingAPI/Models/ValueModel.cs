namespace BookingAPI.Models
{
    public class SpecialRequests
    {
        public string PreferredView { get; set; }
        public bool ConnectingRoom { get; set; }
    }

    public class ValueModel
    {
        public string RoomType { get; set; }
        public int Nights { get; set; }
        public string Season { get; set; }
        public SpecialRequests SpecialRequests { get; set; }
        
        // Constructeur par défaut est suffisant si vous ne souhaitez pas initialiser des valeurs spécifiques

        // Si vous avez besoin d'un constructeur pour initialiser des valeurs
        public ValueModel(string roomType, int nights, string season, SpecialRequests specialRequests )
        {
            RoomType = roomType;
            Nights = nights;
            Season = season;
            SpecialRequests = specialRequests;
        }
          
        
    }
     public class PricingRequest
    {
        public string RoomType { get; set; }
        public int Nights { get; set; }
        public string Season { get; set; }
        public int OccupancyRate { get; set; }
   
     public PricingRequest(string roomType, int nights, string season, int occupancyRate )
        {
            RoomType = roomType;
            Nights = nights;
            Season = season;
            OccupancyRate = occupancyRate;
        }
} }
