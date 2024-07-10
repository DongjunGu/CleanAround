using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumGauge : MonoBehaviour
{
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position + Vector3.up * 0.6f);
    }
}
