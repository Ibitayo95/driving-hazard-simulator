using UnityEngine;


[ExecuteInEditMode]
public class EditorLightSwitch : MonoBehaviour
{
    private bool _lightOn;
    public Material LampMaterial;

    [ContextMenu("LightSwitch")]
    public void LightSwitch()
    {
        Light[] lights = GetComponentsInChildren<Light>();

        _lightOn = lights[0].enabled ? true : false;

        if (_lightOn)
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
                LampMaterial.DisableKeyword("_EMISSION");
            }
        } 
        else
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
                LampMaterial.EnableKeyword("_EMISSION");
            }
        }
        
    }
}
