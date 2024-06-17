using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Object")]
    public PlayerController player;
    public PoolManager pool;
    public bool isLive;
    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public float exp;
    public float increaseExp;
    public int[] nextExp = { 3, 10, 20, 45, 70, 120, 200, 300, 450, 600 };
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime;
    public LevelUI levelUI;
    public Result gameoverUI;
    public GameObject enemyCleaner;
    void Awake()
    {
        instance = this;
    }
    public void GameStart()
    {
        health = maxHealth;

        //TODO 기본무기
        levelUI.Select(0);

        isLive = true;
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        StartCoroutine(GameOverCorou());
    }
    IEnumerator GameOverCorou()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        gameoverUI.gameObject.SetActive(true);
        gameoverUI.Fail();
        Time.timeScale = 0;
    }
    public void GameEnd()
    {
        StartCoroutine(GameEndCorou());
    }
    IEnumerator GameEndCorou()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameoverUI.gameObject.SetActive(true);
        gameoverUI.Victory();
        Time.timeScale = 0;
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }
    void Update()
    {
        if ((!isLive))
            return;
        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameEnd();
        }
    }
    public void GetExp()
    {
        if (!isLive)
            return;

        exp += (1 * increaseExp);
        if(exp >= nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            level++;
            levelUI.PopUI();
            exp = 0;
        }
    }

}
