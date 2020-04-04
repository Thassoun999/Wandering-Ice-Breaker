//using System.Collections;
//using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour {

    private TextMeshProUGUI txt;

    void Awake()
    {
        txt = gameObject.GetComponent<TextMeshProUGUI>();
        string num = SceneManager.GetActiveScene().name.Substring(1);
        txt.text = "Level " + num;
    }
}
