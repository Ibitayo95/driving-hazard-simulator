using DistantLands.Cozy;
using DistantLands.Cozy.Data;
using System.Collections.Generic;
using UnityEngine;
using KaimiraGames;

public class HazardWeatherScript : MonoBehaviour
{
    public WeatherProfile[] WeatherProfiles;
    private readonly WeatherType[] _weatherTypes = new WeatherType[] { WeatherType.CLEAR, WeatherType.MIXEDCLOUD, 
                                                                       WeatherType.LIGHTRAIN, WeatherType.HEAVYRAIN, 
                                                                       WeatherType.THUNDERSTORM, WeatherType.FOG, WeatherType.SNOW }; // the order of this array matters: see order of WeatherProfiles array
    private WeightedList<WeatherType> _weatherProbability;
    private WeightedList<int> _timeOfDayProbability;
    private readonly Dictionary<WeatherType, WeatherProfile> _weatherMap = new();
    private bool weatherSet = false;
    private bool fogCheck = false;
    private CozyWeather _globalWeather;

    public WeatherType currentWeatherType;
    public WeatherProfile currentWeatherProfile;


    private void Awake()
    {
        // populate the weather map
        for (int i = 0; i < WeatherProfiles.Length; i++)
        {
            _weatherMap[_weatherTypes[i]] = WeatherProfiles[i]; 
        }

        // set up time and weather weighted lists
        List<WeightedListItem<WeatherType>> weatherItems = new()
        {
            new WeightedListItem<WeatherType>(WeatherType.CLEAR, 40), // this means clear weather has a 40% chance of occuring (assuming all weights sum to 100)
            new WeightedListItem<WeatherType>(WeatherType.MIXEDCLOUD, 20),
            new WeightedListItem<WeatherType>(WeatherType.LIGHTRAIN, 15),
            new WeightedListItem<WeatherType>(WeatherType.HEAVYRAIN, 10),
            new WeightedListItem<WeatherType>(WeatherType.THUNDERSTORM, 5),
            new WeightedListItem<WeatherType>(WeatherType.FOG, 5),
            new WeightedListItem<WeatherType>(WeatherType.SNOW, 5),
        };

        List<WeightedListItem<int>> timeItems = new()
        {
            new WeightedListItem<int>(900, 25), // this means 9am has a 25% chance of being the starting time (assuming all weights sum to 100)
            new WeightedListItem<int>(1300, 30),
            new WeightedListItem<int>(1700, 15),
            new WeightedListItem<int>(2100, 10),
            new WeightedListItem<int>(100, 10),
            new WeightedListItem<int>(500, 10),
        };

        _weatherProbability = new(weatherItems);
        _timeOfDayProbability = new(timeItems);

        
    }

    void Start()
    {
        _globalWeather = GetComponent<CozyWeather>();

        // set a random weather type
        WeatherType randomWeather = _weatherProbability.Next();
        
        foreach (CozyEcosystem i in _globalWeather.ecosystems)
        {
            i.SetWeather(_weatherMap[randomWeather]);
            weatherSet  = true;
            currentWeatherType = randomWeather;
            currentWeatherProfile = _weatherMap[randomWeather];
        }

        // set a random time of day
        _globalWeather.currentTicks = _timeOfDayProbability.Next();
    }

    void Update()
    {

        if (weatherSet && !fogCheck)
        {
            var fog = GameObject.FindWithTag("Fog");
            // alter fog density if needed
            if (currentWeatherType == WeatherType.FOG)
            {
                fog.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                fog.GetComponent<Renderer>().enabled = false;
            }

            fogCheck = true;
        }

        // changing the type in the editor can force weather change (for debugging)
        if (_globalWeather.currentWeather != _weatherMap[currentWeatherType])
        {
            foreach (CozyEcosystem i in _globalWeather.ecosystems)
            {
                i.SetWeather(_weatherMap[currentWeatherType]);
                currentWeatherProfile = _weatherMap[currentWeatherType];
            }

            fogCheck = false;
        }
    }
}

public enum WeatherType
{
    CLEAR,
    MIXEDCLOUD,
    LIGHTRAIN,
    HEAVYRAIN,
    THUNDERSTORM,
    FOG,
    SNOW
}
