using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health}
    public InfoType type;

    Text levelText;
    Slider expSlider;

    void Awake()
    {
        levelText = GetComponent<Text>();
        expSlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                expSlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                levelText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); //string ��ȯ

                break;
            case InfoType.Kill:

                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                levelText.text = string.Format("{0:D2}:{1:D2}",min,sec); //string ��ȯ
                break;
            case InfoType.Health:
                float curHp = GameManager.instance.health;
                float maxHp = GameManager.instance.maxHealth;
                expSlider.value = curHp / maxHp;
                break;
        }
    }
}
