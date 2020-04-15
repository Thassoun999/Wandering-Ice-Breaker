using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFoxSpirit : Enemy
{
    public int whiteCount = 0;
    // This Spirit does not move based on time, it moves in relation to the player, but inversely
    override public void Move() // Nothing should happen here
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.pause == false)
        {
            if ((Input.GetKeyDown("up") || Input.GetKeyDown("w")))
            {
                if (walkableTiles.Contains(manager.gridStatus[row + 1][col]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.64f, 0);
                    row++;
                }

            }
            if ((Input.GetKeyDown("down") || Input.GetKeyDown("s")))
            {
                if (walkableTiles.Contains(manager.gridStatus[row - 1][col]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.64f, 0);
                    row--;

                }

            }
            if ((Input.GetKeyDown("left") || Input.GetKeyDown("a")))
            {
                if (walkableTiles.Contains(manager.gridStatus[row][col + 1]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(0.64f, 0, 0);
                    col++;

                }

            }
            if ((Input.GetKeyDown("right") || Input.GetKeyDown("d")))
            {
                if (walkableTiles.Contains(manager.gridStatus[row][col - 1]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(-0.64f, 0, 0);
                    col--;

                }

            }
        }
        
    }
}
