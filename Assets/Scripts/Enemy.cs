using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public Rigidbody2D target;
    public SpriteRenderer[] spriteRenderers;
    bool isLive;

    Rigidbody2D rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }
    void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 dirVec = (target.position - rigid.position).normalized;
        Vector2 moveVec = dirVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive)
            return;
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = target.position.x > rigid.position.x;
        }
    }
    public void Init(SpawnData data)
    {
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
}
