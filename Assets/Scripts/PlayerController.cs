using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region References
    Input_Manager _playerInput;
    CharacterController _characterController;
    [SerializeField] Animator _animator;
    #endregion
    #region player movement variables
    Vector2 _movementInput;
    Vector3 _movement;
    bool _isMovementPressed;
    [SerializeField] float _movementSpeed = 5f;

    float _rotationSpeed = 30f;
    public Transform RespawnPoint;
    //jumping 
    bool _isJumpPressed = false;
    bool _isJumping = false;
    [SerializeField] float _jumpHeight = 5f;
    bool _isjumpAnimation = false;
    bool _canDoubleJump;

    //Dashing
    bool _isDashPressed = false;
    bool _isDashing = false;
    Vector3 _movementDirection; 
    [SerializeField] float _dashDuration = 0.5f;
    [SerializeField] float _dashSpeed = 100f;
    bool _canDash = false;
    [SerializeField] private TrailRenderer _tr;
    //crouching
    bool _isCrouchingPressed = false;
    bool _isCrouching = false;
    #endregion
    #region Animation hashs
    int _isWalkingHash;
    //int _isRunningHash;
    int _isJumpingHash;
    int _isDoubleJumpingHash;
    int _isCrouchingHash;
    #endregion
    private void Awake()
    {
        _playerInput = new Input_Manager();
        _characterController = GetComponent<CharacterController>();
        //set the player input callbacks
        _playerInput.Player.Move.started += OnMovementInput;
        _playerInput.Player.Move.canceled += OnMovementInput;
        _playerInput.Player.Move.performed += OnMovementInput;
        _playerInput.Player.Jump.started += OnJump;
        _playerInput.Player.Jump.canceled += OnJump;
        _playerInput.Player.Dash.started += OnDash;
        _playerInput.Player.Dash.canceled += OnDash;
        _playerInput.Player.Crouch.started += OnCrouch;
        _playerInput.Player.Crouch.canceled += OnCrouch;
        //animation hash
        _isWalkingHash = Animator.StringToHash("isWalking");
        //_isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _isDoubleJumpingHash = Animator.StringToHash("isDoubleJumping");
        _isCrouchingHash = Animator.StringToHash("isCrouching");
        _canDash = true;
    }

    void OnCrouch(InputAction.CallbackContext ctx)
    {
        _isCrouchingPressed = ctx.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        _isJumpPressed = ctx.ReadValueAsButton();
    }
    void OnDash(InputAction.CallbackContext ctx)
    {
        _isDashPressed= ctx.ReadValueAsButton();
    }
    void HandleRotation()
    {
        Vector3 lookAtDirection;

        lookAtDirection.x = _movement.x;
        lookAtDirection.y = 0f;
        lookAtDirection.z = 0f;

        Quaternion rotation = transform.rotation;

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookAtDirection);
            transform.rotation = Quaternion.Slerp(rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    void OnMovementInput(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
        _movement.x = _movementInput.x * _movementSpeed;
        //_movement.z = _movementInput.y * _movementSpeed;

        _isMovementPressed = _movementInput.x != 0;
    }

    void HandleGravity()
    {
        bool isFalling = _movement.y <= 0f;
        float fallMultiplier = 3f;
        if (_characterController.isGrounded)
        {
            if (_isjumpAnimation)
            {
                _animator.SetBool(_isJumpingHash, false);
                _isjumpAnimation = false;
            }
            _animator.SetBool(_isDoubleJumpingHash, false);
            float groundedGravity = -.05f;
            _movement.y = groundedGravity;
            if (!_isJumpPressed)
            {
                _canDoubleJump = false;
            }
        }
        else if (isFalling)
        {
            float gravity = -9.81f * fallMultiplier;
            _movement.y += gravity * Time.deltaTime;
        }
        else
        {
            float gravity = -9.81f;
            _movement.y += gravity * Time.deltaTime;
        }
    }
    void HandleAnimation()
    {
        bool isWalking = _animator.GetBool(_isWalkingHash);
        //bool isRunning = _animator.GetBool(_isRunningHash);

        if (_isMovementPressed && !isWalking)
        {
            _animator.SetBool("isWalking", true);
        }
        else if (!_isMovementPressed && isWalking)
        {
            _animator.SetBool("isWalking", false);
        }
    }

    void HandleJump()
    {
        if (_isJumpPressed)
        {
            if (_characterController.isGrounded && !_isJumping)
            {
                _animator.SetBool(_isJumpingHash, true);
                _isjumpAnimation = true;
                _isJumping = true;
                float jumpVelocity = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics.gravity.y));
                _movement.y = jumpVelocity;
                _canDoubleJump = true;
            }
            if (!_isJumping && _canDoubleJump)
            {
                _animator.SetBool(_isDoubleJumpingHash, true);
                _isJumping = true;
                float jumpVelocity = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics.gravity.y));
                _movement.y = jumpVelocity;
                _canDoubleJump = false;
            }
        }
        if (!_isJumpPressed && _isJumping)
        {
            _isJumping = false;
        }
    }
    IEnumerator HandleDash()
    {
        _isDashing = true;
        _tr.emitting= true;
        _movementDirection = transform.forward * _dashSpeed;
        _characterController.Move(_movementDirection* Time.deltaTime);
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
        _canDash = false;
        _tr.emitting = false;
        yield return new WaitForSeconds(0.5f);
        _canDash= true;
    }
    void HandleCrouch()
    {
      if (_isCrouchingPressed)
        {
            if (_characterController.isGrounded)
            {
                _isCrouching = true;
                _characterController.height = 0.5f;
                _animator.SetBool(_isCrouchingHash, true);
                _characterController.center = new Vector3(0, -0.6f, 0);
            }
        }
        else
        {
            _isCrouching = false;
            _characterController.height = 2f;
            _animator.SetBool(_isCrouchingHash, false);
            _characterController.center = new Vector3(0,0,0);
        }
    }

    private void Update()
    {
        HandleCrouch();
        HandleAnimation();
        if (!_isCrouching)
        {
            HandleRotation();
            _characterController.Move(_movement * Time.deltaTime);
            if (_isDashPressed && _canDash)
            {
                StartCoroutine(HandleDash());
            }


            HandleGravity();
            HandleJump();
        }
        
    }

    #region Enable/Disable
    private void OnEnable()
    {
        _playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }
    #endregion
}
