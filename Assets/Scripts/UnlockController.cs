using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockController : MonoBehaviour
{
    public GameObject[] lockObject;
    public GameObject[] unlockObject;

    enum Unlock { JrBig, MsGreen }
    Unlock[] unlocks;

    void Awake()
    {
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
            PlayerPrefs.SetInt(unlock.ToString(), 1);
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
}
