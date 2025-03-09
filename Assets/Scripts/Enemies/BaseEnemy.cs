using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public PlayerController playerController;
    public LevelManager levelManager;

    public Transform PlayerPos;
    public NavMeshAgent _IA;

    private float detectionAngle = 45f;

    public bool PlayerDetected;
    public float DetectDistance;
    public float DetectHeight;

    public float attackDistance;

    public AudioSource _detectAudio;
    public Animator _anim;
    public bool _detected = false;

    public bool _reloading;

    public bool canAttack = true;

    public float attackSpeed;
    public float attackTimer;

    private void Update()
    {
        if (!CheckPlayer() && attackTimer < 1f) {
            attackTimer = 1f;
        }
    }

    //calcular el angulo de vision
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

    //comprovar si el jugador esta dentro del rango de vision
    protected bool CheckPlayer() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, PlayerPos.position - transform.position, out hit, DetectDistance))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                _IA.isStopped = false;
                return false;
            }
        }
        if (CheckAngle()) {
            if (CheckDistance()) {
                if (Vector3.Distance(transform.position, PlayerPos.position) <= attackDistance)
                {
                    _anim.SetBool("InRange", true);
                    _IA.isStopped = true;
                    Attack();

                }
                else { 
                    if (!_reloading)
                    {
                        _IA.isStopped = false;
                    }
                    _anim.SetBool("InRange", false);
                    _anim.SetBool("Attacking", false);
                }
                return true;
            }
        }
        if (!_reloading)
        {
            _IA.isStopped = false;
        }

        return false;
    }

    public void StartReload() {
        _IA.isStopped = true;
        _reloading = true;
        _anim.SetBool("Attacking", false);
    }
    public void EndReload() {
        _reloading = false;
        _anim.SetBool("Reload", false);
    }
    private IEnumerator Reload(float timer) {
        yield return new WaitForSeconds(timer);
        canAttack = true;
    }

    //comprovar la distancia entre el jugador y el enemigo asi como la diferencia de altura
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, PlayerPos.position) <= DetectDistance && Mathf.Abs(transform.position.y - PlayerPos.position.y) < DetectHeight;
    }

    public virtual void PlayerSpotted() {
        if (!_detected)
        {
            _detectAudio.Play();
            _detected = true;
        }
        GoToPosition(PlayerPos.position);
        PlayerDetected = true;
        _anim.SetBool("PlayerDetected", true);
    }

    //persecucion del jugador
    public IEnumerator ChasePlayer() {
        yield return null;
        while (PlayerDetected) {
            yield return new WaitForSeconds(0.2f);
            if (CheckDistance())
            {
                if (Vector3.Distance(transform.position, PlayerPos.position) < 1)
                {
                
                }
                else {
                    GoToPosition(PlayerPos.position);
                }
            }
        }

    }

    public void GoToPosition(Vector3 destination) {
        if(!_reloading)
            _IA.SetDestination(destination);
    }

    public virtual void Attack() {
        StartCoroutine(Reload(attackTimer));
    }
}
