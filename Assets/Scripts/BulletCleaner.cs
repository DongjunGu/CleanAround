using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCleaner : MonoBehaviour
{
    void OnEnable()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
         
        List<GameObject> clones = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "DetergentAsh(Clone)")
            {
                clones.Add(obj);
            }
        }

        foreach (GameObject clone in clones)
        {
            clone.SetActive(false);
        }
    }
}
