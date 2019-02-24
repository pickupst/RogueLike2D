﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    Animator animator;

    public int wallDamage = 1;

    public float restartLevelDelay = 1f;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        base.AttemptMove <T> (horizontal, vertical);
        GameManager.instance.playerTurn = false;
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
        }
        else if (collision.tag == "Food")
        {
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda")
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
