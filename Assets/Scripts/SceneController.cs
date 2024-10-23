using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int lastAnimatorIndex = PlayerPrefs.GetInt("LastAnimator", 1);
    }
}
