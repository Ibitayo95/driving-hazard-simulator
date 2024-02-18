using DistantLands.Cozy;
using DistantLands.Cozy.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaimiraGames;
using System.Linq;

public class HazardWeatherScript : MonoBehaviour
{
    public WeatherProfile[] WeatherProfiles;
    private readonly WeatherType[] _weatherTypes = new WeatherType[] { WeatherType.CLEAR, WeatherType.MIXEDCLOUD, 
                                                                       WeatherType.LIGHTRAIN, WeatherType.HEAVYRAIN, 
                                                                       WeatherType.THUNDERSTORM, WeatherType.FOG, WeatherType.SNOW }; // the order of this array matters: see order of WeatherProfiles array
    private WeightedList<WeatherType> _weatherProbabilityArray;
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

        List<WeightedListItem<WeatherType>> items = new()
        {
            new WeightedListItem<WeatherType>(WeatherType.CLEAR, 40), // this means clear weather has a 40% chance of occuring (assuming all weights sum to 100)
            new WeightedListItem<WeatherType>(WeatherType.MIXEDCLOUD, 20),
            new WeightedListItem<WeatherType>(WeatherType.LIGHTRAIN, 15),
            new WeightedListItem<WeatherType>(WeatherType.HEAVYRAIN, 10),
            new WeightedListItem<WeatherType>(WeatherType.THUNDERSTORM, 5),
            new WeightedListItem<WeatherType>(WeatherType.FOG, 5),
            new WeightedListItem<WeatherType>(WeatherType.SNOW, 5),
        };

        _weatherProbabilityArray = new(items);

        
    }

    void Start()
    {
        _globalWeather = GetComponent<CozyWeather>();

        // set a random weather type
        WeatherType randomWeather = _weatherProbabilityArray.Next();
        
        foreach (CozyEcosystem i in _globalWeather.ecosystems)
        {
            i.SetWeather(_weatherMap[randomWeather]);
            weatherSet  = true;
            currentWeatherType = randomWeather;
            currentWeatherProfile = _weatherMap[randomWeather];
        }

        _globalWeather.currentTicks = Random.Range(600, 1700); // set random time of day between 6am and 5pm



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

        // if global weather pattern changes then update the script to match
        if (_globalWeather.currentWeather != currentWeatherProfile)
        {
            currentWeatherProfile = _globalWeather.currentWeather;
            currentWeatherType = _weatherMap.FirstOrDefault(x => x.Value == currentWeatherProfile).Key;
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
