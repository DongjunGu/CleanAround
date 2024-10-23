using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockController : MonoBehaviour
{
    public GameObject[] lockObject;
    public GameObject[] unlockObject;
    public GameObject unlockUI;
    public Image unlockImageBG;
    public Image unlockImage;
    public Sprite Charc2;
    public Sprite Charc3;
    enum Unlock { JrBig, MsGreen }
    Unlock[] unlocks;
    Animator uiAnim;
    void Awake()
    {
        uiAnim = unlockUI.GetComponent<Animator>();
        uiAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        unlocks = (Unlock[])Enum.GetValues(typeof(Unlock));
        if (!PlayerPrefs.HasKey("CharcData"))
        {
            Init();
        }
    }
    void Start()
    {
        UnlockCharacter();
    }

    void Init()
    {
        PlayerPrefs.SetInt("CharcData", 1);
        foreach (Unlock unlock in unlocks)
        {
            PlayerPrefs.SetInt(unlock.ToString(), 0);
        }
    }

    void UnlockCharacter()
    {
        for(int i = 0; i < lockObject.Length; i++)
        {
            string unlockName = unlocks[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(unlockName) == 1;
            lockObject[i].SetActive(!isUnlock);
            unlockObject[i].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach(Unlock unlock in unlocks)
        {
            CheckUnlock(unlock);
        }
    }

    void CheckUnlock(Unlock unlock)
    {
        bool isUnlock = false;

        switch (unlock)
        {
            case Unlock.JrBig:
                isUnlock = GameManager.instance.gameTime >= 180f;
                break;
            case Unlock.MsGreen:
                isUnlock = GameManager.instance.bossClear == true;
                break;
        }
        if(isUnlock && PlayerPrefs.GetInt(unlock.ToString()) == 0)
        {
            PlayerPrefs.SetInt(unlock.ToString(), 1);
            ChangeImage(unlock);
            uiAnim.SetTrigger("Move");
        }
    }
    void ChangeImage(Unlock unlock)
    {
        if (unlock == Unlock.JrBig)
        {
            unlockImageBG.color = Color.red;
            unlockImage.sprite = Charc2;
        }

        else if (unlock == Unlock.MsGreen)
        {
            unlockImageBG.color = Color.green;
            unlockImage.sprite = Charc3;
        }
    }
}
