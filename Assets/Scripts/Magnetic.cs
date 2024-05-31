using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    Vector3 originScale;
    Vector3 updatedScale;
    public void Init(float scale)
    {
        originScale = GetComponent<Transform>().localScale;
        transform.localScale = originScale + new Vector3(scale, scale, 0f);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Exp"))
        {
            if (collision.gameObject.activeSelf)
            {
                collision.transform.position = Vector3.MoveTowards(collision.transform.position, GameManager.instance.player.transform.position, 10f * Time.deltaTime);
            }
        }
    }
}
