using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int hp = 4;
    public Sprite damageSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageWall (int loss)
    {
        spriteRenderer.sprite = damageSprite;

        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}
