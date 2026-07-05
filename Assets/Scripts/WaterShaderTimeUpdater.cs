using UnityEngine;

public class WaterShaderTimeUpdater : MonoBehaviour
{
    public Renderer waterRenderer; // Assign your water's Renderer here
    public string unscaledTimePropertyName = "_UnscaledTime"; // Must match the property name in your Shader Graph

    private MaterialPropertyBlock _propertyBlock;

    void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (waterRenderer != null)
        {
            waterRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(unscaledTimePropertyName, Time.unscaledTime);
            waterRenderer.SetPropertyBlock(_propertyBlock);
        }
        else
        {
            Debug.LogError("Water Renderer not assigned in WaterShaderTimeUpdater script on " + gameObject.name);
        }
    }
}