using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {

        public int minimum, maximum;
        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }

    }

    public Count wallCount = new Count(5, 7);
    public Count foodCount = new Count(3, 5);

    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;


    public GameObject exitTiles;

    public int columns = 8;
    public int rows = 8;

    private Transform boardHolder;

    private List<Vector3> gridPosition = new List<Vector3>();

    void IniitialiseList()
    {
        gridPosition.Clear();

        for (int x = 1; x < columns -1; x++)
        {
            for (int y = 1; y < rows -1; y++)
            {
                gridPosition.Add(new Vector3(x, y, 0));
            }
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        IniitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        Instantiate(exitTiles, new Vector3(columns -1, rows -1, 0), Quaternion.identity);
    }

    Vector3 RandomPosition()
    {

        int randomIndex = Random.Range(0, gridPosition.Count);
        Vector3 randomPosition = gridPosition[randomIndex];
        gridPosition.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();

            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity);

        }

    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("BoardHolder").transform;

        for (int x = -1; x < columns +1; x++)
        {
            for (int y = -1; y < rows +1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }


                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
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
