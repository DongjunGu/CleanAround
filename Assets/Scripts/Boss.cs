using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject bossKid;
    public GameObject bossBullet;
    public GameObject bossHpUI;
    public GameObject spawnparticle;
    public GameObject jumpparticle;
    public GameObject[] Arms;
    public GameObject[] Ranges;
    public GameObject[] WallRanges;
    Transform wall;
    bool isMoving = false;
    bool isRangeClear = false;
    void Start()
    {
        StartCoroutine(BossPattern());
        damage = 35f;
    }
    protected override void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        anim.SetBool("Dead", false);
        isHit = false;
        health = maxHealth;
        GameObject newBossHpUI = Instantiate(bossHpUI, GameManager.instance.HUDUI.transform);
        newBossHpUI.transform.SetSiblingIndex(0);
    }
    protected override void Move()
    {
        if (isMoving)
            return;

        base.Move();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;

        anim.SetTrigger("Hit");

        if (health <= 0)
        {
            anim.SetBool("Dead", true);

            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
        }
    }
    public enum BossState
    {
        Idle,
        CreateKid,
        FireOne,
        FireArc,
        FireCircle,
        ArmAttack,
        RangeAttack,
        Dead
    }

    public BossState currentState = BossState.Idle;

    void Update()
    {
        if (health > maxHealth * 0.75f)
            currentState = BossState.CreateKid;
        if (health <= maxHealth * 0.75 && health > maxHealth * 0.5f)
            currentState = BossState.FireArc;
        if (health <= maxHealth * 0.5f && health > maxHealth * 0.25f)
            currentState = BossState.ArmAttack;
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
        speed = 2f;
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
                case BossState.ArmAttack:
                    yield return StartCoroutine(ArmAttack());
                    break;

            }
        }

    }
    IEnumerator Chasing()
    {
        yield return null;
    }
    //잡몹 소환
    IEnumerator CreateKid()
    {
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
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CreateKid());
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(bossBullet, transform.position, Quaternion.identity);
            bullet.transform.SetParent(transform);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = GameManager.instance.player.transform.position - transform.position;
            bulletRigid.AddForce(dir.normalized * 10f, ForceMode2D.Impulse);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.BossThrow);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.0f);

    }

    //부채꼴 발사
    IEnumerator FireArc()
    {        
        StartCoroutine(CreateKid());
        float arcAngle = 90;
        float fireSpeed = 8f;
        float fireRotateSpeed = 200f;
        int bulletCount = 6;
        Vector3 directionToTarget = (GameManager.instance.player.transform.position - transform.position).normalized;

        float baseAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        float angleStep = arcAngle / (bulletCount - 1);
        float angleStart = baseAngle - arcAngle / 2f;
        float angle = angleStart;
        speed = 0f;
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BossArc);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BossArc);
        for (int i = 0; i < bulletCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad); // 방향 계산
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 dir = new Vector3(dirX, dirY, 0f).normalized;

            GameObject bullet = Instantiate(bossBullet, transform.position, Quaternion.identity);
            bullet.transform.SetParent(transform);
            bullet.transform.up = dir;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = dir * fireSpeed;
            rb.angularVelocity = fireRotateSpeed;

            angle += angleStep;
        }
        speed = 2f;
        yield return new WaitForSeconds(5.0f);
        
    }

    //원형 발사
    IEnumerator FireCircle()
    {
        if(!isRangeClear)
            yield return StartCoroutine(RangeAttack());

        isRangeClear = true;
        speed = 0f;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(1f);

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
            bullet.transform.SetParent(transform);
            bullet.transform.up = dir;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = dir * fireSpeed;

            angle += angleStep;
        }
        yield return new WaitForSeconds(0.5f);
        speed = 2f;

        yield return new WaitForSeconds(5.0f);
    }
    IEnumerator ArmAttack()
    {
        speed = 0f;
        TriggerAnimation(Ranges, "Notice");
        yield return new WaitForSeconds(1f);
        TriggerAnimation(Arms, "ArmAttack");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Scratch);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Scratch);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Scratch);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Scratch);
        ToggleColliders(Ranges, true);
        yield return new WaitForSeconds(1.5f);
        ToggleColliders(Ranges, false);

        speed = 2f;
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FireOne());
        yield return new WaitForSeconds(5.0f);
    }

    IEnumerator RangeAttack()
    {
        wall = GameObject.Find("Wall(Clone)").transform;
        GameObject[] squares = new GameObject[]
        {
        wall.Find("Square1").gameObject,
        wall.Find("Square2").gameObject,
        wall.Find("Square3").gameObject,
        wall.Find("Square4").gameObject
        };

        // 이동 관련 설정
        isMoving = true;
        col.enabled = false;
        while (Vector3.Distance(transform.position, wall.position) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, wall.position, 4f * Time.deltaTime);
            yield return null;
        }

        transform.position = wall.position;
        anim.SetBool("Walk", false);

        // 패턴 실행
        yield return ExecutePattern(new[] { squares[0], squares[3] }, 5f, 3.5f); // 11시, 5시
        yield return ExecutePattern(new[] { squares[1], squares[2] }, 5f, 3.5f); // 1시, 7시
        yield return ExecutePattern(new[] { squares[1], squares[2], squares[3] }, 5f); // 1시, 5시, 7시
        yield return ExecutePattern(new[] { squares[0], squares[1], squares[2] }, 5f); // 11시, 5시, 7시

        // 이동 완료
        anim.SetBool("Walk", true);
        isMoving = false;
        col.enabled = true;
    }
    private IEnumerator ExecutePattern(GameObject[] activeSquares, float activeDuration, float delayAfter = 0f)
    {
        // 활성화
        foreach (var square in activeSquares)
        {
            square.SetActive(true);
        }

        yield return new WaitForSeconds(activeDuration);

        // 비활성화
        foreach (var square in activeSquares)
        {
            square.SetActive(false);
        }

        // 딜레이
        if (delayAfter > 0)
        {
            yield return new WaitForSeconds(delayAfter);
        }
    }
    private void TriggerAnimation(GameObject[] objects, string triggerName)
    {
        foreach (var obj in objects)
        {
            obj.GetComponent<Animator>().SetTrigger(triggerName);
        }
    }

    private void ToggleColliders(GameObject[] objects, bool isEnabled)
    {
        foreach (var obj in objects)
        {
            obj.GetComponent<CapsuleCollider2D>().enabled = isEnabled;
        }
    }
    public IEnumerator SpawnParticle()
    {
        spawnparticle.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BossJump);
        yield return new WaitForSeconds(1.0f);
        spawnparticle.SetActive(false);
    }
    public IEnumerator JumpParticle()
    {
        jumpparticle.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BossJump);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BossJump);
        yield return new WaitForSeconds(1.0f);
        jumpparticle.SetActive(false);
    }
}
