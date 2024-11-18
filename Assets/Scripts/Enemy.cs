using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float damage;
    public Rigidbody2D target;
    public SpriteRenderer[] spriteRenderers;
    public SpriteRenderer[] entireRenderers;
    protected bool isLive;
    protected bool isHit;

    protected Rigidbody2D rigid;
    protected Collider2D col;
    protected Animator anim;
    protected WaitForFixedUpdate wait;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }

    protected virtual void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        anim.SetBool("Dead", false);
        isHit = false;
        health = maxHealth;
        foreach (SpriteRenderer entireRenderer in entireRenderers)
        {
            entireRenderer.color = Color.white;
            entireRenderer.enabled = true;
        }
    }
    void FixedUpdate()
    {
        if (!isLive || isHit)
            return;
        Move();
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
        damage = data.damage;
    }
    protected virtual void Move()
    {
        Vector2 dirVec = (target.position - rigid.position).normalized;
        Vector2 moveVec = dirVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVec);
        rigid.velocity = Vector2.zero;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;

        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            anim.SetBool("Dead", true);

            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
            GameManager.instance.kill++;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            DropExp();
        }
    }
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.health -= Time.deltaTime * damage;
        }
    }
    protected virtual IEnumerator KnockBack()
    {
        isHit = true;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 knockbackDir = transform.position - playerPos;
        rigid.AddForce(knockbackDir.normalized * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        isHit = false;
    }

    void DropExp()
    {
        GameObject exp = GameManager.instance.pool.Get(7);
        exp.transform.position = transform.position;
    }
    void Death() //animation에서 호출
    {
        anim.SetBool("Dead", false);
        gameObject.SetActive(false);
    }
}
