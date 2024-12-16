using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public Transform PlayerPos;
    public NavMeshAgent _IA;

    private float detectionAngle = 45f;

    public bool PlayerDetected;
    public float DetectDistance;
    public float DetectHeight;


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
    private bool CheckAngle() {

        Vector3 Direction = transform.forward;
        Vector3 Distance = PlayerPos.position - transform.position;

        var lookPercentage = Vector3.Angle(Direction, Distance);

        if (lookPercentage > detectionAngle)
        {
            return false;
        }
        else {
            return true;
        }


    }

    protected bool CheckPlayer() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, PlayerPos.position - transform.position, out hit, DetectDistance, 3))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                return false;
            }
        }
        return CheckAngle() && CheckDistance();
    }

    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, PlayerPos.position) <= DetectDistance && Mathf.Abs(transform.position.y - PlayerPos.position.y) < DetectHeight;
    }

    public virtual void PlayerSpotted() {
        _IA.SetDestination(PlayerPos.position);
        PlayerDetected = true;
    }

    public IEnumerator ChasePlayer() {
        yield return null;
        while (PlayerDetected) {
            yield return new WaitForSeconds(0.2f);
            if (CheckDistance())
            {
                if (Vector3.Distance(transform.position, PlayerPos.position) < 3)
                {
                    Debug.Log("atrapado");
                }
                else { 
                    _IA.SetDestination(PlayerPos.position);
                }
            }
        }

    }
}
