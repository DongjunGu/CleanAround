using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    float activeTime;
    PlayerController player;
    Transform magnetic;

    bool isVacuumWorking = false;
    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            case 1:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
            case 4:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    Throw();
                    timer = 0f;
                }
                break;
            case 5:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    Swipe();
                    timer = 0f;
                }
                break;
            case 7:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    Detergent();
                    timer = 0f;
                }
                break;
            case 12:                
                timer += Time.deltaTime;
                if (timer > 1f)
                {
                    activeTime += 0.05f;
                    if (activeTime > 1f && !isVacuumWorking)
                    {
                        activeTime = 0f;
                        StartCoroutine(VacuumWorking());                        
                    }
                    timer = 0f;
                }
                break;
            case 13:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    UpgradedSwipe();
                    timer = 0f;
                }
                break;
            case 14:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    UpgradedDetergent();
                    timer = 0f;
                }
                break;
            default:
                break;
        }

        if (Input.GetButtonDown("Jump")) //Test
        {
            LevelUp(10, 1);
        }
    }

    IEnumerator VacuumWorking()
    {
        isVacuumWorking = true;
        Transform vacuum = player.transform.Find("Weapon12").GetChild(0);
        vacuum.gameObject.SetActive(true);
        //애니메이션 trigger
        GameManager.instance.player.playerAnimator.SetTrigger("onVacuum");
        //collider
        GameManager.instance.player.capCollider.enabled = false;
        Transform magn = player.transform.Find("Weapon6");
        if (magn != null && magn.childCount > 0)
        {
            magn.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        }
        yield return new WaitForSeconds(5f);
        if (magn != null && magn.childCount > 0)
        {
            magn.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        }
        GameManager.instance.player.capCollider.enabled = true;
        vacuum.gameObject.SetActive(false);
        GameManager.instance.player.playerAnimator.ResetTrigger("onVacuum");
        isVacuumWorking = false;
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;
        if (id == 0) //근접무기
            Batch();

    }
    public void Init(ItemData data)
    {
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.prtojectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }
        switch (id)
        {
            case 0:
                speed = -150;
                Batch();
                break;
            case 1:
                speed = 0.5f;
                break;
            case 4:
                Throw();
                speed = 4f;
                break;
            case 5:
                Swipe();
                speed = 3f;
                break;
            case 6:
                Magnetic();
                break;
            case 7:
                speed = 0.03f;
                Detergent();
                break;
            case 10:
                {
                    ActiveRobotVacuum();
                    ActiveRobotVacuum();
                    ActiveRobotVacuum();
                    ActiveRobotVacuum();
                }   
                break;
            case 12:
                {
                    ActiveVacuum(); //청소기 활성화
                }
                break;
            case 13:
                UpgradedSwipe();
                speed = 3f;
                break;
            case 14:
                speed = 0.03f;
                UpgradedDetergent();
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            Vector3 rotateVec = Vector3.forward * 360 * index / count; //회전 시킨 후
            bullet.Rotate(rotateVec);
            bullet.Translate(bullet.up * 2.0f, Space.World); //이동

            bullet.GetComponent<Bullet>().Init(damage, -10, Vector3.zero, ItemData.ItemType.Melee); //관통
        }
    }
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir, ItemData.ItemType.Range);
    }

    void Throw()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform;

            float randomAngle = Random.Range(0f, 360f);
            float radian = randomAngle * Mathf.Deg2Rad;
            Vector2 targetPosition = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized; //360도 랜덤
            bullet.position = transform.position;
            bullet.Translate(bullet.up * 0.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -10, targetPosition, ItemData.ItemType.Garbage);
            StartCoroutine(Deactivate(bullet.gameObject, 2.0f));
        }

    }
    IEnumerator Deactivate(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

    void Swipe()
    {
        Transform bullet;
        if (transform.childCount > 0)
        {
            bullet = transform.GetChild(0);
        }
        else
        {
            bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform;
        }
        bullet.gameObject.SetActive(true);
        bullet.localPosition = Vector3.zero;
        bullet.GetComponent<Bullet>().Init(damage, -10, Vector3.zero, ItemData.ItemType.FeatherDuster);
        StartCoroutine(Deactivate(bullet.gameObject, 0.5f));
    }
    void UpgradedSwipe()
    {
        Transform bullet;
        if (transform.childCount > 0)
        {
            bullet = transform.GetChild(0);
        }
        else
        {
            bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform;
        }
        bullet.gameObject.SetActive(true);
        bullet.localPosition = Vector3.zero;
        bullet.GetComponent<Bullet>().Init(damage, -10, Vector3.zero, ItemData.ItemType.UpgradeFeather);
        StartCoroutine(Deactivate(bullet.gameObject, 0.5f));
    }
    void Magnetic()
    {
        if (transform.childCount != 0)
            return;
        else
        {
            magnetic = GameManager.instance.pool.Get(prefabId).transform;
            magnetic.parent = transform;
        }
        magnetic.localPosition = new Vector3(0f, -0.2f, 0f);
        
    }
    public void MagneticUpgrade()
    {
        magnetic.GetComponent<Magnetic>().Init(damage);
    }
    void Detergent()
    {
        Transform bullet;
        bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = GameManager.instance.player.transform.position - new Vector3(0f,0.3f,0f);
        bullet.GetComponent<Bullet>().Init(damage, -10, Vector3.zero, ItemData.ItemType.Detergent);
        StartCoroutine(Deactivate(bullet.gameObject, 1.5f));
    }
    void UpgradedDetergent()
    {
        Transform formalDetergent = player.transform.Find("Weapon7");
        if(formalDetergent != null)
        {
            formalDetergent.gameObject.SetActive(false);
        }
        

        Transform bullet;
        bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = GameManager.instance.player.transform.position - new Vector3(0f, 0.3f, 0f);
        bullet.GetComponent<Bullet>().Init(damage, -10, Vector3.zero, ItemData.ItemType.UpgradeDetergent);
        StartCoroutine(Deactivate(bullet.gameObject, 3.0f));
    }
    void ActiveRobotVacuum()
    {
        Transform bullet;
        bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.parent = transform;
        bullet.localPosition = Vector3.zero;
        bullet.GetComponent<Bullet>().Init(damage, -10, Vector3.zero, ItemData.ItemType.RobotVacuum);
    }
   void ActiveVacuum()
    {
        Transform bullet;
        bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.parent = transform;
        bullet.GetComponent<Bullet>().Init(damage, -10, Vector3.zero, ItemData.ItemType.Vacuum);
        bullet.gameObject.SetActive(false);
        GameManager.instance.vacuumGauge.SetActive(true);
    }
}

