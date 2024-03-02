using DistantLands.Cozy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VehicleBehaviour;

[ExecuteInEditMode]
public class DayCycleLighting : MonoBehaviour
{
    public Material LampMaterial;
    public CozyWeather Weather;
    private bool _lightsOn;
    private bool _isDark;
    public List<Light> _lights;
   
    
    void Awake()
    {
        LampMaterial.DisableKeyword("_EMISSION");
        _lightsOn = false;
        
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
