using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    public GameObject[] floorTiles;

    private Transform boardHolder;

    public void SetupScene()
    {
        BoardSetup();
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("BoardHolder").transform;

        for (int x = -1; x < 9; x++)
        {
            for (int y = -1; y < 9; y++)
            {
                GameObject instance = Instantiate(floorTiles[Random.Range(0, floorTiles.Length)], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
