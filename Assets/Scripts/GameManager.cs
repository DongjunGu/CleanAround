using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Object")]
    public PlayerController player;
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
    public int playerId;
    public int[] nextExp = { 3, 10, 20, 45, 70, 120, 200, 300, 450, 600 };
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime;
    public LevelUI levelUI;
    public GameObject HUDUI;
    public Transform joystickUI;
    public GameObject stopButton;
    public ResumeButton resumeButton;
    public Result gameoverUI;
    public GameObject enemyCleaner;
    public GameObject bulletCleaner;
    public GameObject bossController;
    public GameObject bossGrid;
    public BackGround bgImage;
    public bool bossClear = false;
    private bool bossCouroutine = false;
    void Awake()
    {
        instance = this;
    }
    public void GameStart(int id)
    {
        id = playerId;
        health = maxHealth;

        levelUI.Select(id % 2);

        
        //Time.timeScale = 1;
        
        player.playerSpeed *= CharacterSwitch.Speed;
        maxHealth *= CharacterSwitch.Health;
        health = maxHealth;

        StartCoroutine(GameStartCorou());

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        stopButton.SetActive(true);
        
    }
    IEnumerator GameStartCorou()
    {
        player.GetComponent<Animator>().SetTrigger("RunDown");
        bgImage.MoveUpForSeconds(4);
        yield return new WaitForSeconds(4f);
        Cursor.visible = enabled;
        isLive = true;
        Time.timeScale = 1;
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
        bossClear = true;
        joystickUI.localScale = Vector3.zero;
        AudioManager.instance.PlayBgm(false);
    }
    IEnumerator GameEndCorou()
    {
        yield return new WaitForSeconds(1.5f);
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
        ResumeGame();
    }
    public void StopGame()
    {
        Time.timeScale = 0;
        joystickUI.localScale = Vector3.zero;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        joystickUI.localScale = Vector3.one;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            resumeButton.ChangeImage();
        }

        if ((!isLive))
            return;
        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            //���� ��ȯ
            BossStage();

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
    public void SelectSFX()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

}
