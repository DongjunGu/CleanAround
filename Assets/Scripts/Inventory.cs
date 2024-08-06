using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Vector3 currentScale = transform.localScale;

            if(currentScale == Vector3.one)
            {
                transform.localScale = Vector3.zero;
            }
            else if(currentScale == Vector3.zero)
            {
                transform.localScale = Vector3.one;
            }
        }
    }
}
