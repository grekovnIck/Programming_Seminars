using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace KeggleTaskLinq
{
    class Program
    {
        static void Main(string[] args)
        {

                //Нужно дополнить модель WeatherEvent, создать список этого типа List<>
                //И заполнить его, читая файл с данными построчно через StreamReader
                //Ссылка на файл https://www.kaggle.com/sobhanmoosavi/us-weather-events

                //Написать Linq-запросы, используя синтаксис методов расширений
                //и продублировать его, используя синтаксис запросов
                //(возможно с вкраплениями методов расширений, ибо иногда первого может быть недостаточно)

                //0. Linq - сколько различных городов есть в датасете.
                //1. Сколько записей за каждый из годов имеется в датасете.
                //Потом будут еще запросы
                List<WeatherEvent> weatherEvents = new List<WeatherEvent>();


            using (StreamReader f = new StreamReader("WeatherEvents_Jan2016-Dec2020.csv"))
            {
                string field = f.ReadLine();
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((field = f.ReadLine()) != null)
                {
                    var data = field.Split(',').ToList();
                    weatherEvents.Add(new WeatherEvent()
                    {
                        EventId = data[0],
                        Type = WeatherEvent.GetWeatherEventType(data[1]),
                        Severity = WeatherEvent.GetSeverity(data[2]),
                        StartTime = DateTime.Parse(data[3]),
                        EndTime = DateTime.Parse(data[4]),
                        TimeZone = WeatherEvent.GetTimeZone(data[5]),
                        AirportCode = data[6],
                        LocationLat = double.Parse(data[7], new CultureInfo("en-US")),
                        LocationLng = double.Parse(data[8], new CultureInfo("en-US")),
                        City = data[9],
                        Country = data[10],
                        ZipCode = data[11]
                    });
                }

            }
            
            Console.WriteLine($"Всего различных городов: {weatherEvents.Select(x => x.City).Distinct().Count()}");
            List<DateTime> date = new List<DateTime>() { Convert.ToDateTime("05/01/2016"), Convert.ToDateTime("05/01/2017"), Convert.ToDateTime("05/01/2018"), Convert.ToDateTime("05/01/2019"), Convert.ToDateTime("05/01/2020") };
            date.ForEach(x =>
               Console.WriteLine($"{x} - {weatherEvents.Where(y => y.StartTime.Year == x.Year).Count()}")
           );
            /*-1.Вывести количество зафиксированных природных явлений в Америке в 2018 году
            
            0.Вывести количество штатов, количество городов в датасете  
            1.Вывести топ 3 самых дождливых города в 2019 году в порядке убывания количества дождей(вывести город и количество дождей)
            2.Вывести данные самых долгих(топ - 1) снегопадов в Америке по годам(за каждый из годов) - с какого времени, по какое время, в каком городе
            */
            var s = from i in weatherEvents
                    where i.Severity != Severity.Unknown
                    select i;
            Console.WriteLine($"Кол-во природных явлений {weatherEvents.Where(x => x.StartTime.Year == 2018 && x.Type!=WeatherEventType.Unknown).Select(x=>x.Type).Distinct().Count()}");

            Console.WriteLine($"Кол-во штатов: {weatherEvents.Select(x => x.ZipCode).Distinct().Count()} городов: {weatherEvents.Select(x => x.City).Distinct().Count()}");

            var cities = weatherEvents.Where(x => (x.StartTime.Year == 2019) && (x.Type == WeatherEventType.Rain)).Select(x => x.City).ToList();
            cities.Distinct()
                .Take(3).ToList()
                .ForEach(x => Console.WriteLine($"{x.ToString()} - {cities.Where(y => y == x).Count()}"));

        }

        //Дополнить модеь, согласно данным из файла
        class WeatherEvent
        {
            public string EventId { get; set; }
            public WeatherEventType Type { get; set; }
            public Severity Severity { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public TimeZone TimeZone { get; set; }
            public string AirportCode { get; set; }
            public double LocationLat { get; set; }
            public double LocationLng { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string ZipCode { get; set; }

            public static WeatherEventType GetWeatherEventType(string watherEventType)
            {
                return watherEventType switch
                {
                    "Snow" => WeatherEventType.Snow,
                    "Rain" => WeatherEventType.Rain,
                    "Fog" => WeatherEventType.Fog,
                    "Cold" => WeatherEventType.Cold,
                    _ => WeatherEventType.Unknown,
                };
            }

           
            public static Severity GetSeverity(string severity)
            {
                return severity switch
                {
                    "Light" => Severity.Light,
                    "Severe" => Severity.Severe,
                    "Moderate" => Severity.Moderate,
                    _ => Severity.Unknown,
                };
            }
            
            public static TimeZone GetTimeZone(string timeZone)
            {
                return timeZone switch
                {
                    "US/Mountain" => TimeZone.Mountain,
                    "US/Central" => TimeZone.Central,
                    "US/Eastern" => TimeZone.Eastern,
                    "US/Pacific" => TimeZone.Pacific,
                };
            }
        }

        //Дополнить перечисления
        enum Severity
        {
            Unknown,
            Light,
            Severe,
            Moderate
        }

        enum TimeZone
        {
            Eastern,
            Central,
            Mountain,
            Pacific,
        }
        enum WeatherEventType
        {
            Unknown,
            Snow,
            Fog,
            Rain,
            Cold,
        }

    }
}

