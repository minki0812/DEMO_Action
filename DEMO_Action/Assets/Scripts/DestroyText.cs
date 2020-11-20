using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyText : MonoBehaviour
{
    void Start()
    {
        Invoke("OnDestroy", 10f);
    }

    void OnDestroy()
    {
        Destroy(gameObject);
    }
}
