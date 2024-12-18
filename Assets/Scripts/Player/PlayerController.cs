using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CanvasManager _canvasManager;

    public AudioSource _landSound;
    public AudioSource _hookSound;
    public AudioSource _jumpSound;

    public Camera _cam;
    public PlayerInput _inputs;
    public LineRenderer _lr;
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

    public bool _falling;

    private bool _grappling;

    private Vector3 _checkpoint;

    public Transform _crouchTransform;
    public Transform HookSpawn;
    private Vector3 _crouchPosition;
    private Vector3 _standPosition;

    public bool _hasHook;
    public bool _hasDoubleJump;

    public float _jumpForce;

    private Coroutine _hookCoroutine;

    public bool reset;
    public bool hidden;
    public bool atrapado;

    public bool win;

    private const float Coyote_Time = 0.5f;
    private float timeSinceLastGroundTouch = Mathf.Infinity;
    public bool _hasJumped;

    private void Awake()
    {
        reset = false;
        atrapado = false;

        _hasHook = true;
        _hasDoubleJump = true;

        win = false;
        Cursor.lockState = CursorLockMode.Locked;
        _checkpoint = transform.position;
        _standPosition = _cam.transform.localPosition;
        _crouchPosition = _crouchTransform.localPosition;
        _inputs = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _cC = GetComponent<CapsuleCollider>();
        CheckAvailable();
        if (!_hasHook)
        {
            _lr.enabled = false;
        }
    }

    private void Update()
    {
        if (!_canvasManager.gameIsPaused)
        {
            if (_grounded)
            {
                HorizontalMovement();
                Crouch();
            }
            CheckCoyoteTime();
            Jump();
            Hook();
            CheckHidden();

            if (_inputs.actions["AntiGroundBug"].WasPressedThisFrame())
            {
                _grounded = true;
            }
            if (_inputs.actions["GoToStart"].WasPressedThisFrame())
            {
                transform.position = _checkpoint;
            }
        }
    }

    private void CheckAvailable() {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            _hasDoubleJump = false;
            _hasHook = false;
        }
    }

    private void LateUpdate()
    {
        if (!_canvasManager.gameIsPaused)
        {

            var lookDirection = _inputs.actions["CamMovment"].ReadValue<Vector2>();
            var mouse = lookDirection * Time.deltaTime * 20f;

            this.xRotation -= mouse.y;
            this.yRotation += mouse.x;
            this.xRotation = Mathf.Clamp(this.xRotation, -45f, 60f);

            _cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            if (_grappling)
            {
                _lr.SetPosition(0, HookSpawn.position);
            }
        }
    }

    //Coyote time
    private void CheckCoyoteTime() {
        if (_grounded)
        {
            timeSinceLastGroundTouch = 0f;
        }
        else {
            if (timeSinceLastGroundTouch < Coyote_Time)
            {
                // "Run" coyote timer
                timeSinceLastGroundTouch += Time.deltaTime;
            }
        }
        if (timeSinceLastGroundTouch < Coyote_Time && !_hasJumped)
        {
            _grounded = true;
        }
    }

    //Movimiento horizontal
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
    
    //Salto
    private void Jump()
    {
        if (_inputs.actions["Jump"].WasPressedThisFrame()) {
            //Si esta en el suelo
            if (_grounded)
            {
                _jumpSound.Play();
                _hasJumped = true;
                _grounded = false;
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _rb.drag = 0.3f;
            }
            //Si no ha usado doble salto
            else if (!_doubleJump && _hasDoubleJump) {
                _jumpSound.Play();
                _doubleJump = true;
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
            //Si esta usando el gancho
            if (_grappling)
            {
                _jumpSound.Play();
                StopCoroutine(_hookCoroutine);
                CancelGrappleState();
                _grappling = false;
            }
        }
    }

    //Agacharse
    private void Crouch() {
        if (_inputs.actions["Crouch"].WasPressedThisFrame()) { 
            if (_isCrouched){
                //Si esta agachado no se puede correr
                RaycastHit hit;
                //Si no puede levantarse no se levanta
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

    //Comprovar si el jugador esta agachado bajo algo que puede esconderlo
    private void CheckHidden() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f) && _isCrouched)
        {
            hidden = true;
        }
        else
        {
            hidden = false;
        }
    }

    //Gancho
    private void Hook() {
        if (_inputs.actions["Hook"].WasPressedThisFrame() && _hasHook){
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _hookRange)) {
                if (hit.collider.CompareTag("Grappable")) {
                    _hookSound.Play();
                    //Cambiar lo necesario para asegurar un movimiento fluido durante el gancho
                    _lr.enabled = true;
                    _lr.SetPosition(1, hit.point);
                    _doubleJump = false;
                    _grappling = true;
                    _grounded = false;
                    _rb.velocity = Vector3.zero;
                    _rb.useGravity = false;
                    _rb.drag = 0;
                    Vector3 adjustedPos = hit.point + Vector3.up * 1;
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

    //Cancelar el movimiento de gancho saltando mientras te arrastra
    private void CancelGrappleState() {
        _lr.enabled = false;
        _rb.useGravity = true;
        _rb.drag = 0.3f;
        _rb.velocity *= 0.5f;
    }

    //Gestor de colisiones
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (_falling)
            {
                _landSound.Play();
                _falling = false;
            }
            _grounded = true;
            _hasJumped = false;
            _doubleJump = false;
            _rb.drag = 3;
        }
        if (_grappling)
        {
            _lr.enabled = false;
            StopCoroutine(_hookCoroutine);
            CancelGrappleState();
            _grappling = false;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _falling = true;
            _grounded = false;
            if (!_grappling)
            {
                _rb.drag = 0.3f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reseter") || other.gameObject.CompareTag("EnemyCapture"))
        {
            if (SceneManager.GetActiveScene().name == "Sandbox")
            {
                SceneManager.LoadScene("Sandbox");
            }
            transform.position = _checkpoint;
        }
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            _checkpoint = other.transform.position;
        }
        if (other.gameObject.CompareTag("WinCondition"))
        {
            win = true;
        }
    }

    //Corutina para el gancho
    private IEnumerator HookGravity(Vector3 _destination) {
        yield return null;
        while (Vector3.Distance(transform.position, _destination) > 1)
        {
            yield return null;
        }
        CancelGrappleState();
    }
}
