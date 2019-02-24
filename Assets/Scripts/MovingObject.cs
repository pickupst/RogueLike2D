using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer;
    public float moveTime = 0.1f;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void AttemptMove<T>(int horizontal, int vertical)
        where T : Component
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

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

    protected bool Move(int horizontal, int vertical, out RaycastHit2D hit)
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
