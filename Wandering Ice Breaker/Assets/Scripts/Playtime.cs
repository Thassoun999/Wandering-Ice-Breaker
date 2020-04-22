//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Playtime : MonoBehaviour {

    private float timer;
    private int minutes;

    void Start()
    {
        if (!PlayerPrefs.HasKey("playtime"))
        {
            PlayerPrefs.SetString("playtime", "0:00");
        }
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 60)
        {
            timer = 0.0f;
            minutes++;
            int hours = minutes / 60;
            string min = (minutes % 60).ToString();
            if(min.Length < 2)
            {
                min = "0" + min;
            }
            PlayerPrefs.SetString("playtime", hours + ":" + min);
        }
    }
}
