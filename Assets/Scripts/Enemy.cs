using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    private Animator animator;
    private Transform target;
    private bool skipMove;

    public int playerDamage = 30;
    protected override void Awake()
    {
        GameManager.instance.AddEnemyToList(this);

        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        base.Awake();
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(playerDamage);

        animator.SetTrigger("EnemyAttack");
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void AttemptMove<T>(int horizontal, int vertical)
    {

        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(horizontal, vertical);
        skipMove = true;
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
