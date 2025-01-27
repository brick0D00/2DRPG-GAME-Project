using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;//����

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void SetVolumeSlider(float _value)
    {   
        //TODO:���û�����ֵ
        audioMixer.SetFloat(parameter,Mathf.Log10(_value)*multiplier);
    }
    public void LoadSlider(float _value)
    {
        if (_value >= 0.0001f)
        {
            slider.value= _value;
        }
    }
}
