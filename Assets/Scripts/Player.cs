using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void AttemptMove(int horizontal, int vertical)
    {

        Move(horizontal, vertical);

    }

    void Move(int horizontal, int vertical)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontal, vertical);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
        }

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
            AttemptMove(horizontal, vertical);
        }

    }

    protected override void AttemptMove(int horizontal, int vertical)
    {
        base.AttemptMove(horizontal, vertical);
        GameManager.instance.playerTurn = false;
    }
}
