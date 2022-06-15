using UnityEngine;

//This shader used for controll material property block for some effects

[RequireComponent(typeof(Renderer))]
public class Colorize : MonoBehaviour
{
    [Range(-360.0f, 360.0f)]
    public float Hue = 0;
    [Space]
    [Range(-1.0f, 1.0f)]
    public float Brightness = 0;
    [Space]
    [Range(0.0f, 2.0f)]
    public float Contrast = 1;
    [Space]
    [Range(0.0f, 2.0f)]
    public float Saturation = 1;

    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    private static readonly int _Hue = Shader.PropertyToID("_Hue");
    private static readonly int _Brightness = Shader.PropertyToID("_Brightness");
    private static readonly int _Contrast = Shader.PropertyToID("_Contrast");
    private static readonly int _Saturation = Shader.PropertyToID("_Saturation");

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        _renderer.GetPropertyBlock(_propertyBlock);

        _propertyBlock.SetFloat(_Hue, Hue);
        _propertyBlock.SetFloat(_Brightness, Brightness);
        _propertyBlock.SetFloat(_Contrast, Contrast);
        _propertyBlock.SetFloat(_Saturation, Saturation);
            
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}