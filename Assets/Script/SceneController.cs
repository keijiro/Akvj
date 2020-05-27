using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Mathematics;
using Guid = System.Guid;
using IEnumerator = System.Collections.IEnumerator;
using Random = Unity.Mathematics.Random;

namespace Akvj {

sealed class SceneController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] VisualEffect[] _vfxGroup1 = null;
    [SerializeField] VisualEffect[] _vfxGroup2 = null;
    [SerializeField] float _fadingDuration = 3;
    [SerializeField] float _vfxDurationMin = 1;
    [SerializeField] float _vfxDurationMax = 10;
    [SerializeField] float _hueDuration = 10;
    [SerializeField] uint _randomSeed = 0;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
    #if !UNITY_EDITOR
        Cursor.visible = false;
    #endif

        PrepareRandom();

        StartCoroutine(VfxCoroutine(_vfxGroup1));
        StartCoroutine(VfxCoroutine(_vfxGroup2));
        StartCoroutine(ColorCoroutine());
    }

    #endregion

    #region Controller coroutines

    IEnumerator VfxCoroutine(VisualEffect[] vfxGroup)
    {
        while (true)
            foreach (var vfx in vfxGroup.OrderBy(e => _random.NextUInt()))
                yield return VfxSingleCoroutine(vfx);
    }

    IEnumerator VfxSingleCoroutine(VisualEffect vfx)
    {
        vfx.enabled = true;

        for (var fade = 0.0f; fade < 1; fade += FaderDelta)
        {
            vfx.SetFloat(IDs.Fader, fade);
            yield return null;
        }

        vfx.SetFloat(IDs.Fader, 1);

        yield return new WaitForSeconds(RandomVfxDuration);

        for (var fade = 1.0f; fade > 0; fade -= FaderDelta)
        {
            vfx.SetFloat(IDs.Fader, fade);
            yield return null;
        }

        vfx.SetFloat(IDs.Fader, 0);

        yield return new WaitForSeconds(4);

        vfx.enabled = false;

        yield return new WaitForSeconds(RandomVfxDuration);
    }

    IEnumerator ColorCoroutine()
    {
        var hue = 0.0f;

        while (true)
        {
            hue += Time.deltaTime / _hueDuration;

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
        public static int Fader = Shader.PropertyToID("Fader");
        public static int KeyColor = Shader.PropertyToID("KeyColor");
        public static int AltColor = Shader.PropertyToID("AltColor");
    }

    Random _random;

    bool IsVfxAbsent
      => _vfxGroup1.All(vfx => !vfx.enabled) &&
         _vfxGroup2.All(vfx => !vfx.enabled);

    float FaderDelta
      => Time.deltaTime / _fadingDuration;

    float RandomVfxDuration
      => _random.NextFloat(_vfxDurationMin, _vfxDurationMax);

    Color HueToRgb(float hue)
      => Color.HSVToRGB(hue % 1, 1, 1);

    void PrepareRandom()
    {
        _random = new Random(_randomSeed);
        _random.NextUInt();
        _random.NextUInt();
        _random.NextUInt();
    }

    void SetKeyColors(VisualEffect vfx, Color keyColor, Color altColor)
    {
        if (!vfx.enabled) return;

        vfx.SetVector4(IDs.KeyColor, keyColor);

        if (vfx.HasVector4(IDs.AltColor))
            vfx.SetVector4(IDs.AltColor, altColor);
    }

    #endregion
}

}
