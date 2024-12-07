using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{

    Ray hit;
    public Transform PlayerPos;

    private float detectionAngle = 45f;

    public bool PlayerDetected;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + transform.right) * 5f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward - transform.right) * 5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, PlayerPos.position);
    }
    private bool CheckAngle(Ray ray) {

        Vector3 rayDirection = ray.direction;
        Vector3 rayDistance = ray.origin - PlayerPos.position;

        var lookPercentage = Vector3.Angle(rayDirection, rayDistance);

        if (lookPercentage > detectionAngle)
        {
            return false;
        }
        else {
            return true;
        }

        //Debug.Log(lookPercentage);

    }

    protected bool CheckPlayer() {
        return CheckAngle(hit = new Ray(transform.position, transform.position + transform.forward));
    }

    public virtual void PlayerSpotted() {
        transform.LookAt(PlayerPos.position);
        PlayerDetected = true;
    }
}
