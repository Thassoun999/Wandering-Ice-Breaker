using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    //public AudioMixer soundEffectMixer;
    public GameObject mainMenuButtons;
    public GameObject settingsMenu;
    public GameObject credits;
    public Slider slider;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("Sound Effects").GetComponent<AudioSource>();
        if (mainMenuButtons != null && settingsMenu != null)
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
        audioSource.clip = Resources.Load("crack", typeof(AudioClip)) as AudioClip;
        audioSource.Play();
    }

    public void MainMenuTransitionSettings()
    {
        mainMenuButtons.SetActive(false);
        settingsMenu.SetActive(true);
        credits.SetActive(false);
        audioSource.clip = Resources.Load("win", typeof(AudioClip)) as AudioClip;
        audioSource.Play();
    }

    public void MainMenuTransitionCredits()
    {
        mainMenuButtons.SetActive(false);
        settingsMenu.SetActive(false);
        credits.SetActive(true);
        audioSource.clip = Resources.Load("win", typeof(AudioClip)) as AudioClip;
        audioSource.Play();
    }
}
