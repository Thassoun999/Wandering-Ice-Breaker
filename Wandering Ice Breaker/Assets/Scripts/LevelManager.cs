using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    //Double Lists used for tracking and displaying gamestate
    public List<List<int>> gridStatus = new List<List<int>>();
    public List<List<GameObject>> tiles = new List<List<GameObject>>();

    //Assets
    public GameObject ice;
    public GameObject foxPrefab;
    private PlayerMovement pm;
    private Sprite[] iceSprites;

    public GameObject GreyFoxSpiritPrefab;
    public GameObject WhiteFoxSpiritPrefab;
    public GameObject PurpleFoxSpiritPrefab;

    public List<GameObject> enemyList; // Instances of every enemy

    //Coordinates of the center of the grid (for aligning to center of screen)
    private float centerR;
    private float centerC;

    //Sound related
    public AudioClip crack;
    public AudioClip loseSound;
    public AudioClip winSound;
    AudioSource audioSource;
    

    private List<int> walkableTiles = new List<int>(); //List of which sprites can be stood on

    void Awake()
    {
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

        if(gridStatus.Count % 2 != 0)
        {
            centerR = gridStatus.Count / 2;
        }
        else
        {
            centerR = (gridStatus.Count / 2) -0.5f;
        }
        if (gridStatus[0].Count % 2 != 0)
        {
            centerC = gridStatus[0].Count / 2;
        }
        else
        {
            centerC = (gridStatus[0].Count / 2) - 0.5f ;
        }

        //Loop through grid looking for starting location (Denoted as 9) and enemy locations (any double digit number)
        for (int i = 0; i < gridStatus.Count; i++)
        {
            for (int j = 0; j < gridStatus[i].Count; j++)
            {
                if (gridStatus[i][j] == 9)
                {
                    gridStatus[i][j] = 1;

                    GameObject player = Instantiate(foxPrefab, new Vector3((j - centerC) * 0.64f, (centerR - i) * 0.64f, transform.position.z), Quaternion.identity);
                    pm = player.GetComponent<PlayerMovement>();
                    pm.row = i;
                    pm.col = j;
                    pm.manager = this;
                }

                // Grey Fox Spirit Enemy Location --> 11 for Horizontal, 12 for Vertical
                if (gridStatus[i][j] == 11 || gridStatus[i][j] == 12)
                {
                    GameObject enemy = Instantiate(GreyFoxSpiritPrefab, new Vector3((j - centerC) * 0.64f, (centerR - i) * 0.64f, transform.position.z), Quaternion.identity);
                    enemy.AddComponent<GreyFoxSpirit>();
                    enemy.GetComponent<Enemy>().row = i;
                    enemy.GetComponent<Enemy>().col = j;
                    enemy.GetComponent<Enemy>().manager = this;
                    if (gridStatus[i][j] == 11)
                    {
                        enemy.GetComponent<GreyFoxSpirit>().direction = "horizontal";
                    }
                    else
                    {
                        enemy.GetComponent<GreyFoxSpirit>().direction = "vertical";
                    }
                    enemyList.Add(enemy);

                    gridStatus[i][j] = 0; // Change the bottom into the appropriate tile
                }

                // White Fox Spirit Enemy Location --> 13
                if (gridStatus[i][j] == 13)
                {
                    GameObject enemy = Instantiate(WhiteFoxSpiritPrefab, new Vector3((j - centerC) * 0.64f, (centerR - i) * 0.64f, transform.position.z), Quaternion.identity);
                    enemy.AddComponent<WhiteFoxSpirit>();
                    enemy.GetComponent<Enemy>().row = i;
                    enemy.GetComponent<Enemy>().col = j;
                    enemy.GetComponent<Enemy>().manager = this;
                    enemyList.Add(enemy);

                    enemy.GetComponent<WhiteFoxSpirit>().whiteCount++;

                    gridStatus[i][j] = 0; // Change the bottom into the appropriate tile
                }

                // Purple Fox Spirit Enemy Location --> 14
                if (gridStatus[i][j] == 14)
                {
                    GameObject enemy = Instantiate(PurpleFoxSpiritPrefab, new Vector3((j - centerC) * 0.64f, (centerR - i) * 0.64f, transform.position.z), Quaternion.identity);
                    enemy.AddComponent<PurpleFoxSpirit>();
                    enemy.GetComponent<Enemy>().row = i;
                    enemy.GetComponent<Enemy>().col = j;
                    enemy.GetComponent<Enemy>().manager = this;
                    enemyList.Add(enemy);
                    gridStatus[i][j] = 0; // Change the bottom into the appropriate tile
                }
            }
        }

        //Instantiate every tile object on the grid
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

    void Update()
    {
        // Collision Check between AI and Player
        if(enemyList.Count > 0 && pm.isHit == false)
        {
            CollisionCheck();
        }
    }

    void CollisionCheck()
    {
        // Look through all the different AI, check if collision
        for (int i = 0; i < enemyList.Count; i++)
        {
            if(enemyList[i].GetComponent<Enemy>().row == pm.row && enemyList[i].GetComponent<Enemy>().col == pm.col) // Collision!! Make player lose!
            {
                pm.isHit = true;
                Destroy(pm.gameObject);
                StartCoroutine(Lose());
                return;
            }
        }
    }

    public void UpdateTile(int R, int C, int direction) //directions: 0 = north, 1 = south, 2 = West, 3 East
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
            pm.pause = true;
            pm.Slide(direction);
            
        }

        //Checks if player fell through the ice
        if (gridStatus[R][C] == 2)
        {
            Destroy(pm.gameObject);
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
        pm.pause = true;
        audioSource.PlayOneShot(loseSound);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Win()
    {
        pm.pause = true;
        audioSource.PlayOneShot(winSound);
        yield return new WaitForSeconds(1);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene - 1 > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextScene - 1);
        }
        SceneManager.LoadScene(nextScene);
    }

}
