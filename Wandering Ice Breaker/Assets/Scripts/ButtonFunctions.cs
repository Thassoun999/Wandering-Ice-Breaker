//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonFunctions : MonoBehaviour {

    public string txt;
    bool isInt;
    int level = 0;

    public GameObject settingsMenu;
    bool optionsShown = false;
    public PlayerMovement pm;

    void Start()
    {
        txt = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        isInt = int.TryParse(txt, out level);
        if(level > PlayerPrefs.GetInt("levelAt"))
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    
   public void selectScene()
    {
        if (isInt)
        {
            SceneManager.LoadScene(1 + level);
        }
        else if (txt == "back" || txt == "Main Menu")
        {
            SceneManager.LoadScene(0);
        }
        else if (txt == "play" || txt == "Play")
        {
            if(PlayerPrefs.GetInt("levelAt") + 1 > SceneManager.sceneCountInBuildSettings - 2)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelAt"));
            }
            else
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelAt") + 1);
            }
        }
        else if (txt == "levels" || txt == "Levels")
        {
            SceneManager.LoadScene(1);
        }
        else if (txt == "Clear Save Data")
        {
            PlayerPrefs.SetInt("levelAt", 1);
        }
    }

    public void optionButton()
    {
        if (optionsShown)
        {
            optionBackButton();
        }
        else
        {
            settingsMenu.SetActive(true);
            pm = GameObject.Find("fox(Clone)").GetComponent<PlayerMovement>();
            pm.pause = true;
            Time.timeScale = 0;
            optionsShown = true;
        }
    }

    public void optionBackButton()
    {
        settingsMenu.SetActive(false);
        pm = GameObject.Find("fox(Clone)").GetComponent<PlayerMovement>();
        pm.pause = false;
        Time.timeScale = 1;
        optionsShown = false;
    }
}
