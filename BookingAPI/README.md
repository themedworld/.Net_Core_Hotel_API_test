The Booking API is a robust system designed to handle hotel room bookings efficiently. Built with ASP.NET Core, it supports functionalities for allocating rooms and calculating prices based on various factors such as seasonal demand, occupancy rates, and competitor pricing. This document provides an overview of the system, the approach to solving challenging problems, and information about the production URL for testing.
Approach to Difficult Problems
1. Room Allocation and Price Calculation
Problem: Allocating rooms and calculating prices dynamically based on user requests, market conditions, and competitor pricing.

Approach:

Input Validation: Ensure that the input data is valid and complete before processing to prevent errors and handle incorrect requests.
Base Prices and Modifiers: Use predefined base prices and apply seasonal and occupancy modifiers to calculate the adjusted price per night. This dynamic approach ensures that pricing reflects current market conditions.
Competitor Pricing: Fetch competitor prices from a CSV file to adjust the pricing competitively. This involves reading data from a file, processing it, and using it to influence the final pricing.
Error Handling: Implement robust error handling to manage unexpected issues gracefully. Use try-catch blocks to handle exceptions and return meaningful HTTP status codes and messages to clients.
2. CSV Handling for Competitor Prices
Problem: Reading and processing competitor pricing data from a CSV file to adjust room prices.

Approach:

CSV Reading: Use the CsvHelper library to read and parse the CSV file. This library simplifies the process of extracting data from CSV files and converting it into usable objects.
Data Matching: Filter and match room types between the CSV data and user requests to gather relevant competitor prices.
Price Adjustment: Calculate price adjustments based on competitor prices and ensure the adjusted price is competitive but not negative.
3. Dynamic Pricing Modifiers
Problem: Adjusting prices based on seasonal changes and occupancy rates.

Approach:

Seasonal Modifiers: Use a dictionary to store seasonal modifiers and adjust base prices accordingly.
Occupancy Modifiers: Implement a method to determine the occupancy modifier based on the occupancy rate, using predefined thresholds and modifiers.
4. Error Handling and Logging
Problem: Handling errors and providing meaningful feedback to users.

Approach:

Exception Handling: Use try-catch blocks to handle exceptions and return appropriate HTTP status codes and error messages.
Logging: Although basic error handling is implemented, future improvements include integrating a comprehensive logging framework for better error tracking and debugging.
Endpoints
Allocate Rooms

URL: /allocate
Method: POST
Request Body: List of BookingRequest objects
Response: JSON object with room allocations and details
Example Request:
[
  {
    "roomType": "Deluxe",
      "nights": 3,
      "season": "Peak Season",
      "specialRequests": {
        "preferredView": "sea",
        "connectingRoom": false
    }
  },
  {
"roomType": "Standard",
      "nights": 2,
      "season": "Off-Season",
      "specialRequests": {
        "preferredView": "garden",
        "connectingRoom": true
    }
  },
  {
    "roomType": "Suite",
      "nights": 1,
      "season": "High Occupancy",
      "specialRequests": {
        "preferredView": "city",
        "connectingRoom": false
    }
  }
]

Calculate Price

URL: /calculate
Method: POST
Request Body: PricingRequest object
Response: JSON object with price details
Example Request:
{
  "roomType": "Deluxe",
  "season": "Off-Season",
  "occupancyRate": 32,
  "competitorPrices": ["Nazl Al Jabal"]
}

Production URL
The application is deployed and available for testing at the following URL:

http://localhost:5171/bookings

Please use this URL to interact with the API endpoints and test the system's functionality.