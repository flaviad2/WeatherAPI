using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Enum.Ext.NewtonsoftJson;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using WebApplication2.Models;

namespace WebApplication2.WeatherRepository
{
  
    

    public class WeatherEntity
    {
        [Key]
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
        [JsonProperty("DataSource")]
        public SourceEnum DataSource { get; set; }


        public WeatherEntity(int id, DateTime date, TimeSpan time, int minimumTemperature, int maximumTemperature, int precipitationsProbability, bool atmosphericFenomens, string otherInformation, SourceEnum dataSource)
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

        public WeatherEntity()
        {
        }
    }

}
