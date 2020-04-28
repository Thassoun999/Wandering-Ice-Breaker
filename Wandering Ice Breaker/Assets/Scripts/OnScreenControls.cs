using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenControls : MonoBehaviour {

    private PlayerMovement player;
    private List<WhiteFoxSpirit> whiteFoxSpirits = new List<WhiteFoxSpirit>();

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        GameObject[] whiteFoxArray = GameObject.FindGameObjectsWithTag("whiteFox");
        for(int i = 0; i < whiteFoxArray.Length; i++)
        {
            whiteFoxSpirits.Add(whiteFoxArray[i].GetComponent<WhiteFoxSpirit>());
        }
    }

    public void MoveUp()
    {
        player.MoveUp();
        for(int i = 0; i < whiteFoxSpirits.Count; i++)
        {
            whiteFoxSpirits[i].MoveUp();
        }
    }
    public void MoveDown()
    {
        player.MoveDown();
        for (int i = 0; i < whiteFoxSpirits.Count; i++)
        {
            whiteFoxSpirits[i].MoveDown();
        }
    }
    public void MoveLeft()
    {
        player.MoveLeft();
        for (int i = 0; i < whiteFoxSpirits.Count; i++)
        {
            whiteFoxSpirits[i].MoveLeft();
        }
    }
    public void MoveRight()
    {
        player.MoveRight();
        for (int i = 0; i < whiteFoxSpirits.Count; i++)
        {
            whiteFoxSpirits[i].MoveRight();
        }
    }
}
