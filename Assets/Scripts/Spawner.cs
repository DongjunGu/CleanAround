using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;
    float timer;
    int level;
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        
    }
    void Start()
    {
        levelTime = GameManager.instance.maxGameTime / spawnData.Length; //몬스터 주기
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            AutoSpawn();
            timer = 0;
        }
    }

    void AutoSpawn()
    {
        GameObject enemy =  GameManager.instance.pool.Get(level);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int health;
    public float speed;
    public float damage;
    [SerializeField] string memo;
}
