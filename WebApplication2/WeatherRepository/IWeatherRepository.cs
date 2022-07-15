using WebApplication2.Models;

namespace WebApplication2.WeatherRepository
{
    public interface IWeatherRepository
    {
        /*toate prognozele*/
        List<WeatherEntity> getWeathers();

        /*prognoza cu un anumit id  */
        WeatherEntity GetWeather(int Id);

        /*adauga o prognoza data ca parametru*/
        WeatherEntity AddWeather(WeatherEntity weather);

        /*sterge o prognoza dupa id*/
        void DeleteWeather(int Id);

        /*editeaza o prognoza, avand obiectul nou modificat si id-ul obiectului de editat*/
        WeatherEntity EditWeather(int Id, WeatherEntity weather);

        /*toate prognozele dintr-un anumit interval de timp */
        List<WeatherEntity> GetWeathersBetweenDates(DateTime date1, DateTime date2);

        /*toate prognozele dintr-o anumita zi*/
        List<WeatherEntity> GetWeathersFromDay(DateTime date_day);

        /*sterge toate prognozele mai vechi de 30 de zile sau de dupa 30 de zile de la ziua curenta*/
        List<WeatherEntity> GetWeathersNotTooFar(DateTime today);

        /* editeaza o prognoza, avand obiectul nou modificat si data si sursa pentru prognoza respectiva*/
        WeatherEntity EditWeatherFromDayFromSource(DateTime date, SourceEnum source, WeatherEntity weather);


        /* adauga o prognoza pentru o anumita zi urmatoare si arunca eroare daca ziua este invalida*/
        WeatherEntity AddWeatherWithDate(DateTime date, WeatherEntity weathe);



    }
}
