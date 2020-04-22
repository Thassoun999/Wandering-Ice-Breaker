//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour {
    
    void Start()
    {
        TextMeshProUGUI txt = gameObject.GetComponent<TextMeshProUGUI>();
        txt.text = "Total Play Time: " + PlayerPrefs.GetString("playtime", "0:00") + "\nTotal Deaths: " + PlayerPrefs.GetInt("deaths",0);
    }
}
