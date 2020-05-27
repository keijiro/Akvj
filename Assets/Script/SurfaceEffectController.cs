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

            _block.SetFloat(IDs._Fader, fader);
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
        public static int _Fader = Shader.PropertyToID("_Fader");
        public static int _ColorMap = Shader.PropertyToID("_ColorMap");
        public static int _PositionMap = Shader.PropertyToID("_PositionMap");
    }

    Renderer _renderer;
    MaterialPropertyBlock _block;

    #endregion
}

}
