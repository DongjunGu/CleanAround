using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Object")]
    public PlayerController player;
    public Camera mainCamera;
    public Spawner spawner;
    public PoolManager pool;
    public bool isLive;
    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public float exp;
    public float increaseExp;
    public GameObject vacuumGauge;
    public int[] nextExp = { 3, 10, 20, 45, 70, 120, 200, 300, 450, 600 };
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime;
    public LevelUI levelUI;
    public GameObject HUDUI;
    public Transform joystickUI;
    public Result gameoverUI;
    public GameObject enemyCleaner;
    public GameObject bossController;

    private bool bossCouroutine = false;
    void Awake()
    {
        instance = this;
    }
    public void GameStart()
    {
        health = maxHealth;

        levelUI.Select(0);

        isLive = true;
        Time.timeScale = 1;
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        joystickUI.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        StartCoroutine(GameOverCorou());
        joystickUI.localScale = Vector3.zero;
        AudioManager.instance.PlayBgm(false);
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
        joystickUI.localScale = Vector3.zero;
        AudioManager.instance.PlayBgm(false);
    }
    IEnumerator GameEndCorou()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameoverUI.gameObject.SetActive(true);
        gameoverUI.Victory();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        Time.timeScale = 0;
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }
    public void StopGame()
    {
        Time.timeScale = 0;
        AudioManager.instance.PlayBgm(false);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayBgm(true);
    }
    void Update()
    {
        if ((!isLive))
            return;
        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            //보스 소환
            BossStage();

            //GameEnd();
        }
    }
    void BossStage()
    {
        StartCoroutine(BossStageCorou());
    }

    IEnumerator BossStageCorou()
    {
        enemyCleaner.SetActive(true);
        spawner.gameObject.SetActive(false);

        if (!bossCouroutine)
        {
            Instantiate(bossController);
            bossCouroutine = true;
        }
        yield return null;        
    }
    public void GetExp()
    {
        if (!isLive)
            return;

        exp += (1 * increaseExp);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Exp);
        if (exp >= nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            level++;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup);
            levelUI.PopUI();
            exp = 0;
        }
    }

}
