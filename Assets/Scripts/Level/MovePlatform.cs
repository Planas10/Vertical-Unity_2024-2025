using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Vector3 initPos;
    public Vector3 targetPos;

    public Vector3 offset;

    public bool movingToTarget;

    private bool ableToMove = true;

    public float speed;
    public float waitTime = 1;

    private void Start()
    {
        initPos = transform.position;
        targetPos = initPos + offset;
    }

    private void Update()
    {
        if (ableToMove) {
            if (movingToTarget)
            {
                MoveTowardsPosition(targetPos);
            }
            else {
                MoveTowardsPosition(initPos);
            }
        }
    }

    public void MoveTowardsPosition(Vector3 position) {
        if (Vector3.Distance(transform.position, position) < 0.1f)
        {
            movingToTarget = !movingToTarget;
            ableToMove = false;
            Invoke("StartMovment", waitTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        }
    }
    public void StartMovment() {
        ableToMove = true;
    }
}
