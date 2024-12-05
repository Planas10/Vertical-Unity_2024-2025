using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    public float rotationSpeed = 2f;
    public float targetAngle = 45f;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool rotatingToTarget = true;

    void Start()
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    void Update()
    {
        Look();
    }

    private void Look() {

        if (rotatingToTarget){ transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); }
        else{ transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, rotationSpeed * Time.deltaTime); }

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f && rotatingToTarget){
            rotatingToTarget = false;
        }
        else if (Quaternion.Angle(transform.rotation, startRotation) < 1f && !rotatingToTarget){
            rotatingToTarget = true;
        }
    }
}
