
using Microsoft.AspNetCore.Mvc;

using WebApplication2.Models;
using WebApplication2.WeatherRepository;


namespace WebApplication2.Controllers
{
    [Route("")]
    [ApiController]
    

    public class WeatherController : ControllerBase
    {
        private IWeatherRepository _weatherData;

       
         
        public WeatherController(IWeatherRepository weather)
        {
            _weatherData = weather;
            

        }

        //get all 
        [HttpGet]
        [Route("api/[controller]")]

        public  IActionResult GetWeathers()
        {
            if (_weatherData.getWeathers().Count != 0)
                return Ok(Converter.WeatherToResponseList(_weatherData.getWeathers()));
            else
                return NoContent();

        }



        //get one dupa id
        [HttpGet]
        [Route("api/[controller]/{id}")]
        public IActionResult GetWeather(int id)
        {
            var weather = _weatherData.GetWeather(id);
            if (weather != null)
            {
                return Ok(Converter.weatherToResponseElem(_weatherData.GetWeather(id)));
            }
            return NotFound($"The weather with id : {id} was not found");
        }





        //add
        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult PostWeather(WeatherRequest weather)
        {
            _weatherData.AddWeather(Converter.requestToWeather(weather));
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + weather.Id, weather);

        }



        //update
        [HttpPut]
        [Route("api/[controller]/{id}")]
        public IActionResult EditWeather(int id, WeatherRequest weatherRequest)
        {

            WeatherEntity weather = Converter.requestToWeather(weatherRequest);
            if (_weatherData.EditWeather(id, weather) != null)
                return Ok(Converter.weatherToResponseElem(weather));
            else
                return NotFound($"The weather with id : {id} was not found");

        }




        //delete
        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public IActionResult DeleteWeather(int id)
        {
            var weather = _weatherData.GetWeather(id);
            if (weather != null)
            {
                _weatherData.DeleteWeather(weather.Id);
                return Ok();
            }
            return NotFound($"Weather with Id: {id} was not found");
        }





        //apel cu : 2008-11-11T00:00:00 
        //          2010-11-11T00:00:00
        //returneaza array cu toate prognozele din acest interval de timp 
        //daca nu gaseste returneaza array vid
        [HttpGet]
        [Route("api/[controller]/{date1}/{date2}")]
        public IActionResult GetWeathersBetweenDates(DateTime date1, DateTime date2)
        {
            if (date1 > date2)
                return BadRequest();
            List<WeatherEntity> weatherResult = _weatherData.GetWeathersBetweenDates(date1, date2);
            if (weatherResult.Count != 0)
            {
                return Ok(Converter.WeatherToResponseList(weatherResult));
            }
            return NotFound($"No weather forecasts between dates {date1} and {date2} were found.");


        }





        //apel cu : 2008-11-11T00:00:00 
        //returneaza prognoza/prognozele din aceasta zi, indiferent de sursa
        [HttpGet]
        [Route("api/[controller]/forDay/{date_day}")]

        public IActionResult GetWeathersFromDate(DateTime date_day)
        {
            List<WeatherEntity> weathers = _weatherData.GetWeathersFromDay(date_day);
            if (weathers.Count != 0)
            {
                return Ok(Converter.WeatherToResponseList(weathers));
            }
            return NoContent();

        }



        //apel cu : 2008-11-11T00:00:00 
        //sterge toate prognozele din acea data daca data e valida
        [HttpDelete]
        [Route("api/[controller]/30days/{day}")]
        public IActionResult DeleteWeather30days(DateTime day)
        {
            bool hasBeenDeleted = false;
            DateTime today = DateTime.Now;

            var x = today - day; 
            if (Math.Abs((today-day).TotalDays) >=30) //atunci se poate sterge
            {
                //daca da, stergem toate prognozele din acea data

                List<WeatherEntity> forDeletion = _weatherData.GetWeathersFromDay(day); 
                for(int i=0; i<forDeletion.Count; i++)
                {
                    _weatherData.DeleteWeather(forDeletion[i].Id);
                    hasBeenDeleted = true; 
                }
                if(hasBeenDeleted == true)
                    return Ok();
                else 
                    return NotFound($"No weather forecast has been deleted because there aren't weather forecasts from this date. ");


            }
           return BadRequest($"Wrong calendar date! ");


        }
       



        //update
        //editeaza prognoza pentru ziua data si sursa data
        //obiectul editat il pun in header
        //data ca parametru si nu trebuie pusa neaparat in obiect 

        //nu se pune id, data, source in json dat ca req
        [HttpPut]
        [Route("api/[controller]/for_day/{date}/{source}")]
        public IActionResult EditWeather(DateTime date, SourceEnum source, WeatherRequest weatherRequest)
        {
            WeatherEntity weather = Converter.requestToWeather(weatherRequest); 
            weather.DataSource = source;
            weather.Date = date;
            var result = _weatherData.EditWeatherFromDayFromSource(date, source, weather);
            if (result != null)
                return Ok(Converter.weatherToResponseElem(result));
            else return NotFound($"Weather for day {date} and source {source} was not found");


        }



        //add
        //adauga o prognoza meteo noua cu datele date in header, dar data se da separat, fiind prognoza pentru ziua respectiva

        /*
         * apel cu header fara data si data o pun doar la param de intare la URI
         * date: 2022-11-11T00:00:00
    
    {"time": "14:00:11",
    "minimumTemperature": -16,
    "maximumTemperature": 30,
    "precipitationsProbability": 80,
    "atmosphericFenomens": false,
    "otherInformation": "Informatii suplimentare",
    "dataSource": 3 }    


   */

        [HttpPost]
        [Route("api/[controller]/{date}")]

        public IActionResult PostWeatherWithDate(DateTime date, WeatherEntity weather)
        {
            try
            {
                weather.Date = date;
                _weatherData.AddWeatherWithDate(weather.Date, weather);
                return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + weather.Id, weather);
            }
            catch (Exception )
            {
                return BadRequest($"No weather forecast has been added because the date is invalid. Please enter a valid date for the forecast! ");
            }
        }
    }
}
