using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;
    float timer;
    Rigidbody2D rigid;
    SpriteRenderer spriteRend;
    ItemData.ItemType bulletName;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int per, Vector3 dir, ItemData.ItemType itemName)
    {
        this.damage = damage;
        this.per = per;

        //if(per >= 0)
        //{
        //    rigid.velocity = dir * 10f; //속도
        //}
        switch (itemName)
        {
            case ItemData.ItemType.Range:
                rigid.velocity = dir * 10f;
                break;
            case ItemData.ItemType.Garbage:
                rigid.velocity = dir * 3.5f;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) //관통
    {
        if (!collision.CompareTag("Enemy") || per == -10)
            return;
        per--;
        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D collision) //총알 삭제
    {
        if (!collision.CompareTag("Area") || per == -10)
            return;
        gameObject.SetActive(false);
    }
}
