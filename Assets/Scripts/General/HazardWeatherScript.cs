using DistantLands.Cozy;
using DistantLands.Cozy.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardWeatherScript : MonoBehaviour
{
    public WeatherProfile[] WeatherProfiles;
    private WeatherType[] WeatherTypes = new WeatherType[] { WeatherType.CLEAR, WeatherType.MIXEDCLOUD, WeatherType.LIGHTRAIN, WeatherType.HEAVYRAIN, WeatherType.THUNDERSTORM, WeatherType.FOG, WeatherType.SNOW };
    private List<WeatherType> weatherTypeProbabilities = new();
    private Dictionary<WeatherType, WeatherProfile> _weatherMap = new();
    private bool weatherSet = false;
    private bool fogCheck = false;
    public WeatherType currentWeatherType;

    private void Awake()
    {
        // set up weathermapping
        for (int i = 0; i < WeatherProfiles.Length; i++)
        {
            _weatherMap[WeatherTypes[i]] = WeatherProfiles[i]; 
        }

        // populate random weather picker array: number from enum represents chance of being selected out of 20
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            int count = 0;
            while (count < ((int) WeatherTypes[i]))
            {
                weatherTypeProbabilities.Add(WeatherTypes[i]);
                count++;
            }
        }
       
    }

    void Start()
    {
        // set a random weather type
        int random = Random.Range(0, weatherTypeProbabilities.Count);
        var weather = GetComponent<CozyWeather>();
        
        WeatherType randomWeather = weatherTypeProbabilities[random];
        

        foreach (CozyEcosystem i in weather.ecosystems)
        {
            i.SetWeather(_weatherMap[randomWeather]);
            weatherSet  = true;
            currentWeatherType = randomWeather;
        }



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
    }
}

public enum WeatherType
{
    // each number represents the likelyhood of the weather type being chosen at the start
    // note that one cannot assign the same number to different weathers or unexpected errors arise
    CLEAR = 7,
    MIXEDCLOUD = 6,
    LIGHTRAIN = 5,
    HEAVYRAIN = 4,
    THUNDERSTORM = 1,
    FOG = 2,
    SNOW = 3
}
