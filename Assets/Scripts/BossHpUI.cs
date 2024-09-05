using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpUI : MonoBehaviour
{
    RectTransform rect;
    GameObject bossAlien;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {
        bossAlien = GameObject.Find("BossAlien(Clone)");
        if(bossAlien != null)
        {
            rect.position = Camera.main.WorldToScreenPoint(bossAlien.transform.position + Vector3.up * 5.5f);
        }
        
    }
}
