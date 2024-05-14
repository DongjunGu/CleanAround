using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public static GameManager instance;
    
    void Awake()
    {
        instance = this;
    }
}
