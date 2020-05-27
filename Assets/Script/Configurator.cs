using UnityEngine;
using UnityEngine.VFX;

namespace Akvj {

sealed class Configurator : MonoBehaviour
{
    [SerializeField] GameObject _effectRoot = null;
    [SerializeField] VisualEffect _debugVfx = null;
    [Space]
    [SerializeField] Transform _cameraRoot = null;
    [SerializeField] Renderer _targetIndicator = null;

    public float DepthThreshold { get; set; } = 0;
    public float CameraOffset { set => SetCameraOffset(value); }

    void SetCameraOffset(float offset)
      => _cameraRoot.localPosition = new Vector3(0, 0, offset);

    void Start()
    {
    #if !UNITY_EDITOR
        Cursor.visible = false;
    #endif

        _effectRoot.SetActive(false);

        _debugVfx.enabled = true;
        _targetIndicator.enabled = true;

        foreach (var m in _cameraRoot.GetComponentsInChildren<Klak.Motion.BrownianMotion>())
        {
            m.transform.localPosition = Vector3.zero;
            m.transform.localRotation = Quaternion.identity;
            m.enabled = false;
        }
    }
}

}
