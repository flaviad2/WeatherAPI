namespace WebApplication2.WeatherRepository
{
    public class WeatherRepository : IWeatherRepo
    {
        private readonly WeatherContext _weatherContext;
        public WeatherRepository(WeatherContext context)
        {
            _weatherContext = context;
        }

        public WeatherRepository()
        {

        }

        public WeatherContext GetWeatherContext()
        {
            return _weatherContext;
        }

        public async Task<List<WeatherEntity>> GetWeathersBetweenDates(DateTime date1, DateTime date2)
        {
 
            var dbWeathers = _weatherContext.WeatherForecasts.Where(w => w.Date >= date1 && w.Date <= date2).ToList();
            return dbWeathers;
        }

        public async Task<List<WeatherEntity>> GetWeathersFromDay(DateTime date_day)
        {

            var dbWeathers = _weatherContext.WeatherForecasts.Where(w => w.Date == date_day).ToList();
            return dbWeathers;
        }


       

        public async Task<List<WeatherEntity>> GetAll()
        {
            var dbWeathers = _weatherContext.WeatherForecasts.ToList();
            return dbWeathers;
        }


        public async Task<List<WeatherEntity>> GetWeathersNotTooFar(DateTime today)
        {
            var dbWeathers = _weatherContext.WeatherForecasts.Where(w => w.Date.Year == today.Year && w.Date.DayOfYear - today.DayOfYear < 30 && w.Date.DayOfYear - today.DayOfYear < 30).ToList();
            return dbWeathers;


        }


        public Task<WeatherEntity> EditWeatherFromDayFromSource(DateTime date, SourceEnum source, WeatherEntity weather)
        {


            WeatherEntity oldWeather = _weatherContext.WeatherForecasts.Where(w => w.Date == date && w.DataSource == source).FirstOrDefault();
            int id = oldWeather.Id;
            if (oldWeather != null)
            {
                oldWeather.Id = id;
                oldWeather.Id = oldWeather.Id;
                oldWeather.Date = weather.Date;
                oldWeather.Time = weather.Time;
                oldWeather.MinimumTemperature = weather.MinimumTemperature;
                oldWeather.MaximumTemperature = weather.MaximumTemperature;
                oldWeather.AtmosphericFenomens = weather.AtmosphericFenomens;
                oldWeather.PrecipitationsProbability = weather.PrecipitationsProbability;
                oldWeather.DataSource = weather.DataSource;
                oldWeather.OtherInformation = weather.OtherInformation;
                _weatherContext.WeatherForecasts.Update(oldWeather);
                _weatherContext.SaveChanges();

                Task<WeatherEntity> wTask = Task.FromResult(oldWeather);
                return wTask; 

            }
            return null;

        }



        public Task<WeatherEntity> AddWeatherWithDate(DateTime date, WeatherEntity weather)
        {
            if (date >= DateTime.Today)
            {
                _weatherContext.WeatherForecasts.Add(weather);
                _weatherContext.SaveChanges();

                Task<WeatherEntity> wTask = Task.FromResult(weather);
                return wTask;
            }
            else
            {
                throw new Exception("This method only adds weather forecast for future days!");
            }
        }

        public void DeleteWeather(int Id)
        {
            //verifica metoda din Controller daca prognoza exista si aici fac doar stergere dupa id, stiind ca exista
            if (_weatherContext.WeatherForecasts.Find(Id) != null)
            {
                _weatherContext.WeatherForecasts.Remove(_weatherContext?.WeatherForecasts?.Find(Id));
                _weatherContext.SaveChanges();
            }
        }

        public async Task<List<WeatherEntity>> DeleteWeathersFromDay(DateTime day)
        {
            DateTime today = DateTime.Now;
            List<WeatherEntity> forDeletion = new List<WeatherEntity>();

            var x = today - day;
            if (Math.Abs((today - day).TotalDays) <= 30) //atunci se poate sterge
            {
                //daca da, stergem toate prognozele din acea data
                forDeletion = _weatherContext.WeatherForecasts.Where(w => w.Date == day).ToList();
                for (int i = 0; i < forDeletion.Count; i++)
                {
                    DeleteWeather(forDeletion[i].Id);
                    
                }

            }
            return forDeletion;

           
        }


    }


}
