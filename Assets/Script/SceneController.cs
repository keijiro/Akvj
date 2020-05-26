using UnityEngine;
using UnityEngine.VFX;
using IEnumerator = System.Collections.IEnumerator;

class SceneController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] VisualEffect[] _vfxGroup1 = null;
    [SerializeField] VisualEffect[] _vfxGroup2 = null;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        StartCoroutine(VfxCoroutine());
        StartCoroutine(ColorCoroutine());
    }

    #endregion

    #region Controller coroutines

    IEnumerator VfxCoroutine()
    {
        while (true)
        {
            var i1 = Random.Range(0, _vfxGroup1.Length);
            var i2 = Random.Range(0, _vfxGroup2.Length);

            _vfxGroup1[i1].enabled = true;
            _vfxGroup2[i2].enabled = true;

            yield return new WaitForSeconds(60);

            _vfxGroup1[i1].enabled = false;
            _vfxGroup2[i2].enabled = false;
        }
    }

    IEnumerator ColorCoroutine()
    {
        while (true)
        {
            var hue = Time.time / 120;
            var keyColor = HueToRgb(hue);
            var altColor = HueToRgb(hue + 1.0f / 3);

            foreach (var vfx in _vfxGroup1)
                SetKeyColors(vfx, keyColor, altColor);

            foreach (var vfx in _vfxGroup2)
                SetKeyColors(vfx, keyColor, altColor);

            yield return null;
        }
    }

    #endregion

    #region Private members

    static class IDs
    {
        public static int KeyColor = Shader.PropertyToID("KeyColor");
        public static int AltColor = Shader.PropertyToID("AltColor");
    }

    Color HueToRgb(float hue)
      => Color.HSVToRGB(hue % 1, 1, 1);

    void SetKeyColors(VisualEffect vfx, Color keyColor, Color altColor)
    {
        if (!vfx.enabled) return;

        vfx.SetVector4(IDs.KeyColor, keyColor);

        if (vfx.HasVector4(IDs.AltColor))
            vfx.SetVector4(IDs.AltColor, altColor);
    }

    #endregion
}
