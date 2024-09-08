using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(GameManager.instance.player != null)
        {
            Vector2 pos = GameManager.instance.player.transform.position + new Vector3(0f, -1f, 0f);
            rb.MovePosition(pos);
        }
        if (GameManager.instance.player.inputVector.x != 0)
        {
            if (GameManager.instance.player.inputVector.x > 0)
                transform.localRotation = Quaternion.Euler(0f, 180, 0f);
            else
                transform.localRotation = Quaternion.identity;
        }
    }
}
