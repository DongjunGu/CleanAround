using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    // Start is called before the first frame update
    void LateUpdate()
    {
        if (GameManager.instance.player.inputVector.x != 0)
        {
            if (GameManager.instance.player.inputVector.x > 0)
                transform.localRotation = Quaternion.Euler(0f, 180, 0f);
            else
                transform.localRotation = Quaternion.identity;
        }
    }
}
