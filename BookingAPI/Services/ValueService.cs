using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using BookingAPI.Models;

namespace BookingAPI.Services
{
    public class BookingService
    {
        private readonly Dictionary<string, decimal> _basePrices = new Dictionary<string, decimal>
        {
            { "Standard", 100 },
            { "Deluxe", 150 },
            { "Suite", 250 }
        };

        private readonly Dictionary<string, decimal> _seasonalModifiers = new Dictionary<string, decimal>
        {
            { "Off-Season", -0.20m },
            { "Peak Season", 0.30m },
        };

        private readonly Dictionary<string, decimal> _occupancyModifiers = new Dictionary<string, decimal>
        {
            { "Low", -0.10m },
            { "Medium", 0.00m },
            { "High", 0.20m }
        };

        private readonly string _csvFilePath = @"C:\Users\XPS\Desktop\Nouveau dossier (7)\BookingAPI\Data_Resources\competitor_prices.csv";
        private int _nextRoomId = 100; // Simple ID generator for demonstration

        public List<AllocationResult> AllocateRooms(List<BookingRequest> bookingRequests)
        {
            var results = new List<AllocationResult>();

            foreach (var request in bookingRequests)
            {
                var competitorPrices = FetchCompetitorPrices(request.RoomType);
                var priceDetails = CalculatePrice(
                    request.RoomType, 
                    request.Nights,
                    request.Season, 
                    request.OccupancyRate, 
                    competitorPrices
                );

                results.Add(new AllocationResult
                {
                    RoomType = request.RoomType,
                    AllocatedRoomId = _nextRoomId++,
                    totalPrice = priceDetails.AdjustedPrice,
                    SpecialRequests = request.SpecialRequests
                });
            }

            return results;
        }

        public PriceDetails CalculatePrice(string roomType, int nights, string season, int occupancyRate, List<decimal> competitorPrices)
        {
            if (!_basePrices.ContainsKey(roomType))
                throw new ArgumentException("Invalid room type.");

            var basePrice = _basePrices[roomType];
            var seasonalModifier = _seasonalModifiers.GetValueOrDefault(season, 0);
            var occupancyModifier = GetOccupancyModifier(occupancyRate);

            // Calculate price per night
            var pricePerNight = basePrice * (1 + seasonalModifier) * (1 + occupancyModifier);

            // Adjust price based on competitor prices
            if (competitorPrices.Any())
            {
                var averageCompetitorPrice = competitorPrices.Average();
                var priceAdjustment = (averageCompetitorPrice - pricePerNight) * 0.10m;
                pricePerNight += priceAdjustment;
            }

            // Calculate adjusted price for the number of nights
            var totalPrice = pricePerNight * nights;

            return new PriceDetails
            {
                BasePrice = basePrice,
                AdjustedPrice = Math.Max(totalPrice, 0), // Ensure the price is not negative
                PricePerNight = pricePerNight
            };
        }

        private decimal GetOccupancyModifier(int occupancyRate)
        {
            if (occupancyRate <= 30)
                return _occupancyModifiers["Low"];
            else if (occupancyRate <= 70)
                return _occupancyModifiers["Medium"];
            else
                return _occupancyModifiers["High"];
        }

        public List<decimal> FetchCompetitorPrices(string roomType)
        {
            var competitorPrices = new List<decimal>();

            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                foreach (var record in records)
                {
                    var roomTypeRecord = record.RoomType as string;
                    var basePrice = Convert.ToDecimal(record.BasePrice);

                    if (roomType == roomTypeRecord)
                    {
                        competitorPrices.Add(basePrice);
                    }
                }
            }

            return competitorPrices;
        }
    }

    public class PriceDetails
    {
        public decimal BasePrice { get; set; }
        public decimal AdjustedPrice { get; set; }
        public decimal PricePerNight { get; set; }
    }

    public class AllocationResult
    {
        public string RoomType { get; set; }
        public int AllocatedRoomId { get; set; }
        public decimal totalPrice { get; set; }
        public SpecialRequests SpecialRequests { get; set; }
    }

    public class BookingRequest
    {
        public string RoomType { get; set; }
        public int Nights { get; set; }
        public string Season { get; set; }
        public int OccupancyRate { get; set; }
        public SpecialRequests SpecialRequests { get; set; }
    }
}
