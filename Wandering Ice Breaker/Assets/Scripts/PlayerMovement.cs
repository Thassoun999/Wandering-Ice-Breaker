using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public int row;
    public int col;
    public bool isHit = false; // Need this so that the player isn't hit multiple times by same spirit (lose sound plays only once and not 1000 times)
    public LevelManager manager;
    private List<int> walkableTiles = new List<int>(); //List of which sprites can be stood on

    public bool pause = false;
    public bool startedMoving = false;
    private float slipSpeed = 1;
    Vector3 dirVector;

    void Start()
    {
        //Defines which tiles can be stood on
        walkableTiles.Add(0);
        walkableTiles.Add(1);
        walkableTiles.Add(3);
        walkableTiles.Add(4);
        walkableTiles.Add(5);
    }

    void Update()
    {
        //Control to restart level
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Movement controls
        if ((Input.GetKeyDown("up") || Input.GetKeyDown("w")) && !pause)
        {
            if (walkableTiles.Contains(manager.gridStatus[row - 1][col]))
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.64f, 0);
                row--;
                manager.UpdateTile(row, col, 0);
                startedMoving = true;
            }

        }
        if ((Input.GetKeyDown("down") || Input.GetKeyDown("s")) && !pause)
        {
            if (walkableTiles.Contains(manager.gridStatus[row + 1][col]))
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.64f, 0);
                row++;
                manager.UpdateTile(row, col, 1);
                startedMoving = true;
            }

        }
        if ((Input.GetKeyDown("left") || Input.GetKeyDown("a")) && !pause)
        {
            if (walkableTiles.Contains(manager.gridStatus[row][col - 1]))
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(-0.64f, 0, 0);
                col--;
                manager.UpdateTile(row, col, 2);
                startedMoving = true;
            }

        }
        if ((Input.GetKeyDown("right") || Input.GetKeyDown("d")) && !pause)
        {
            if (walkableTiles.Contains(manager.gridStatus[row][col + 1]))
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0.64f, 0, 0);
                col++;
                manager.UpdateTile(row, col, 3);
                startedMoving = true;
            }

        }
    }

    public void Slide(int direction)
    {
        if (direction == 0)
        {
            if (walkableTiles.Contains(manager.gridStatus[row - 1][col]))
            {
                row--;
                StartCoroutine(SlideAnim(direction));
            }
            else
            {
                pause = false;
            }
        }
        else if (direction == 1)
        {
            if (walkableTiles.Contains(manager.gridStatus[row + 1][col]))
            {
                row++;
                StartCoroutine(SlideAnim(direction));
            }
            else
            {
                pause = false;
            }
        }
        else if (direction == 2)
        {
            if (walkableTiles.Contains(manager.gridStatus[row][col - 1]))
            {
                col--;
                StartCoroutine(SlideAnim(direction));
            }
            else
            {
                pause = false;
            }
        }
        else if (direction == 3)
        {
            if (walkableTiles.Contains(manager.gridStatus[row][col + 1]))
            {
                col++;
                StartCoroutine(SlideAnim(direction));
            }
            else
            {
                pause = false;
            }
        }
    }
    public IEnumerator SlideAnim(int direction)
    {
        
        if(direction == 0)
        {
            dirVector = new Vector3(0, 0.64f, 0);
        }
        else if(direction == 1)
        {
            dirVector = new Vector3(0, -0.64f, 0);
        }
        else if(direction == 2)
        {
            dirVector = new Vector3(-0.64f, 0, 0);
        }
        else if (direction == 3)
        {
            dirVector = new Vector3(0.64f, 0, 0);
        }

        float startTime = Time.time;
        Vector3 start_pos = transform.position;
        Vector3 end_pos = transform.position + dirVector;

        while (start_pos != end_pos && ((Time.time - startTime) * slipSpeed) < 0.18195f)
        {
            float move = Mathf.Lerp(0, 1, (Time.time - startTime) * slipSpeed);
            transform.position += dirVector * move;

            yield return null;
        }
        transform.position = end_pos;
        pause = false;
        manager.UpdateTile(row, col, direction);
    }

}
