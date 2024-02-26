using DistantLands.Cozy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayCycleLighting : MonoBehaviour
{
    public Material LampMaterial;
    public CozyWeather Weather;
    private bool _lightsOn;
    private bool _isDark;
    private Light[] _lights;
    
    void Awake()
    {
        LampMaterial.DisableKeyword("_EMISSION");
        _lightsOn = false;
        _lights = GetComponentsInChildren<Light>();
        if (_lights[0].enabled)
        {
            foreach (Light light in _lights)
            {
                light.enabled = false;
            }
        }
        
    }

   
    void Update()
    {
        _isDark = Weather.currentTicks <= 600 || Weather.currentTicks > 1700; // if before 6am or after 5pm, visibility will be reduced (is dark)

        if (!_lightsOn && _isDark)
        {
            LampMaterial.EnableKeyword("_EMISSION");
            foreach (Light light in _lights)
            {
                light.enabled = true;
            }
            _lightsOn = true;
        }

        if (_lightsOn && !_isDark)
        {
            LampMaterial.DisableKeyword("_EMISSION");
            foreach (Light light in _lights)
            {
                light.enabled = false;
            }
            _lightsOn = false;
        }
    }
}
