using UnityEngine;
using UnityEngine.UI;

public class ToggleControls : MonoBehaviour {

    public GameObject swipeManager;
    public GameObject buttonContainer;
    public GameObject swipeToggle;
    public GameObject buttonToggle;

    public void Start()
    {
        if (PlayerPrefs.HasKey("buttonToggle"))
        {
            buttonToggle.GetComponent<Toggle>().isOn = (PlayerPrefs.GetInt("buttonToggle") == 1);
            buttonContainer.SetActive(PlayerPrefs.GetInt("buttonToggle") == 1);
        }
        if (PlayerPrefs.HasKey("swipeToggle"))
        {
            swipeToggle.GetComponent<Toggle>().isOn = (PlayerPrefs.GetInt("swipeToggle") == 1);
        }
    }

    public void ToggleButtons()
    {
        if(buttonToggle.GetComponent<Toggle>().isOn == true)
        {
            PlayerPrefs.SetInt("buttonToggle", 1);
        }
        else
        {
            PlayerPrefs.SetInt("buttonToggle", 0);

        }
        PlayerPrefs.Save();
    }

    public void ToggleSwipe()
    {
        if (swipeToggle.GetComponent<Toggle>().isOn == true)
        {
            PlayerPrefs.SetInt("swipeToggle", 1);
            swipeManager.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("swipeToggle", 0);
            swipeManager.SetActive(false);
        }
        PlayerPrefs.Save();
    }
}
