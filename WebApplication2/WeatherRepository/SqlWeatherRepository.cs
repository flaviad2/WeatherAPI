﻿using WebApplication2.Models;


namespace WebApplication2.WeatherRepository
{
    public class SqlWeatherData : IWeatherRepository
    {
        private WeatherContext _weatherContext; 
        public SqlWeatherData(WeatherContext context)
        {
            _weatherContext = context;
        }
        public WeatherEntity AddWeather(WeatherEntity weather)
        {
           _weatherContext.WeatherForecasts.Add(weather);
            _weatherContext.SaveChanges();
            return weather;
        }

      
        public void DeleteWeather(int Id)
        {
            //verifica metoda din Controller daca prognoza exista si aici fac doar stergere dupa id, stiind ca exista
            _weatherContext.WeatherForecasts.Remove(_weatherContext.WeatherForecasts.Find(Id));
            _weatherContext.SaveChanges();
        }
        

        public WeatherEntity EditWeather(int Id, WeatherEntity weather)
        {
            WeatherEntity oldWeather = _weatherContext.WeatherForecasts.Where(w=>w.Id==weather.Id).FirstOrDefault();
            if (oldWeather != null)
            {
                oldWeather.Date = weather.Date;
                oldWeather.Time = weather.Time;
                oldWeather.MinimumTemperature = weather.MinimumTemperature;
                oldWeather.MaximumTemperature = weather.MaximumTemperature;
                oldWeather.AtmosphericFenomens = weather.AtmosphericFenomens;
                oldWeather.PrecipitationsProbability = weather.PrecipitationsProbability;
                oldWeather.DataSource = weather.DataSource;
                _weatherContext.WeatherForecasts.Update(oldWeather);
                _weatherContext.SaveChanges();
                return oldWeather;

            }
            return null; //adica nu s-au facut modificari 

        }
    
        public WeatherEntity GetWeather(int id)
        {

            var weather = _weatherContext.WeatherForecasts.Find(id);
            return weather; 
        }


        public List<WeatherEntity> getWeathers()
        {
           return  _weatherContext.WeatherForecasts.ToList();

            
        }
        public List<WeatherEntity> GetWeathersBetweenDates(DateTime date1, DateTime date2)
        {
            return _weatherContext.WeatherForecasts.Where(w => w.Date >= date1 && w.Date <= date2).ToList();
        }
 
        public List<WeatherEntity> GetWeathersFromDay(DateTime date_day)
        {
            return _weatherContext.WeatherForecasts.Where(w => w.Date == date_day).ToList(); 
        }
      
        
        public List<WeatherEntity> GetWeathersNotTooFar(DateTime today)
        {
            return _weatherContext.WeatherForecasts.Where(w => w.Date.Year == today.Year && w.Date.DayOfYear - today.DayOfYear < 30 && w.Date.DayOfYear - today.DayOfYear < 30).ToList();

        }

        public WeatherEntity AddWeatherWithDate(DateTime date, WeatherEntity weather)
        {
            if (date>= DateTime.Today )
            {
                _weatherContext.WeatherForecasts.Add(weather);
                _weatherContext.SaveChanges();
                return weather;
            }
            else
            {
                throw new Exception("This method only adds weather forecast for future days!"); 
            }
        }

        public WeatherEntity EditWeatherFromDayFromSource(DateTime date, SourceEnum source, WeatherEntity weather)
        {


            WeatherEntity oldWeather = _weatherContext.WeatherForecasts.Where(w => w.Date == date && w.DataSource == source).FirstOrDefault(); 
          if(oldWeather != null)
            {
                oldWeather.Date = weather.Date;
                oldWeather.Time = weather.Time;
                oldWeather.MinimumTemperature = weather.MinimumTemperature;
                oldWeather.MaximumTemperature = weather.MaximumTemperature;
                oldWeather.AtmosphericFenomens = weather.AtmosphericFenomens;
                oldWeather.PrecipitationsProbability = weather.PrecipitationsProbability;
                oldWeather.DataSource = weather.DataSource; 
                _weatherContext.WeatherForecasts.Update(oldWeather);
                _weatherContext.SaveChanges();
                return oldWeather;

            }
            return null;

        }
    }
}
