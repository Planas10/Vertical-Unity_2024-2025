using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : BaseEnemy
{

    public Transform _startPos;
    public Transform _endPos;

    public List<Transform> waypoints;

    private int _currentPoint = 0;

    void Update()
    {
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
            Debug.Log(_currentPoint);
            _IA.SetDestination(waypoints[_currentPoint].position);
        }
    }
    public override void PlayerSpotted()
    {
        base.PlayerSpotted();
        if (PlayerDetected)
        {
            _IA.speed = 4;
        }
    }
}
