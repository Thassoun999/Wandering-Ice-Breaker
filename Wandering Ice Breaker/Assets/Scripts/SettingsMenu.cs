using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public GameObject mainMenuButtons;
    public GameObject settingsMenu;
    public GameObject credits;
    public Slider slider;


    void Start()
    {
        //slider = GameObject.Find("/Canvas/settingsMenu/Slider").GetComponent<slider>();

        if(mainMenuButtons != null && settingsMenu != null)
        {
            mainMenuButtons.SetActive(true);
            settingsMenu.SetActive(false);
        }

        if (PlayerPrefs.HasKey("volume"))
        {
            slider.value = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            slider.value = 1;
            audioMixer.SetFloat("volume", Mathf.Log10(1) * 20);
        }
        
    }

    
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void MainMenuTransitionBack()
    {
        mainMenuButtons.SetActive(true);
        settingsMenu.SetActive(false);
        credits.SetActive(false);
    }

    public void MainMenuTransitionSettings()
    {
        mainMenuButtons.SetActive(false);
        settingsMenu.SetActive(true);
        credits.SetActive(false);
    }

    public void MainMenuTransitionCredits()
    {
        mainMenuButtons.SetActive(false);
        settingsMenu.SetActive(false);
        credits.SetActive(true);
    }
}
