using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyFoxSpirit : Enemy //Inherited from Enemy Class
{
    public string direction; //"horizontal" or "vertical"
    public bool orientation = true; //true: right/up, false: left,down
    //aiTimer = 1; //changes this value if timer should be anything other than default seconds

    override public void Move()
    {
        if (direction == "horizontal")
        {
            if (orientation) //right
            {
                if (col != manager.gridStatus[0].Count-1 && walkableTiles.Contains(manager.gridStatus[row][col + 1]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(0.64f, 0, 0);
                    col++;
                }
                else
                {
                    orientation = !orientation;
                }
            }
            else //left
            {
                if (col != 0 && walkableTiles.Contains(manager.gridStatus[row][col - 1]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(-0.64f, 0, 0);
                    col--;
                }
                else
                {
                    orientation = !orientation;
                }
            }
        }
        else
        {
            if (orientation) //up
            {
                if (row != 0 && walkableTiles.Contains(manager.gridStatus[row - 1][col]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.64f, 0);
                    row--;
                }
                else
                {
                    orientation = !orientation;
                }
            }
            else //down
            {
                if (row != manager.gridStatus.Count-1 && walkableTiles.Contains(manager.gridStatus[row + 1][col]))
                {
                    gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.64f, 0);
                    row++;
                }
                else
                {
                    orientation = !orientation;
                }
            }
        }
    }
}
