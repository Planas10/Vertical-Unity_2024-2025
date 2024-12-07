using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StaticEnemy : BaseEnemy
{
    public float rotationSpeed = 2f;
    private float targetAngle;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool rotatingToTarget = true;

    void Start()
    {
        targetAngle = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z).y - 45f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    void Update()
    {
        if (CheckPlayer()) {
            PlayerSpotted();
        }
        if (!PlayerDetected)
        {
            Look();
        }
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

    public override void PlayerSpotted()
    {
        base.PlayerSpotted();
    }
}
