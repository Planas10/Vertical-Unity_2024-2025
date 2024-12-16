using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera _cam;
    public PlayerInput _inputs;
    private Rigidbody _rb;
    private CapsuleCollider _cC;

    public float _speed = 0.05f;
    public bool _grounded = true;
    public bool _doubleJump;

    public float _crouchHeight;
    public float _crouchCenter;

    private bool _isCrouched;

    public float _hookCooldown;
    public float _hookRange;

    private float xRotation;
    private float yRotation;

    private bool _grappling;

    public Transform _crouchTransform;
    private Vector3 _crouchPosition;
    private Vector3 _standPosition;


    public float _jumpForce = 12f;

    private Coroutine _hookCoroutine;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _standPosition = _cam.transform.localPosition;
        _crouchPosition = _crouchTransform.localPosition;
        _inputs = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _cC = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (_grounded)
        {
            HorizontalMovement();
            Crouch();
        }
        Jump();
        Hook();
    }

    private void LateUpdate()
    {
        var lookDirection = _inputs.actions["CamMovment"].ReadValue<Vector2>();
        var mouse = lookDirection * Time.deltaTime * 20f;

        this.xRotation -= mouse.y;
        this.yRotation += mouse.x;
        this.xRotation = Mathf.Clamp(this.xRotation, -45f, 60f);

        _cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    //Movimiento WASD
    private void HorizontalMovement (){
        Vector2 movementInput = _inputs.actions["Move"].ReadValue<Vector2>();
        if (_inputs.actions["Run"].IsPressed() && !_isCrouched)
        {
            _speed = 10f;
        }
        else
        {
            _speed = 5f;
        }
        Vector3 direction = (movementInput.x * _cam.transform.right + movementInput.y * _cam.transform.forward).normalized;
        direction = new Vector3(direction.x * _speed, _rb.velocity.y, direction.z * _speed);
        _rb.velocity = direction;
    }

    private void Jump()
    {
        if (_inputs.actions["Jump"].WasPressedThisFrame()) {
            if (_grounded)
            {
                _grounded = false;
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _rb.drag = 0.3f;
            }
            else if (!_doubleJump) {
                _doubleJump = true;
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
            if (_grappling)
            {
                StopCoroutine(_hookCoroutine);
                CancelGrappleState();
                _grappling = false;
            }
        }
    }

    private void Crouch() {
        if (_inputs.actions["Crouch"].WasPressedThisFrame()) { 
            if (_isCrouched){
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f)){ return; }               
                _cC.height = 2;
                _cC.center = new Vector3(0, 0, 0);
                _cam.transform.localPosition = _standPosition;
            }
            else {
                _cC.height = _crouchHeight;
                _cC.center = new Vector3(0, _crouchCenter, 0);
                _cam.transform.localPosition = _crouchPosition;
            }
            _isCrouched = !_isCrouched;
        }
    }

    private void Hook() {
        if (_inputs.actions["Hook"].WasPressedThisFrame()){
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _hookRange)) {
                if (hit.collider.CompareTag("Grappable")) {
                    _doubleJump = false;
                    _grappling = true;
                    _grounded = false;
                    _rb.velocity = Vector3.zero;
                    _rb.useGravity = false;
                    _rb.drag = 0;
                    Vector3 adjustedPos = hit.point + Vector3.up * 3;
                    _rb.AddRelativeForce(adjustedPos - transform.position, ForceMode.Impulse);
                    if (_hookCoroutine != null)
                    {
                        StopCoroutine(_hookCoroutine);
                    }
                    _hookCoroutine = StartCoroutine(HookGravity(adjustedPos));
                }
            }
        }
    }

    private void CancelGrappleState() {
        _rb.useGravity = true;
        _rb.drag = 0.3f;
        _rb.velocity *= 0.5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _grounded = true;
            _doubleJump = false;
            _rb.drag = 3;
        }
        if (_grappling)
        {
            StopCoroutine(_hookCoroutine);
            CancelGrappleState();
            _grappling = false;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _grounded = false;
            if (!_grappling)
            {
                _rb.drag = 0.3f;
            }
        }
    }

    private IEnumerator HookGravity(Vector3 _destination) {
        yield return null;
        while (Vector3.Distance(transform.position, _destination) > 5)
        {
            yield return null;
        }
        CancelGrappleState();
    }
}
