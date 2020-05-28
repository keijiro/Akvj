using UnityEngine;

namespace Akvj {

sealed class ColorMaster : MonoBehaviour
{
    #region Editable attribute

    [SerializeField] float _hueDuration = 10;

    #endregion

    #region Public properties

    public Color KeyColor
      => Color.HSVToRGB(_hue % 1, 1, 1);

    public Color GetOffsetColor(float offset)
      => Color.HSVToRGB((_hue + offset) % 1, 1, 1);

    public Color GetOffsetColor(float offset, float saturation)
      => Color.HSVToRGB((_hue + offset) % 1, saturation, 1);

    #endregion

    #region Private member

    float _hue;

    #endregion

    #region MonoBehaviour implementation

    void Update()
      => _hue += Time.deltaTime / _hueDuration;

    #endregion
}

}
