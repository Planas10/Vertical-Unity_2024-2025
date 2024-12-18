using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : BaseEnemy
{
    public List<Transform> waypoints;

    private int _currentPoint = 0;

    public CanvasManager _canvasManager;

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
                    _IA.isStopped = false;
                    _IA.speed = 3.5f;
                    _IA.SetDestination(waypoints[_currentPoint].position);
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
}
