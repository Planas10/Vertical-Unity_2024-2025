using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StaticEnemy : BaseEnemy
{
    public float rotationSpeed = 2f;
    private float targetAngle;

    private Vector3 _startpos;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool rotatingToTarget = true;

    public GameObject rotateTarget;

    public CanvasManager _canvasManager;

    void Start()
    {
        _startpos = transform.position;
        BeginRotation();
    }

    void Update()
    {
        if (!_canvasManager.gameIsPaused)
        {
            if (_IA.isStopped)
            {
                _IA.isStopped = false;
            }
            if (CheckPlayer())
            {
                PlayerSpotted();
            }
            else
            {
                if (PlayerDetected)
                {
                    PlayerDetected = false;
                    _anim.SetBool("PlayerDetected", false);
                    _anim.SetBool("Returning", true);
                    _IA.SetDestination(_startpos);
                }
            }
            if (!PlayerDetected && Vector3.Distance(transform.position, _startpos) < 0.1f)
            {
                Look();
                _anim.SetBool("Returning", false);
            }
        }
        else {
            _IA.isStopped = true;
        }
    }

    private void BeginRotation() {
        targetAngle = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z).y - 45f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
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
