using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : BaseEnemy
{
    public List<Transform> waypoints;

    private int _currentPoint = 0;

    public CanvasManager _canvasManager;

    public AudioSource gunShoot;

    void Update()
    {
        if (!_canvasManager.gameIsPaused)
        {
            if (_IA.isStopped && !_reloading)
            {
                _IA.isStopped = false;
            }
            if (CheckPlayer())
            {
                PlayerSpotted();
            }
            else
            {
                if (PlayerDetected && !_reloading)
                {
                    PlayerDetected = false;
                    _anim.SetBool("InRange", false);
                    _anim.SetBool("PlayerDetected", false);
                    _IA.isStopped = false;
                    _IA.speed = 3.5f;
                    _IA.SetDestination(waypoints[_currentPoint].position);
                }
                if (_detected) {
                    _detected = false;
                }
            }
            if (!PlayerDetected)
            {
                Patrol();
            }
        }
        else
        {
            _IA.isStopped = true;
        }
    }

    private void Patrol() {
        if (Vector3.Distance(transform.position, waypoints[_currentPoint].transform.position) < 1)
        {
            if (_currentPoint < waypoints.Count - 1)
            {
                _currentPoint++;
            }
            else{
                _currentPoint = 0;
            }
            _IA.SetDestination(waypoints[_currentPoint].position);
        }
    }
    public override void PlayerSpotted()
    {
        base.PlayerSpotted();
        if (PlayerDetected)
        {
            _IA.speed = 6;
        }
    }

    public override void Attack() {
        if (canAttack)
        {
            _anim.SetBool("Attacking", true);
            _anim.SetBool("Reload", true);
            gunShoot.Play();
            playerController.TakeDamage();
            canAttack = false;
            _reloading = true;
            base.Attack();
        }
    }
}
