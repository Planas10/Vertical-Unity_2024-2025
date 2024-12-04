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

    public Transform _crouchTransform;
    private Vector3 _crouchPosition;
    private Vector3 _standPosition;


    public float _jumpForce = 10f;
    private Vector3 _direction;


    private void Awake()
    {
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
    }
    private void HorizontalMovement (){
        Vector2 movementInput = _inputs.actions["Move"].ReadValue<Vector2>();
        if (movementInput != Vector2.zero) {

            if (_inputs.actions["Run"].IsPressed()) {
                _speed = 10f;
            }
            else {
                _speed = 5f;
            }
            _direction = new Vector3(movementInput.x * _speed, _rb.velocity.y, movementInput.y * _speed);

            _rb.velocity = _direction;
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) {
            _grounded = true;
            _doubleJump = false;
            _rb.drag = 3;
        }
    }
}
