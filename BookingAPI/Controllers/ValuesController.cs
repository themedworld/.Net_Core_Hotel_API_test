using Microsoft.AspNetCore.Mvc;
using BookingAPI.Models;
using BookingAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BookingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingsController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("allocate")]
        public IActionResult AllocateRooms([FromBody] List<BookingRequest> bookingRequests)
        {
            if (bookingRequests == null || !bookingRequests.Any())
            {
                return BadRequest("Invalid booking requests.");
            }

            try
            {
                var allocations = _bookingService.AllocateRooms(bookingRequests);
                return Ok(new { Allocations = allocations });
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("calculate")]
        public IActionResult CalculatePrice([FromBody] PricingRequest pricingRequest)
        {
            if (pricingRequest == null)
            {
                return BadRequest("Invalid pricing request.");
            }

            try
            {
                var competitorPrices = _bookingService.FetchCompetitorPrices(pricingRequest.RoomType);
                var priceDetails = _bookingService.CalculatePrice(
                    pricingRequest.RoomType,
                    pricingRequest.Nights,
                    pricingRequest.Season,
                    pricingRequest.OccupancyRate,
                    competitorPrices
                );

                return Ok(new
                {
                    RoomType = pricingRequest.RoomType,
                    BasePrice = priceDetails.BasePrice,
                    PricePerNight= priceDetails.PricePerNight
                });
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
