using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    public SpriteRenderer[] spriteRenderers;
    bool isLive = true; //Todo Test

    Rigidbody2D rigid;
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
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
}
