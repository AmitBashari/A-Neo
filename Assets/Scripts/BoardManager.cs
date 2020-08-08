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
        public int Mininum;
        public int Maximum;

        public Count(int min, int max)
        {
            Mininum = min;
            Maximum = max;

        }
    }
    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;
    public GameObject Ghost;

    private int Colums = 16;
    private int Rows = 16;
    private Count WallCount = new Count(15, 30); // Change magic numbers
    private Count FoodCount = new Count(10, 15);

    private Transform _boardHolder;
    private List<Vector3> _gridPositions = new List<Vector3>();

    private void Awake()
    {
        Ghost = GameObject.FindGameObjectWithTag("Ghost");
    }

    void InitialiseList()
    {
        _gridPositions.Clear();
        for (int x = 1; x < Colums - 1; x++)
        {
            for (int y = 1; y < Rows - 1; y++)
            {
                _gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        _boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < Colums + 1; x++)
        {
            for (int y = -1; y < Rows + 1; y++)
            {
                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
                if (x == -1 || x == Colums || y == -1 || y == Rows)
                {
                    toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(_boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int mininum, int maximun)
    {
        int objectCount = Random.Range(mininum, maximun + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(WallTiles, WallCount.Mininum, WallCount.Maximum);
        LayoutObjectAtRandom(FoodTiles, FoodCount.Mininum, FoodCount.Maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
      
        

        IEnumerator DelayEnemySpawn()
        {
            yield return null;

        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);
        Instantiate(Exit, new Vector3(Colums - 1, Rows - 1, 0f), Quaternion.identity);

        }

        StartCoroutine(DelayEnemySpawn());

    }

 
    




}
