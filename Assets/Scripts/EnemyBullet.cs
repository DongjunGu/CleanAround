using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            GameManager.instance.health -= 10;

        Invoke("DestroyBullet", 0.1f);
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}