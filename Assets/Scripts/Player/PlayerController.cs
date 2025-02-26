using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CanvasManager _canvasManager;

    public AudioSource _landSound;
    public AudioSource _hookSound;
    public AudioSource _jumpSound;
    public AudioSource _footStepSound;
    public AudioSource _slideSound;

    public Camera _cam;
    public PlayerInput _inputs;
    public LineRenderer _lr;

    private CharacterController _cc;
    private CapsuleCollider _cC;

    public float _gravity;
    public float _Mgravity;

    public float _speed;
    public bool _grounded = true;
    public bool _doubleJump;

    public bool _running;
    public float _slideForce;
    public bool _sliding;

    public float _crouchHeight;
    public float _crouchCenter;

    private bool _isCrouched;

    public float _hookCooldown;
    public float _hookRange;

    public float _interactRange;

    private float xRotation;
    private float yRotation;

    public bool _falling;

    private bool _grappling;

    private Vector3 _checkpoint;

    private Vector3 playerVelocity;

    public Transform _crouchTransform;
    public Transform HookSpawn;
    private Vector3 _crouchPosition;
    private Vector3 _standPosition;

    public bool _hasHook;

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

        win = false;
        Cursor.lockState = CursorLockMode.Locked;
        _checkpoint = transform.position;
        _standPosition = _cam.transform.localPosition;
        _crouchPosition = _crouchTransform.localPosition;
        _inputs = GetComponent<PlayerInput>();
        _cc = GetComponent<CharacterController>();
        _cC = GetComponent<CapsuleCollider>();
        if (!_hasHook)
        {
            _lr.enabled = false;
        }
    }

    private void Update()
    {
        //Check if grounded
        if ((_cc.collisionFlags & CollisionFlags.Below) != 0 && !_grounded)
        {
            _grounded = true;
        } else if (_grounded && (_cc.collisionFlags & CollisionFlags.Below) == 0 && _cc.velocity != Vector3.zero) {
            _grounded = false;
            _footStepSound.Stop();
            _falling = false;
        }

        if (_grounded && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        if (!_canvasManager.gameIsPaused)
        {
            if (!_grappling)
            {
                HorizontalMovement();
                Jump();
            }
            if (_grounded)
            {
                if (!_running)
                {
                    Crouch();
                }
                else {
                    Slide();
                }
            }
            CheckInteractable();
            CheckCoyoteTime();
            Hook();
            CheckHidden();
            Interact();
            if (_inputs.actions["AntiGroundBug"].WasPressedThisFrame())
            {
                _grounded = true;
            }
            if (_inputs.actions["GoToStart"].WasPressedThisFrame())
            {
                transform.position = _checkpoint;
            }

            //variables sin tipo predefinido
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
                //coyote timer
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
        if (_sliding) { return; }
        if (_inputs.actions["Move"].WasPressedThisFrame() && _grounded && _cc.velocity != Vector3.zero && !_footStepSound.isPlaying) {
            _footStepSound.Play();
        }
        if (_inputs.actions["Run"].IsPressed() && !_isCrouched)
        {
            _speed = 7f;
            _running = true;
        }
        else
        {
            _speed = 5f;
            _running = false;
        }
        if (!_grounded && !_running && _cc.velocity.y < 0) {
            _speed = 5f;
        }
        Vector3 MoveDirection = Quaternion.Euler(0f, _cam.transform.eulerAngles.y, 0f) * new Vector3(Input.GetAxis("Horizontal"), _Mgravity, Input.GetAxis("Vertical"));
        MoveDirection = new Vector3(MoveDirection.x, _Mgravity, MoveDirection.z);
        MoveDirection = transform.TransformDirection(MoveDirection);
        _cc.Move(MoveDirection * _speed * Time.deltaTime);

        if (_grounded && !_footStepSound.isPlaying && _cc.velocity != Vector3.zero)
        {
            _footStepSound.Play();
        } else if (_grounded && _footStepSound.isPlaying && _cc.velocity == Vector3.zero) {
            _footStepSound.Stop();
        }
    }
    
    //Salto
    private void Jump()
    {
        if (_inputs.actions["Jump"].WasPressedThisFrame()) {
            //Si esta en el suelo
            if (_grounded)
            {
                _jumpSound.Play();
                playerVelocity.y += Mathf.Sqrt(_jumpForce * -2.0f * _gravity);
                _hasJumped = true;
                _grounded = false;
                _footStepSound.Stop();
            }
            //Si esta usando el gancho
            if (_grappling)
            {
                _jumpSound.Play();
                StopCoroutine(_hookCoroutine);

                _grappling = false;
            }
        }
        playerVelocity.y += _gravity * Time.deltaTime;
        _cc.Move(playerVelocity * Time.deltaTime);
    }

    //Agacharse
    private void Crouch() {
        if (_inputs.actions["Crouch"].WasPressedThisFrame()) { 
            if (_isCrouched){
                //Si esta agachado no se puede correr
                RaycastHit hit;
                //Si no puede levantarse no se levanta
                if (Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f)){ return; }               
                _cc.height = 2;
                _cc.center = new Vector3(0, 0, 0);
                _cam.transform.localPosition = _standPosition;
            }
            else {
                _cc.height = _crouchHeight;
                _cc.center = new Vector3(0, _crouchCenter, 0);
                _cam.transform.localPosition = _crouchPosition;
            }
            _isCrouched = !_isCrouched;
        }
    }

    private void Slide() {
        if (_inputs.actions["Crouch"].WasPressedThisFrame() && !_sliding && !_isCrouched) {
            _sliding = true;
            _footStepSound.Stop();
            _slideSound.Play();
            _cc.height = _crouchHeight;
            _cc.center = new Vector3(0, _crouchCenter, 0);
            _cam.transform.localPosition = _crouchPosition;
            _isCrouched = true;
            Vector3 MoveDirection = Quaternion.Euler(0f, _cam.transform.eulerAngles.y, 0f) * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            MoveDirection = transform.TransformDirection(MoveDirection);
            _cc.Move(MoveDirection * (_speed * 2f) * Time.deltaTime);
            StartCoroutine(SlideEnd());
        }
    }

    private IEnumerator SlideEnd() {
        float slideTime = 0.8f;
        Vector3 MoveDirection = Quaternion.Euler(0f, _cam.transform.eulerAngles.y, 0f) * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MoveDirection = transform.TransformDirection(MoveDirection);
        while (slideTime > 0f) {
            yield return null;
            _cc.Move(MoveDirection * ((_speed * 5f) * slideTime) * Time.deltaTime);
            slideTime -= Time.deltaTime;
        }
        _sliding = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f)) { yield break; }
        _cc.height = 2;
        _cc.center = new Vector3(0, 0, 0);
        _cam.transform.localPosition = _standPosition;
        _isCrouched = false;
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
                    _lr.enabled = true;
                    _lr.SetPosition(1, hit.point);
                    _doubleJump = false;
                    _grappling = true;
                    _grounded = false;
                    _footStepSound.Stop();
                    Vector3 adjustedPos = hit.collider.transform.position + Vector3.up * 5;
                    Vector3 direction = adjustedPos - transform.position;
                    _cc.Move(direction * (_speed / 3f) * Time.deltaTime);
                    if (_hookCoroutine != null)
                    {
                        StopCoroutine(_hookCoroutine);
                    }
                    _hookCoroutine = StartCoroutine(HookGravity(direction, adjustedPos));
                }
            }
        }
    }

    //Corutina para el gancho
    private IEnumerator HookGravity(Vector3 _direction, Vector3 _destination)
    {
        float timerPostHook = 0.5f;
        Vector3 DestinationInertia = _cam.transform.forward;
        yield return null;
        while (Vector3.Distance(transform.position, _destination) > 1)
        {
            _cc.Move(_direction * (_speed / 3f) * Time.deltaTime);
            yield return null;
        }
        _grappling = false;
        while (timerPostHook >= 0f) {
            yield return null;
            _cc.Move(DestinationInertia * _speed * Time.deltaTime);
            timerPostHook -= Time.deltaTime;
        }
        //Cancelar el movimiento de gancho saltando mientras te arrastra
        _cc.Move((_cc.velocity / 3f) * Time.deltaTime);
        _lr.enabled = false;
        _grappling = false;
    }

    //Detectar interactuable
    public bool CheckInteractable() {
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }

    public GameObject placeholder;
    public GameObject GetInteractable() {
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                return hit.collider.gameObject;
            }
            else { return placeholder; }
        }
        else { return placeholder; }
    }

    //Funcion para los interactuables
    public void Interact() {
        if (_inputs.actions["Interact"].WasPressedThisFrame()) {
            if(CheckInteractable() && !GetInteractable().GetComponent<Button>().activated) {
                GetInteractable().GetComponent<Button>().activated = true;
            }
        }
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
            _hasJumped = false;
            _doubleJump = false;
        }
        if (_grappling)
        {
            _lr.enabled = false;
            StopCoroutine(_hookCoroutine);
            _cc.Move(Vector3.zero);
            _grappling = false;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _falling = true;
            if (!_grappling)
            {
                //_rb.drag = 0.3f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wind"))
        {
            _Mgravity = -1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wind"))
        {
            _Mgravity = 0f;
        }
    }
}
