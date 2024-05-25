using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClean : MonoBehaviour
{
    void OnEnable()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }
}
