using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{
    Animator animator;
    int food;
    private int pointsPerSoda = 50;
    private int pointsPerFood = 30;

    //-------------------------

    public int wallDamage = 1;

    public float restartLevelDelay = 1f;

    public Text foodText;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip dringSound1;
    public AudioClip dringSound2;
    public AudioClip gameOverSound;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        food = GameManager.instance.foodPoints;

        foodText.text = "Food: " + food;
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.playerTurn)
        {
            return;
        }


        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove <Wall> (horizontal, vertical);
        }

    }

    protected override void AttemptMove <T> (int horizontal, int vertical)
    {
        food -= 10;
        foodText.text = "Food: " + food;

        base.AttemptMove <T> (horizontal, vertical);
        RaycastHit2D hit;

        if (Move(horizontal, vertical, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();

        GameManager.instance.playerTurn = false;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);

        animator.SetTrigger("PlayerChop");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Food")
        {
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

            food += pointsPerFood;
            foodText.text = "Food: " + food;

            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda")
        {
            SoundManager.instance.RandomizeSfx(dringSound1, dringSound2);

            food += pointsPerSoda;
            foodText.text = "Food: " + food;

            collision.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void OnDisable()
    {
        GameManager.instance.foodPoints = food;
    }

    public void LoseFood(int point)
    {
        animator.SetTrigger("PlayerHit");
        food -= point;
        foodText.text = "Food: " + food;

        CheckIfGameOver();
    }
}
