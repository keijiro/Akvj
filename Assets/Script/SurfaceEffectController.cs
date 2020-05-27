using UnityEngine;
using UnityEngine.VFX;

namespace Akvj {

sealed class SurfaceEffectController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Akvfx.DeviceController _device = null;
    [SerializeField] VisualEffect _masterVfx = null;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _block = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (_masterVfx.enabled)
        {
            var fader = _masterVfx.GetFloat(IDs.Fader);
            var keyColor = _masterVfx.GetVector4(IDs.KeyColor);
            var altColor = _masterVfx.GetVector4(IDs.AltColor);

            _block.SetFloat(IDs._Fader, fader);
            _block.SetColor(IDs._KeyColor, keyColor);
            _block.SetColor(IDs._AltColor, altColor);

            _block.SetTexture(IDs._ColorMap, _device.ColorMap);
            _block.SetTexture(IDs._PositionMap, _device.PositionMap);

            _renderer.enabled = true;
            _renderer.SetPropertyBlock(_block);
        }
        else
        {
            _renderer.enabled = false;
        }
    }

    #endregion

    #region Private members

    static class IDs
    {
        public static int Fader = Shader.PropertyToID("Fader");
        public static int KeyColor = Shader.PropertyToID("KeyColor");
        public static int AltColor = Shader.PropertyToID("AltColor");

        public static int _Fader = Shader.PropertyToID("_Fader");
        public static int _KeyColor = Shader.PropertyToID("_KeyColor");
        public static int _AltColor = Shader.PropertyToID("_AltColor");

        public static int _ColorMap = Shader.PropertyToID("_ColorMap");
        public static int _PositionMap = Shader.PropertyToID("_PositionMap");
    }

    Renderer _renderer;
    MaterialPropertyBlock _block;

    #endregion
}

}
