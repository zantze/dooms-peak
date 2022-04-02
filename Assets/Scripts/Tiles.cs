using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{

    public GameObject tile;
    public GameObject[][] tiles = new GameObject[][] { new GameObject[3], new GameObject[3], new GameObject[3] };

    public GameObject[] possibleTiles;
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
            createRow(y);
        }
    }

    void createRow(int y)
    {
        for (int x = 0; x < 3; x++)
        {   
            Vector3 pos = startPos + new Vector3(-x * 50, 0, -y * 50);
            GameObject g = Instantiate(GetTile(), pos, tile.transform.rotation, this.transform);
            g.name = x + " + " + y;
            tiles[y][x] = g;
        }
    }

    public void shiftDown()
    {
        GameObject[] temp = new GameObject[3];
        for (int y = 0; y < 3; y++)
        {
            temp = new GameObject[3];
            for (int x = 0; x < 3; x++)
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

    public GameObject GetTile()
    {
        return possibleTiles[Random.Range(0, possibleTiles.Length)];
    }
}
