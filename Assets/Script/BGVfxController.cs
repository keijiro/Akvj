using UnityEngine;
using UnityEngine.VFX;
using Unity.Mathematics;
using IEnumerator = System.Collections.IEnumerator;
using Random = Unity.Mathematics.Random;

namespace Akvj {

sealed class BGVfxController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] VisualEffect[] _vfxGroup = null;
    [SerializeField] float _fadingDuration = 3;
    [SerializeField] float _vfxDurationMin = 1;
    [SerializeField] float _vfxDurationMax = 10;
    [SerializeField] float _noiseFrequency = 0.1f;
    [SerializeField] uint _randomSeed = 0;

    #endregion

    #region MonoBehaviour implementation

    IEnumerator Start()
    {
        PrepareRandom();

        var vfxCount = _vfxGroup.Length;

        _vfxGroup[0].enabled = true;

        while (true)
        {
            for (var i = 0; i < vfxCount; i++)
            {
                // Current VFX
                var vfxCurrent = _vfxGroup[i];
                vfxCurrent.enabled = true;
                yield return VfxWaitCoroutine(vfxCurrent, RandomVfxDuration);

                // Crossfading
                var vfxNext = _vfxGroup[(i + 1) % vfxCount];
                vfxNext.enabled = true;
                yield return VfxCrossFadeCoroutine(vfxCurrent, vfxNext);

                // Clearing
                vfxCurrent.SetFloat(IDs.Fader, 0);
                yield return VfxWaitCoroutine(vfxNext, 10);

                // End the current VFX.
                vfxCurrent.enabled = false;
            }
        }
    }

    #endregion

    #region VFX coroutines

    IEnumerator VfxWaitCoroutine(VisualEffect vfx, float duration)
    {
        for (var t = 0.0f; t < duration; t += Time.deltaTime)
        {
            vfx.SetFloat(IDs.Fader, NormalizedNoiseValue);
            yield return null;
        }
    }

    IEnumerator VfxCrossFadeCoroutine(VisualEffect vfxFrom, VisualEffect vfxTo)
    {
        for (var fade = 0.0f; fade < 1; fade += FaderDelta)
        {
            var n = NormalizedNoiseValue;
            vfxFrom.SetFloat(IDs.Fader, (1 - fade) * n);
            vfxTo.SetFloat(IDs.Fader, fade * n);
            yield return null;
        }
    }

    #endregion

    #region Private members

    static class IDs
      { public static int Fader = Shader.PropertyToID("Fader"); }

    Random _random;

    float RandomVfxDuration
      => _random.NextFloat(_vfxDurationMin, _vfxDurationMax);

    float FaderDelta
      => Time.deltaTime / _fadingDuration;

    float NoiseValue
      => noise.snoise(math.float2(_noiseFrequency * Time.time, 0.3f));

    float NormalizedNoiseValue
      => math.saturate(0.55f + 0.5f * NoiseValue);

    void PrepareRandom()
    {
        _random = new Random(_randomSeed);
        _random.NextUInt();
        _random.NextUInt();
        _random.NextUInt();
    }

    #endregion
}

}
