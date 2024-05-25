using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] resultPages;

    public void Victory()
    {
        resultPages[0].SetActive(true);
    }
    public void Fail()
    {
        resultPages[1].SetActive(true);
    }
}
