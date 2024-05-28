using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * -250f, Space.World);}
    }
