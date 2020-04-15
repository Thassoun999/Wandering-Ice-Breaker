using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonFunctions : MonoBehaviour {

    public string txt;
    bool isInt;
    int level = 0;

    public GameObject settingsMenu;
    public PlayerMovement pm;
    AudioSource audioSource;

    void Start()
    {
        txt = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        isInt = int.TryParse(txt, out level);
        if(level > PlayerPrefs.GetInt("levelAt"))
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }

        audioSource = GameObject.Find("Sound Effects").GetComponent<AudioSource>();
    }

    
   public void selectScene()
    {
        if (isInt)
        {
            SceneManager.LoadScene(1 + level);
            StartCoroutine(PlayUpbeatSoundEffect());
        }
        else if (txt == "back" || txt == "Main Menu")
        {
            SceneManager.LoadScene(0);
            StartCoroutine(PlayMediumSoundEffect());
        }
        else if (txt == "play" || txt == "Play")
        {
            if(!PlayerPrefs.HasKey("levelAt") || PlayerPrefs.GetInt("levelAt") == 1)
            {
                SceneManager.LoadScene(2);
            }
            else if(PlayerPrefs.GetInt("levelAt") + 1 > SceneManager.sceneCountInBuildSettings - 2)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelAt"));
            }
            else
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelAt") + 1);
            }
            StartCoroutine(PlayUpbeatSoundEffect());
        }
        else if (txt == "levels" || txt == "Levels")
        {
            SceneManager.LoadScene(1);
            StartCoroutine(PlayUpbeatSoundEffect());
        }
        else if (txt == "Clear Save Data")
        {
            PlayerPrefs.SetInt("levelAt", 1);
            StartCoroutine(PlayDownbeatSoundEffect());
        }
    }

    public void optionButton()
    {
        if (!settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(true);
            pm = GameObject.Find("fox(Clone)").GetComponent<PlayerMovement>();
            pm.pause = true;
            Time.timeScale = 0;
            StartCoroutine(PlayUpbeatSoundEffect());
        }
        else
        {
            optionBackButton();
        }
        
    }

    public void optionBackButton()
    {
        pm = GameObject.Find("fox(Clone)").GetComponent<PlayerMovement>();
        pm.pause = false;
        Time.timeScale = 1;
        StartCoroutine(PlayMediumSoundEffect());
        settingsMenu.SetActive(false);
    }

    IEnumerator PlayUpbeatSoundEffect()
    {
        audioSource.clip = Resources.Load("win", typeof(AudioClip)) as AudioClip;
        audioSource.Play();
        yield return null;
    }
    IEnumerator PlayDownbeatSoundEffect()
    {
        audioSource.clip = Resources.Load("lose", typeof(AudioClip)) as AudioClip;
        audioSource.Play();
        yield return null;
    }

    IEnumerator PlayMediumSoundEffect()
    {
        audioSource.clip = Resources.Load("crack", typeof(AudioClip)) as AudioClip;
        audioSource.Play();
        yield return null;
    }
}
