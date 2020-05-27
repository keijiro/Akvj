using UnityEngine;
using UnityEngine.VFX;
using BrownianMotion = Klak.Motion.BrownianMotion;
using IEnumerator = System.Collections.IEnumerator;

namespace Akvj {

sealed class Configurator : MonoBehaviour
{
    #region Editor attributes

    [Space]
    [SerializeField] GameObject _uiRoot = null;
    [Space]
    [SerializeField] GameObject _effectRoot = null;
    [SerializeField] VisualEffect _debugVfx = null;
    [Space]
    [SerializeField] Transform _cameraRoot = null;
    [SerializeField] Renderer _targetIndicator = null;

    #endregion

    #region Public accessor properties (accessed from UI)

    public float DepthThreshold
      { set => DeviceSettings.maxDepth = value; }

    public float CameraOffset
      { set => _cameraRoot.localPosition = new Vector3(0, 0, value); }

    #endregion

    #region Device settings accessor

    Akvfx.DeviceSettings DeviceSettings
      => _deviceSettings ?? (_deviceSettings = CloneDeviceSettings());

    Akvfx.DeviceSettings _deviceSettings;

    Akvfx.DeviceSettings CloneDeviceSettings()
    {
        var dev = GetComponent<Akvfx.DeviceController>();
        return (dev.DeviceSettings = Instantiate(dev.DeviceSettings));
    }

    #endregion

    #region MonoBehaviour implementation

    IEnumerator Start()
    {
        #if !UNITY_EDITOR
        Cursor.visible = false;
        #endif

        while (true)
        {
            StartConfiguration();
            yield return null;

            while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

            EndConfiguration();
            yield return null;

            while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        }
    }

    void StartConfiguration()
    {
        _uiRoot.SetActive(true);
        _effectRoot.SetActive(false);
        _debugVfx.enabled = true;
        _targetIndicator.enabled = true;

        foreach
          (var motion in _cameraRoot.GetComponentsInChildren<BrownianMotion>())
        {
            motion.transform.localPosition = Vector3.zero;
            motion.transform.localRotation = Quaternion.identity;
            motion.enabled = false;
        }
    }

    void EndConfiguration()
    {
        _uiRoot.SetActive(false);
        _effectRoot.SetActive(true);
        _debugVfx.enabled = false;
        _targetIndicator.enabled = false;

        foreach
          (var motion in _cameraRoot.GetComponentsInChildren<BrownianMotion>())
            motion.enabled = true;
    }

    #endregion
}

}
