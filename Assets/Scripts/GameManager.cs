using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Object")]
    public PlayerController player;
    public PoolManager pool;
    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 10, 30, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public LevelUI levelUI;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        health = maxHealth;

        //TODO
        levelUI.Select(0);
    }
    void Update()
    {
        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
    public void GetExp()
    {
        exp++;
        if(exp == nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            level++;
            levelUI.PopUI();
            exp = 0;
        }
    }

}
