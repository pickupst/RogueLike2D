using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BoardManager boardScript;

    private List<Enemy> enemies;

    private GameObject levelImage;

    //------------------------------------------

    public static GameManager instance = null;
    
    public int level = 8;
    public int foodPoints = 100;

    [HideInInspector]
    public bool playerTurn = true;
    [HideInInspector]
    public bool enemiesMoving;

    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        enemies = new List<Enemy>();

        boardScript = GetComponent<BoardManager>();

        InitGame(); 
   
    }

    private void InitGame()
    {
        levelImage = GameObject.Find("LevelImage");

        Invoke("HideLevelImage", levelStartDelay);

        boardScript.SetupScene(level);
    }

    private void HideLevelImage ()
    {
        levelImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn || enemiesMoving)
        {
            return;
        }

        StartCoroutine(MoveEnemies());

    }

     IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }

}
