using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Mathematics;
using IEnumerator = System.Collections.IEnumerator;
using Random = Unity.Mathematics.Random;

namespace Akvj {

sealed class VfxController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] VisualEffect[] _vfxGroup1 = null;
    [SerializeField] VisualEffect[] _vfxGroup2 = null;
    [SerializeField] float _fadingDuration = 3;
    [SerializeField] float _vfxDurationMin = 1;
    [SerializeField] float _vfxDurationMax = 10;
    [SerializeField] uint _randomSeed = 0;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        PrepareRandom();
        _colorMaster = GetComponent<ColorMaster>();
        StartCoroutine(VfxCoroutine(_vfxGroup1));
        StartCoroutine(VfxCoroutine(_vfxGroup2));
    }

    void Update()
    {
        var keyColor = _colorMaster.KeyColor;
        var altColor = _colorMaster.GetOffsetColor(1.0f / 3);
        foreach (var vfx in _vfxGroup1) SetVfxColors(vfx, keyColor, altColor);
        foreach (var vfx in _vfxGroup2) SetVfxColors(vfx, keyColor, altColor);
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

        yield return new WaitForSeconds(5);

        vfx.enabled = false;

        yield return null; // One frame delay
        yield return WaitWhileAnyActive(RandomVfxDuration);
    }

    IEnumerator WaitWhileAnyActive(float duration)
    {
        for (var t = 0.0f; t < duration; t += Time.deltaTime)
            if (IsVfxAbsent)  break; else yield return null;
    }

    #endregion

    #region Private members

    static class IDs
    {
        public static int Fader = Shader.PropertyToID("Fader");
        public static int KeyColor = Shader.PropertyToID("KeyColor");
        public static int AltColor = Shader.PropertyToID("AltColor");
    }

    ColorMaster _colorMaster;
    Random _random;

    bool IsVfxAbsent
      => _vfxGroup1.All(vfx => !vfx.enabled) &&
         _vfxGroup2.All(vfx => !vfx.enabled);

    float FaderDelta
      => Time.deltaTime / _fadingDuration;

    float RandomVfxDuration
      => _random.NextFloat(_vfxDurationMin, _vfxDurationMax);

    void PrepareRandom()
    {
        _random = new Random(_randomSeed);
        _random.NextUInt();
        _random.NextUInt();
        _random.NextUInt();
    }

    void SetVfxColors(VisualEffect vfx, Color keyColor, Color altColor)
    {
        if (!vfx.enabled) return;

        if (vfx.HasVector4(IDs.KeyColor))
            vfx.SetVector4(IDs.KeyColor, keyColor);

        if (vfx.HasVector4(IDs.AltColor))
            vfx.SetVector4(IDs.AltColor, altColor);
    }

    #endregion
}

}
