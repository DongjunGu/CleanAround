using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVacuum : MonoBehaviour
{
    [SerializeField] float changeDirectionTime = 0.5f; // 방향 변경 시간
    public float radius = 5.0f;
    public float speed = 2.0f;
    private float angle;
    private Vector2 direction;
    private float timer;

    void Start()
    {
        angle = Random.Range(0f, 2f * Mathf.PI);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        timer = changeDirectionTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeDirection();
            timer = changeDirectionTime;
        }

        MoveInCircle();
    }
    void ChangeDirection() //방향 변경
    {
        angle = Random.Range(0f, 2f * Mathf.PI);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    void MoveInCircle() //원형 범위
    {
        Vector2 centerPosition = GameManager.instance.player.transform.position;
        Vector2 newPosition = centerPosition + direction * radius;

        transform.position = Vector2.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
    }
}
