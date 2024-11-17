﻿using WebApplicationFP2.Models;
using System.Linq;

namespace WebApplicationFP2.Storage
{
    public static class AirportStorage
    {
        public static List<Airport> Airports = new List<Airport>();

        public static void AddAirport(Airport airport)
        {
            if (airport != null && !string.IsNullOrEmpty(airport.AirportCode))
            {
                Airports.Add(airport);
            }
        }

        public static void ClearAirports()
        {
            Airports.Clear();
        }

        public static List<Airport> SearchAirports(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Airport>();

            var trimmedQuery = query.Trim().ToUpper();

            return Airports
                .Where(a => a.AirportCode.ToUpper().Contains(trimmedQuery))
                .ToList();
        }
    }
}