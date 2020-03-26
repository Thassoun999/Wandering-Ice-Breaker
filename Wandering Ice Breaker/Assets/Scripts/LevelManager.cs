/* Author: Alexander Ngo
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private List<List<int>> tileStart = new List<List<int>>();
    public List<List<GameObject>> tiles = new List<List<GameObject>>();
    public GameObject ice;
    public GameObject foxPrefab;
    public GameObject player;
    private Sprite[] iceSprites;
    private int centerR;
    private int centerC;
    public int foxR;
    public int foxC;

    public AudioClip crack;
    public AudioClip loseSound;
    public AudioClip winSound;
    AudioSource audioSource;
    private bool pause = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        iceSprites = Resources.LoadAll<Sprite>("Ice Tile");

        TextAsset tileData = Resources.Load<TextAsset>(SceneManager.GetActiveScene().name);

        string[] data = tileData.text.Split(new char[] {  '\n' });

        for (int i = 1; i <data.Length - 1; i++)
        {
            List<int> newRow = new List<int>();
            tileStart.Add(newRow);
            string[] row = data[i].Split(new char[] { ',' });

            for(int j = 0; j < row.Length; j++)
            {
                tileStart[i - 1].Add(int.Parse(row[j]));
            }
            
        }

        centerR = tileStart.Count / 2;
        centerC = tileStart[0].Count / 2;

        for(int i = 0; i < tileStart.Count; i++)
        {
            tiles.Add(new List<GameObject>());
            for(int j = 0; j < tileStart[i].Count; j++)
            {
                GameObject tile = Instantiate(ice, new Vector3( (j-centerC) * 0.64f, (centerR-i) * 0.64f, transform.position.z) , Quaternion.identity);
                tile.transform.parent = this.gameObject.transform;
                tile.GetComponent<SpriteRenderer>().sprite = iceSprites[tileStart[i][j]];
                tile.name = i + " " + j;
                tiles[i].Add(tile);
            }
        }

    }

    void Start()
    {

        foxR = tiles.Count-1;

        if(SceneManager.GetActiveScene().name == "L1" || SceneManager.GetActiveScene().name == "L2" || SceneManager.GetActiveScene().name == "L3")
        {
            foxC = 2;
        }
        else
        {
            foxC = 3;
        }
        player = Instantiate(foxPrefab, new Vector3((centerC - foxC) * 0.64f, (centerR - tiles.Count + 1) * 0.64f, transform.position.z), Quaternion.identity);
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if ((Input.GetKeyDown("up") || Input.GetKeyDown("w")) && !pause)
        {
            if (tileStart[foxR-1][foxC] == 0 || tileStart[foxR - 1][foxC] == 1)
            {
                player.transform.position = player.transform.position + new Vector3(0, 0.64f, 0);
                foxR--;
                UpdateTile(foxR,foxC);
            }
            
        }
        if ((Input.GetKeyDown("down") || Input.GetKeyDown("s")) && !pause)
        {
            if (tileStart[foxR + 1][foxC] == 0 || tileStart[foxR + 1][foxC] == 1)
            {
                player.transform.position = player.transform.position + new Vector3(0, -0.64f, 0);
                foxR++;
                UpdateTile(foxR, foxC);
            }

        }
        if ((Input.GetKeyDown("left") || Input.GetKeyDown("a")) && !pause)
        {
            if (tileStart[foxR][foxC-1] == 0 || tileStart[foxR][foxC-1] == 1)
            {
                player.transform.position = player.transform.position + new Vector3(-0.64f, 0, 0);
                foxC--;
                UpdateTile(foxR, foxC);
            }

        }
        if ((Input.GetKeyDown("right") || Input.GetKeyDown("d")) && !pause)
        {
            if (tileStart[foxR][foxC+1] == 0 || tileStart[foxR][foxC+1] == 1)
            {
                player.transform.position = player.transform.position + new Vector3(0.64f, 0, 0);
                foxC++;
                UpdateTile(foxR, foxC);
            }

        }
    }

    void UpdateTile(int R, int C)
    {
        tileStart[R][C]++;
        tiles[R][C].GetComponent<SpriteRenderer>().sprite = iceSprites[tileStart[R][C]];
        audioSource.PlayOneShot(crack,0.5f);

        if (tileStart[R][C] == 2) //lose
        {
            Destroy(player);
            StartCoroutine(Lose());
        }

        if (R == 0) //beat level
        {
            for(int i = 0; i < tileStart.Count; i++)
            {
                for(int j =0; j < tileStart[0].Count; j++)
                {
                    if(tileStart[i][j] == 0)
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
}
