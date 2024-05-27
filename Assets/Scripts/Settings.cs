using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{
    //public Slider volumeSlider;
    public AudioMixer audioMixer;
    public AudioMixer audioMixer2;
    
    public void OnCloseHandler()
    {
        gameObject.SetActive(false);
    }

    

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume",value);
    }

    public void SetVolume2(float value)
    {
        Debug.Log("SetVolume2 method called with value: " + value);
        audioMixer2.SetFloat("MyExposedParam",value);


    }


}
