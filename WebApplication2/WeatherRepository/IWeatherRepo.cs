namespace WebApplication2.WeatherRepository
{
    public interface IWeatherRepo
    {

        /*toate prognozele*/
        Task<List<WeatherEntity>> GetAll();



        /*toate prognozele dintr-un anumit interval de timp */
        Task<List<WeatherEntity>> GetWeathersBetweenDates(DateTime date1, DateTime date2);



        /*toate prognozele dintr-o anumita zi*/
        Task<List<WeatherEntity>> GetWeathersFromDay(DateTime date_day);



        /*sterge toate prognozele mai vechi de 30 de zile sau de dupa 30 de zile de la ziua curenta*/
        Task<List<WeatherEntity>> GetWeathersNotTooFar(DateTime today);


        /* Sterge toate prognozele dintr-o zi, daca ziua este o zi indepartata cu cel mult 30 de zile */
        Task<List<WeatherEntity>> DeleteWeathersFromDay(DateTime day);



        /* editeaza o prognoza, avand obiectul nou modificat si data si sursa pentru prognoza respectiva*/
        Task<WeatherEntity> EditWeatherFromDayFromSource(DateTime date, SourceEnum source, WeatherEntity weather);


        /* adauga o prognoza pentru o anumita zi urmatoare si arunca eroare daca ziua este invalida*/
        Task<WeatherEntity> AddWeatherWithDate(DateTime date, WeatherEntity weather);


        /* sterge o prognoza dupa id */ 
        void DeleteWeather(int Id);





    }
}
