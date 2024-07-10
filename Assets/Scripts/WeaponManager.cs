using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public PlayerController player;
    void Update()
    {
        WeaponRobotVacuum();


    }

    void WeaponRobotVacuum()
    {
        Transform weapon4 = player.transform.Find("Weapon4");
        Transform weapon10 = player.transform.Find("Weapon10");

        if (weapon10 != null)
        {
            if (weapon10.gameObject.activeSelf)
            {
                weapon4.gameObject.SetActive(false);
            }
        }
    }
}
