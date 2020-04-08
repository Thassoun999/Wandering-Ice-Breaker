using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public LevelManager manager;
    public int row;
    public int col;
    protected List<int> walkableTiles = new List<int>(); //List of which sprites can be stood on
    public float aiTimer = 1; //How long until enemy moves (may change depending on enemy type)

    public void Start()
    {
        StartCoroutine(MovementTimer());
        walkableTiles.Add(0);
        walkableTiles.Add(1);
        walkableTiles.Add(3);
        walkableTiles.Add(4);
        walkableTiles.Add(5);
        walkableTiles.Add(6);
    }

    virtual public void Move() {}

    IEnumerator MovementTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(aiTimer);
            Move();
        }
    }

}