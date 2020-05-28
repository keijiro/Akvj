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
    [SerializeField] float _dimmerFrequency = 1;
    [SerializeField] AnimationCurve _dimmerCurve = null;

    #endregion

    #region Private member

    ColorMaster _colorMaster;
    HDAdditionalLightData _keyLightData;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _colorMaster = GetComponent<ColorMaster>();
        _keyLightData = _keyLight.GetComponent<HDAdditionalLightData>();
    }

    void Update()
    {
        var n = noise.snoise(math.float2(_dimmerFrequency * Time.time, 1.3f));
        _keyLightData.volumetricDimmer = _dimmerCurve.Evaluate((1 + n) / 2);
        _fillLight.color = _colorMaster.GetOffsetColor(2.0f / 3, 0.4f);
    }

    #endregion
}

}
