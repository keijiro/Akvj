using UnityEngine;
using UnityEngine.UI;

namespace Akvj {

sealed class SliderToPlayerPrefs : MonoBehaviour
{
    Slider Slider => GetComponent<Slider>();

    void Start()
      => Slider.value = PlayerPrefs.GetFloat(name, Slider.value);

    void OnDisable()
      => PlayerPrefs.SetFloat(name, Slider.value);
}

}
