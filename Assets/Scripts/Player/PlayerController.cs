using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera _Cam;

    private CharacterController _ch;

    private void Awake()
    {
        _ch = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
    }
}
