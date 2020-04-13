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
        else if (txt == "options" || txt == "Options")
        {
            SceneManager.LoadScene(0);
        }
        else if (txt == "credits" || txt == "Credits")
        {
            SceneManager.LoadScene(0);
        }
    }
}
