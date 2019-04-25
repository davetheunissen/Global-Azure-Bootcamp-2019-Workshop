using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightDataFunctionApp
{
    public class Rootobject
    {
        public int time { get; set; }
        public string[][] states { get; set; }
    }

    public class Flight
    {
        [JsonProperty("id")]
        public string icao24 { get; set; }
        public string callsign { get; set; }
        public string originCountry { get; set; }
        public float? longitute { get; set; }
        public float? latitude { get; set; }
        public float? altitude { get; set; }
        public float? velocity { get; set; }
        public float? trueTrack { get; set; }

        public static Flight CreateFromData(string[] data)
        {
            return new Flight
            {
                icao24 = data[0].Trim(),
                callsign = data[1].Trim(),
                originCountry = data[2].Trim(),
                longitute = string.IsNullOrEmpty(data[5]) ? (float?)null : float.Parse(data[5].Trim()),
                latitude = string.IsNullOrEmpty(data[6]) ? (float?)null : float.Parse(data[6].Trim()),
                altitude = string.IsNullOrEmpty(data[7]) ? (float?)null : float.Parse(data[7].Trim()),
                velocity = string.IsNullOrEmpty(data[9]) ? (float?)null : float.Parse(data[9].Trim()),
                trueTrack = string.IsNullOrEmpty(data[10]) ? (float?)null : float.Parse(data[10].Trim())
            };
        }
    }
}
