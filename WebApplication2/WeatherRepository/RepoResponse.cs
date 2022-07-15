
namespace WebApplication2.WeatherRepository
{
    public class RepoResponse<T>
    {
        public T info { get; set; }

        public bool Succes { get; set; } = true;


        public string Message { get; set; } = null; 

    }
}
