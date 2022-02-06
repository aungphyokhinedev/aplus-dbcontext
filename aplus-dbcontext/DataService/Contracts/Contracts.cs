
namespace DataService;
using AplusDbContext;
public interface GetWeatherForecasts
{
}

public interface WeatherForecasts
{
    WeatherForecast[] Forecasts {  get; }
}

public interface GetList{
    ListRequest request {get;}
    
}

public interface ListData{
    ListResponse response {get;}
}

public interface AddData{
    CreateRequest request {get;}
    
}

public interface ResultData{
    DataResponse response {get;}
}