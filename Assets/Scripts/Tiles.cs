using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{

    public GameObject tile;
    public GameObject[][] tiles = new GameObject[][] { new GameObject[5], new GameObject[5], new GameObject[5] };

    public GameObject[] possibleTiles;
    public GameObject[] barrenTiles;
    public GameObject reunaLeft;
    public GameObject reunaRight;
    private Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        createTiles();
        setPos();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setPos()
    {
        startPos = tiles[1][0].transform.position;
    }

    void createTiles()
    {
        for (int y = 0; y < 3; y++)
        {
            createRow(y, true);
        }
    }

    void createRow(int y, bool barren = false)
    {
        for (int x = 0; x < 5; x++)
        {   
            Vector3 pos = startPos + new Vector3(-x * 50, 0, -y * 50);

            GameObject g;
            if (x == 0)
            {
                g = Instantiate(reunaLeft, pos, tile.transform.rotation, this.transform);
            }
            if (x == 4)
            {
                g = Instantiate(reunaRight, pos, tile.transform.rotation, this.transform);
            }
            else
            {
                g = Instantiate(GetTile(barren), pos, tile.transform.rotation, this.transform);
            }
            g.name = x + " + " + y;
            tiles[y][x] = g;
        }
    }

    public void shiftDown()
    {
        GameObject[] temp = new GameObject[5];
        for (int y = 0; y < 3; y++)
        {
            temp = new GameObject[5];
            for (int x = 0; x < 5; x++)
            {
                temp[x] = tiles[y][x];

                if (y == 0)
                {
                    Destroy(tiles[y][x]);
                }
            }

            // if not first row
            if (y > 0)  
            {
                tiles[y - 1] = temp;
            }
        }

        createRow(2);
        setPos();
    }

    public GameObject GetTile(bool barren)
    {
        if (barren)
        {
            return barrenTiles[Random.Range(0, barrenTiles.Length)];
        }

        return possibleTiles[Random.Range(0, possibleTiles.Length)];
    }
}
