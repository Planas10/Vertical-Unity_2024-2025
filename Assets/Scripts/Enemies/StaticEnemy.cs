using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StaticEnemy : BaseEnemy
{
    public float rotationSpeed = 2f;
    private float targetAngle;

    private Vector3 _startpos;

    private Quaternion InitRot;
    private bool correctRot;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool rotatingToTarget = true;

    public Transform POVtransform;

    public CanvasManager _canvasManager;

    void Start()
    {
        InitRot = transform.rotation;
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
                    _anim.SetBool("InRange", false);
                    _anim.SetBool("Attacking", false);
                    _IA.SetDestination(_startpos);
                }
            }
            if (!PlayerDetected && Vector3.Distance(transform.position, _startpos) < 0.1f)
            {
                Look();
                _anim.SetBool("AtStart", true);
            }
        }
        else {
            _IA.isStopped = true;
        }
    }

    private void BeginRotation() {
        targetAngle = Quaternion.Euler(POVtransform.rotation.x, POVtransform.rotation.y, POVtransform.rotation.z).y - 45f;
        startRotation = POVtransform.rotation;
        targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    private void Look() {
        if (!correctRot) {
            transform.rotation = InitRot;
            correctRot = true;
        }
        if (rotatingToTarget){ POVtransform.rotation = Quaternion.Slerp(POVtransform.rotation, targetRotation, rotationSpeed * Time.deltaTime); }
        else{ POVtransform.rotation = Quaternion.Slerp(POVtransform.rotation, startRotation, rotationSpeed * Time.deltaTime); }

        if (Quaternion.Angle(POVtransform.rotation, targetRotation) < 1f && rotatingToTarget){
            rotatingToTarget = false;
        }
        else if (Quaternion.Angle(POVtransform.rotation, startRotation) < 1f && !rotatingToTarget){
            rotatingToTarget = true;
        }
    }

    public void DoDamage() {
        playerController.TakeDamage();
    }

    public override void PlayerSpotted()
    {
        base.PlayerSpotted();
        correctRot = false;
        _anim.SetBool("AtStart", false);
    }

    public override void Attack()
    {
        if (canAttack)
        {
            _anim.SetBool("Attacking", true);
            canAttack = false;
            base.Attack();
        }
    }
}
