using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject bossKid;
    public GameObject bossBullet;
    
    private float timer = 0f;
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
    }

    public enum BossState
    {
        Idle,
        Chase,
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
        if (health <= 950 && health > 900)
            currentState = BossState.CreateKid;
        if (health <= 900 && health > 850)
            currentState = BossState.FireOne;
        if (health <= 850 && health > 800)
            currentState = BossState.FireArc;
        if (health <= 800 && health > 750)
            currentState = BossState.FireCircle;
        if(health <= 0)
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
        float interval = 5f;
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            Instantiate(bossKid, transform.position + Vector3.right, Quaternion.identity);
            Instantiate(bossKid, transform.position + Vector3.down, Quaternion.identity);
            Instantiate(bossKid, transform.position + Vector3.left, Quaternion.identity);
            yield return null;
        }
    }
    //평타

    //총알
    IEnumerator FireOne()
    {
        GameObject bullet = Instantiate(bossBullet, transform);
        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dir = GameManager.instance.player.transform.position - transform.position;
        bulletRigid.AddForce(dir.normalized * 10f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(3.0f);

    }
    IEnumerator FireArc()
    {
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

    IEnumerator FireCircle()
    {
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
    //범위
}
