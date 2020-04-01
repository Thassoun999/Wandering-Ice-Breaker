/* Author: Alexander Ngo
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    //Double Lists used to tracking and displaying gamestate
    private List<List<int>> gridStatus = new List<List<int>>();
    public List<List<GameObject>> tiles = new List<List<GameObject>>();

    //Assets
    public GameObject ice;
    public GameObject foxPrefab;
    public GameObject greyFoxSpiritPrefab;
    public GameObject player;
    private Sprite[] iceSprites;

    public GameObject[] greyFoxSpiritArray; // Records all instances

    //Coordinates of the center of the grid (for aligning to center of screen)
    private int centerR;
    private int centerC;

    //Current coords of the player
    public int foxR;
    public int foxC;

    //Current coords of all enemy AI -- Grey Fox Spirit
    public List<List<int>> greyFoxCoords = new List<List<int>>(); // Records all coordinates of all grey foxes
    private int greyFoxCount = 0; // How many?
    private List<int> aiDirections = new List<int>(); // For each AI figure out Vertical or Horizontal
    private List<bool> aiOrientation = new List<bool>(); // For each AI, figure out up / right (TRUE) or down / left (FALSE)

    private float maxTimerAIMove = 1.0f; // Have this kept on hand! MAX Timer for AI move!
    private float timerAIMove = 1.0f; // This will decrease per second, once it hits 0 the AI moves and we reset it to above variable

    private bool isHit = false; // Need this so that the player isn't hit multiple times by same spirit (lose sound plays only once and not 1000 times)

    //Sound related
    public AudioClip crack;
    public AudioClip loseSound;
    public AudioClip winSound;
    AudioSource audioSource;
    private bool pause = false;

    private List<int> walkableTiles = new List<int>(); //List of which sprites can be stood on

    void Awake()
    {
        //Defines which tiles can be stood on
        walkableTiles.Add(0);
        walkableTiles.Add(1);
        walkableTiles.Add(3);
        walkableTiles.Add(4);
        walkableTiles.Add(5);

        audioSource = GetComponent<AudioSource>();
        iceSprites = Resources.LoadAll<Sprite>("Ice Tile");

        //Load level data from CVS with name matching the scene, i.e. Scene "LV2" uses "LV2.csv"
        TextAsset tileData = Resources.Load<TextAsset>(SceneManager.GetActiveScene().name);

        string[] data = tileData.text.Split(new char[] {  '\n' });

        for (int i = 1; i <data.Length - 1; i++)
        {
            List<int> newRow = new List<int>();
            gridStatus.Add(newRow);
            string[] row = data[i].Split(new char[] { ',' });

            for(int j = 0; j < row.Length; j++)
            {
                gridStatus[i - 1].Add(int.Parse(row[j]));
            }
            
        }

        centerR = gridStatus.Count / 2;
        centerC = gridStatus[0].Count / 2;

        //Loop through grid looking for starting location (Denoted as -1)
        for (int i = 0; i < gridStatus.Count; i++)
        {
            for (int j = 0; j < gridStatus[i].Count; j++)
            {
                if (gridStatus[i][j] == 9)
                {
                    foxR = i;
                    foxC = j;
                    gridStatus[i][j] = 1;
                }

                // Grey Fox Spirit Enemy Location --> 11 for Horizontal, 12 for Vertical
                if (gridStatus[i][j] == 11 || gridStatus[i][j] == 12)
                {
                    List<int> AiLocGrey = new List<int>() { i, j, gridStatus[i][j]}; // Record location
                    greyFoxCoords.Add(AiLocGrey); // Add location
                    greyFoxCount++; // increase greyFoxCount

                    gridStatus[i][j] = 0; // Change the bottom into the appropriate tile
                }
            }
        }

        for (int i = 0; i < gridStatus.Count; i++)
        {
            tiles.Add(new List<GameObject>());
            for(int j = 0; j < gridStatus[i].Count; j++)
            {
                GameObject tile = Instantiate(ice, new Vector3( (j-centerC) * 0.64f, (centerR-i) * 0.64f, transform.position.z) , Quaternion.identity);
                tile.transform.parent = this.gameObject.transform;
                tile.GetComponent<SpriteRenderer>().sprite = iceSprites[gridStatus[i][j]];
                tile.name = i + " " + j;
                tiles[i].Add(tile);
            }
        }

    }

    void Start()
    {
        player = Instantiate(foxPrefab, new Vector3((foxC - centerC) * 0.64f, (centerR - tiles.Count + 1) * 0.64f, transform.position.z), Quaternion.identity);
        isHit = false;

        greyFoxSpiritArray = new GameObject[greyFoxCount];
        // For loop instantiating game objects and adding them in!
        for (int i = 0; i < greyFoxCount; i++)
        {
            GameObject greyFox = new GameObject(); // Create new spirit object
            // Instantiate + add object to array + record its direction

            // Problem 1
            greyFox = Instantiate(greyFoxSpiritPrefab, new Vector3((greyFoxCoords[i][1] - centerC) * 0.64f, (centerR - greyFoxCoords[i][0]) * 0.64f, transform.position.z), Quaternion.identity);
            greyFoxSpiritArray[i] = greyFox;
            aiDirections.Add(greyFoxCoords[i][2]); // Verticle or Horizontal
            aiOrientation.Add(true); // All start by going up / right
        }
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
            //if (gridStatus[foxR-1][foxC] == 0 || gridStatus[foxR - 1][foxC] == 1)
            if (walkableTiles.Contains(gridStatus[foxR - 1][foxC]))
            {
                player.transform.position = player.transform.position + new Vector3(0, 0.64f, 0);
                foxR--;
                UpdateTile(foxR,foxC, 0);
            }
            
        }
        if ((Input.GetKeyDown("down") || Input.GetKeyDown("s")) && !pause)
        {
            //if (gridStatus[foxR + 1][foxC] == 0 || gridStatus[foxR + 1][foxC] == 1)
            if (walkableTiles.Contains(gridStatus[foxR + 1][foxC]))
            {
                player.transform.position = player.transform.position + new Vector3(0, -0.64f, 0);
                foxR++;
                UpdateTile(foxR, foxC, 1);
            }

        }
        if ((Input.GetKeyDown("left") || Input.GetKeyDown("a")) && !pause)
        {
            //if (gridStatus[foxR][foxC-1] == 0 || gridStatus[foxR][foxC-1] == 1)
            if (walkableTiles.Contains(gridStatus[foxR][foxC - 1]))
            {
                player.transform.position = player.transform.position + new Vector3(-0.64f, 0, 0);
                foxC--;
                UpdateTile(foxR, foxC, 2);
            }

        }
        if ((Input.GetKeyDown("right") || Input.GetKeyDown("d")) && !pause)
        {
            //if (gridStatus[foxR][foxC+1] == 0 || gridStatus[foxR][foxC+1] == 1)
            if (walkableTiles.Contains(gridStatus[foxR][foxC + 1]))
            {
                player.transform.position = player.transform.position + new Vector3(0.64f, 0, 0);
                foxC++;
                UpdateTile(foxR, foxC, 3);

            }

        }

        // Move every 1 second
        timerAIMove -= Time.deltaTime;
        if(timerAIMove < 0 && greyFoxCount > 0)
        {
            moveAI(); // Move every AI a single tile in their intended direction
            timerAIMove = maxTimerAIMove; // reset the timer
        }

        

        // Collision Check between AI and Player
        if(greyFoxCount > 0)
        {
            //Debug.Log(greyFoxCount);
            CollisionCheck();
        }
    }

    void moveAI()
    {
        // Ai Movement Controls
        for (int i = 0; i < greyFoxCount; i++)
        {
            if (aiDirections[i] == 11) // Horizontal Movement
            {
                if (aiOrientation[i] == true) // Right
                {

                    if (walkableTiles.Contains(gridStatus[greyFoxCoords[i][0]][greyFoxCoords[i][1] + 1]))
                    {
                        // Update transform (position) and coordinate record
                        greyFoxSpiritArray[i].transform.position = greyFoxSpiritArray[i].transform.position + new Vector3(0.64f, 0, 0);
                        greyFoxCoords[i][1]++;
                    }
                    else
                    {
                        aiOrientation[i] = false; // change direction if tile not walkable
                    }
                }
                else // Left
                {
                    if (walkableTiles.Contains(gridStatus[greyFoxCoords[i][0]][greyFoxCoords[i][1] - 1]))
                    {
                        // Update transform (position) and coordinate record
                        greyFoxSpiritArray[i].transform.position = greyFoxSpiritArray[i].transform.position + new Vector3(-0.64f, 0, 0);
                        greyFoxCoords[i][1]--;
                    }
                    else
                    {
                        aiOrientation[i] = true; // change direction if tile not walkable
                    }
                }
            }
            else if (aiDirections[i] == 12) // Vertical Movement
            {
                if (aiOrientation[i] == true) // Up
                {
                    if (walkableTiles.Contains(gridStatus[greyFoxCoords[i][0] - 1][greyFoxCoords[i][1]]))
                    {
                        // Update transform (position) and coordinate record
                        greyFoxSpiritArray[i].transform.position = greyFoxSpiritArray[i].transform.position + new Vector3(0, 0.64f, 0);
                        greyFoxCoords[i][0]--;
                    }
                    else
                    {
                        aiOrientation[i] = false; // change direction if tile not walkable
                    }
                }
                else // Down
                {
                    if (walkableTiles.Contains(gridStatus[greyFoxCoords[i][0] + 1][greyFoxCoords[i][1]]))
                    {
                        // Update transform (position) and coordinate record
                        greyFoxSpiritArray[i].transform.position = greyFoxSpiritArray[i].transform.position + new Vector3(0, -0.64f, 0);
                        greyFoxCoords[i][0]++;
                    }
                    else
                    {
                        aiOrientation[i] = true; // change direction if tile not walkable
                    }
                }
            }
        }
    }

    void CollisionCheck()
    {
        // Look through all the different AI, check if collision
        for (int i = 0; i < greyFoxCount; i++)
        {
            if(greyFoxCoords[i][0] == foxR && greyFoxCoords[i][1] == foxC) // Collision!! Make player lose!
            {
                if(isHit == false)
                {
                    isHit = true;
                    Destroy(player);
                    StartCoroutine(Lose());
                }
                
            }
        }
    }

    void UpdateTile(int R, int C, int direction) //directions: 0 = north, 1 = south, 2 = West, 3 East
    {
        //Update tile if it's breakable
        if (gridStatus[R][C] == 0 || gridStatus[R][C] == 1)
        {
            gridStatus[R][C]++;
            tiles[R][C].GetComponent<SpriteRenderer>().sprite = iceSprites[gridStatus[R][C]];
            audioSource.PlayOneShot(crack, 0.5f);
        }

        //If player steps on slippery ice
        if (gridStatus[R][C] == 4)
        {
            StartCoroutine(Slide(direction));
            
        }

        //Checks if player fell through the ice
        if (gridStatus[R][C] == 2)
        {
            Destroy(player);
            StartCoroutine(Lose());
        }

        //If player walks over destination tile (denoted as 5)
        if (gridStatus[R][C] == 5) 
        {
            for(int i = 0; i < gridStatus.Count; i++)
            {
                for(int j =0; j < gridStatus[0].Count; j++)
                {
                    if(gridStatus[i][j] == 0)
                    {
                        return;
                    }
                }
            }

            StartCoroutine(Win());
        }
    }

    IEnumerator Lose()
    {
        pause = true;
        audioSource.PlayOneShot(loseSound);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Win()
    {
        pause = true;
        audioSource.PlayOneShot(winSound);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator Slide(int direction)
    {
        yield return new WaitForSeconds(0);
        if (direction == 0)
        {
            if (walkableTiles.Contains(gridStatus[foxR - 1][foxC]))
            {
                player.transform.position = player.transform.position + new Vector3(0, 0.64f, 0);
                foxR--;
                UpdateTile(foxR, foxC, 0);
            }
        }
        if (direction == 1)
        {
            if (walkableTiles.Contains(gridStatus[foxR + 1][foxC]))
            {
                player.transform.position = player.transform.position + new Vector3(0, -0.64f, 0);
                foxR++;
                UpdateTile(foxR, foxC, 1);
            }
        }
        if (direction == 2)
        {
            if (walkableTiles.Contains(gridStatus[foxR][foxC - 1]))
            {
                player.transform.position = player.transform.position + new Vector3(-0.64f, 0, 0);
                foxC--;
                UpdateTile(foxR, foxC, 2);
            }
        }
        if (direction == 3)
        {
            if (walkableTiles.Contains(gridStatus[foxR][foxC + 1]))
            {
                player.transform.position = player.transform.position + new Vector3(0.64f, 0, 0);
                foxC++;
                UpdateTile(foxR, foxC, 3);
            }
        }
    }

}
