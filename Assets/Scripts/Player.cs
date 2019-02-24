using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{

    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void AttemptMove <T> (int horizontal, int vertical)
        where T: Component
    {
        RaycastHit2D hit;

        bool canMove = Move(horizontal, vertical, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove <T> (T component)
        where T: Component;

    bool Move(int horizontal, int vertical, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontal, vertical);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDist = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDist > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb2D.position, end, 10f * Time.deltaTime);
            rb2D.MovePosition(newPos);

            sqrRemainingDist = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

}


public class Player : MovingObject
{
    Animator animator;

    public int wallDamage = 1;

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
}
