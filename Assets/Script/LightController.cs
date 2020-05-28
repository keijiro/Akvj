using UnityEngine;
using Unity.Mathematics;
using HDAdditionalLightData =
  UnityEngine.Rendering.HighDefinition.HDAdditionalLightData;

namespace Akvj {

sealed class LightController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Light _keyLight = null;
    [SerializeField] Light _fillLight = null;
    [SerializeField] float _dimmerFrequency = 0.03f;
    [SerializeField] AnimationCurve _dimmerCurve = null;
    [SerializeField] float _fillFrequency = 0.05f;

    #endregion

    #region Private member

    ColorMaster _colorMaster;
    HDAdditionalLightData _keyLightData;
    HDAdditionalLightData _fillLightData;
    float _fillIntensity;

    float Noise(float frequency, int seed)
      => noise.snoise(math.float2(frequency * Time.time, 1.17843f * seed));

    float NormalizedNoise(float frequency, int seed)
      => math.saturate((1 + Noise(frequency, seed)) / 2);

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _colorMaster = GetComponent<ColorMaster>();
        _keyLightData = _keyLight.GetComponent<HDAdditionalLightData>();
        _fillLightData = _fillLight.GetComponent<HDAdditionalLightData>();
        _fillIntensity = _fillLight.intensity;
    }

    void Update()
    {
        _keyLightData.volumetricDimmer = _dimmerCurve.Evaluate
          (NormalizedNoise(_dimmerFrequency, 1));

        _fillLight.color = _colorMaster.GetOffsetColor
          (2.0f / 3, NormalizedNoise(_fillFrequency, 2) * 0.6f);

        _fillLight.intensity
          = _fillIntensity * 2 * NormalizedNoise(_fillFrequency, 3);
    }

    #endregion
}

}
