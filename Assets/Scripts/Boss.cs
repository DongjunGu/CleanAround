using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject bossKid;
    public GameObject bossBullet;
    public GameObject bossHpUI;
    protected override void OnEnable()
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

        Instantiate(bossHpUI, GameManager.instance.HUDUI.transform);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;


        if (health <= 0)
        {
            bossHpUI.SetActive(false);
            anim.SetBool("Dead", true);

            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
            GameManager.instance.kill++;
        }
    }
    public enum BossState
    {
        Idle,
        CreateKid,
        FireOne,
        FireArc,
        FireCircle,
        Dead
    }

    public BossState currentState = BossState.Idle;

    void Start()
    {
        StartCoroutine(BossPattern());
    }
    void Update()
    {
        if (health > maxHealth * 0.75f)
            currentState = BossState.CreateKid;
        if (health <= maxHealth * 0.75 && health > maxHealth * 0.5f)
            currentState = BossState.FireOne;
        if (health <= maxHealth * 0.5f && health > maxHealth * 0.25f)
            currentState = BossState.FireArc;
        if (health <= maxHealth * 0.25f)
            currentState = BossState.FireCircle;
        if (health <= 0)
        {
            currentState = BossState.Dead;
            GameManager.instance.GameEnd();
        }

    }
    IEnumerator BossPattern()
    {
        while (currentState != BossState.Dead)
        {
            switch (currentState)
            {
                case BossState.Idle:
                    yield return StartCoroutine(Chasing());
                    break;

                case BossState.CreateKid:
                    yield return StartCoroutine(CreateKid());
                    break;

                case BossState.FireOne:
                    yield return StartCoroutine(FireOne());
                    break;

                case BossState.FireArc:
                    yield return StartCoroutine(FireArc());
                    break;
                case BossState.FireCircle:
                    yield return StartCoroutine(FireCircle());
                    break;

            }
        }

    }
    IEnumerator Chasing()
    {
        speed = 2f;
        yield return null;
    }
    //잡몹 소환
    IEnumerator CreateKid()
    {
        //float interval = 5f;
        //timer += Time.deltaTime;
        //if (timer >= interval)
        //{
        //    timer = 0f;
        //    Instantiate(bossKid, transform.position + Vector3.right, Quaternion.identity);
        //    Instantiate(bossKid, transform.position + Vector3.down, Quaternion.identity);
        //    Instantiate(bossKid, transform.position + Vector3.left, Quaternion.identity);
        //    yield return null;
        //}
        Vector3[] spawnOffsets = { Vector3.right, Vector3.down, Vector3.left };

        foreach (Vector3 offset in spawnOffsets)
        {
            GameObject bossKidInstance = Instantiate(bossKid, transform.position + offset, Quaternion.identity);
            bossKidInstance.transform.SetParent(transform);
        }

        yield return new WaitForSeconds(3.0f);

    }

    //총알
    IEnumerator FireOne()
    {
        StartCoroutine(CreateKid());
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet;
            bullet = Instantiate(bossBullet, transform);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = GameManager.instance.player.transform.position - transform.position;
            bulletRigid.AddForce(dir.normalized * 10f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.0f);
    }

    //부채꼴 발사
    IEnumerator FireArc()
    {
        StartCoroutine(CreateKid());
        float arcAngle = 90;
        float fireSpeed = 5f;
        float fireRotateSpeed = 200f;
        int bulletCount = 6;
        Vector3 directionToTarget = (GameManager.instance.player.transform.position - transform.position).normalized;

        float baseAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        float angleStep = arcAngle / (bulletCount - 1);
        float angleStart = baseAngle - arcAngle / 2f;
        float angle = angleStart;

        for (int i = 0; i < bulletCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad); // 방향 계산
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 dir = new Vector3(dirX, dirY, 0f).normalized;

            GameObject bullet = Instantiate(bossBullet, transform.position, Quaternion.identity);
            bullet.transform.up = dir;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = dir * fireSpeed;
            rb.angularVelocity = fireRotateSpeed;

            angle += angleStep;
        }

        yield return new WaitForSeconds(3.0f);
    }

    //원형 발사
    IEnumerator FireCircle()
    {
        StartCoroutine(CreateKid());
        speed = 0f;
        float angleStep = 360f / 30;
        float angle = 0f;
        float fireSpeed = 5f;
        int bulletCount = 30;

        for (int i = 0; i < bulletCount; i++)
        {
            float dirX = Mathf.Sin(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Cos(angle * Mathf.Deg2Rad);

            Vector3 dir = new Vector3(dirX, dirY, 0f).normalized;

            GameObject bullet = Instantiate(bossBullet, transform.position, Quaternion.identity);
            bullet.transform.up = dir;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = dir * fireSpeed;

            angle += angleStep;
        }
        speed = 2f;

        yield return new WaitForSeconds(5.0f);
    }
}
