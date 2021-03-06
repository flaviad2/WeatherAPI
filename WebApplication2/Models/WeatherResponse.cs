using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApplication2.WeatherRepository;

namespace WebApplication2.Models
{

   
    public class WeatherResponse
    {
        
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        [JsonProperty("Time")]
        public TimeSpan Time { get; set; }

        [JsonProperty("MinimumTemperature")]
        public int MinimumTemperature { get; set; }

        [JsonProperty("MaximumTemperature")]
        public int MaximumTemperature { get; set; }

        [JsonProperty("PrecipitationsProbability")]
        public int PrecipitationsProbability { get; set; }


        [JsonProperty("AtmosphericFenomens")]
        public bool AtmosphericFenomens { get; set; }

        [JsonProperty("OtherInformation")]
        public String OtherInformation { get; set; }


        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("DataSource")]//
        public SourceEnum DataSource { get; set; }

        public WeatherResponse(int id, DateTime date, TimeSpan time, int minimumTemperature, int maximumTemperature, int precipitationsProbability, bool atmosphericFenomens, string otherInformation, SourceEnum dataSource)
        {
            Id = id;
            Date = date;
            Time = time;
            MinimumTemperature = minimumTemperature;
            MaximumTemperature = maximumTemperature;
            PrecipitationsProbability = precipitationsProbability;
            AtmosphericFenomens = atmosphericFenomens;
            OtherInformation = otherInformation;
            DataSource = dataSource;
        }

    }
}
