using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;

    private void Start()
    {
        SetSensitivity(PlayerPrefs.GetFloat("SavedMouseSensitivity", 800));
    }

    public void SetSensitivity(float _value)
    {

        if (_value < 1)
        {
            _value = 1f;
        }


        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedMouseSensitivity", _value/100);
    }
    public void SetSensitivityFromSlider()
    {
        SetSensitivity(sensitivitySlider.value);

    }

    public void RefreshSlider(float _value)
    {

        sensitivitySlider.value = _value;
    }
}
