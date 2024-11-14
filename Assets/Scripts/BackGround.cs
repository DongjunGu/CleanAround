using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed = 2f;
    private float duration;
    private float timer;
    private bool isMoving = false;
    void Update()
    {
        if (isMoving)
        {
            if (timer < duration)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
                timer += Time.deltaTime;
            }
            else
            {
                isMoving = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void MoveUpForSeconds(float moveDuration)
    {
        duration = moveDuration;
        timer = 0f;
        isMoving = true;
    }
}
