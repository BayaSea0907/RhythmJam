using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTransform : MonoBehaviour
{
    [SerializeField] private Transform defaultTransform;

    private void Awake()
    {
        defaultTransform = this.transform;
    }
        
    private void Update ()
    {
        this.transform.position = defaultTransform.position;
        this.transform.rotation = defaultTransform.rotation;
    }
}
