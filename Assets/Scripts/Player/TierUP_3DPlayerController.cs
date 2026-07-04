using System;
using UnityEngine;

public class TierUP_3DPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody_Player;
    [SerializeField] private Transform Transform_GroundCheck;
    [SerializeField] private LayerMask LayerMask_Ground;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _runSpeed = 15f;
    [SerializeField] private float _rotateSpeed = 700f;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _groundCheckRadius = 0.2f;

    public event Action<EntityAnimState> OnStateChanged;
    public event Action<float> OnMoveSpeedChanged;

    private EntityAnimState _currentState;

    private Vector2 _moveInput;
    private Vector3 _moveDirection;

    private bool _isRunning;
    private bool _isGrounded;
    private bool _jumpInput;

    private void Awake()
    {
        if (Rigidbody_Player == null)
        {
            Rigidbody_Player = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        ReadInput();
        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        CheckGround();
        MovePlayer();
        RotatePlayer();
        Jump();
    }

    private void ReadInput()
    {
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            _jumpInput = true;
        }
    }

    private void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(Transform_GroundCheck.position, _groundCheckRadius, LayerMask_Ground);
    }

    private void MovePlayer()
    {
        float currentSpeed = _moveSpeed;
        if (_isRunning == true)
        {
            currentSpeed = _runSpeed;
        }

        Vector3 moveVelocity = _moveDirection * currentSpeed;
        moveVelocity.y = Rigidbody_Player.linearVelocity.y;
        Rigidbody_Player.linearVelocity = moveVelocity;
    }

    private void RotatePlayer()
    {
        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        if (_jumpInput == true && _isGrounded == true)
        {
            Vector3 jumpVelocity = Rigidbody_Player.linearVelocity;
            jumpVelocity.y = _jumpForce;
            Rigidbody_Player.linearVelocity = jumpVelocity;
        }
        _jumpInput = false;
    }

    private void UpdateAnimationState()
    {
        float moveMagnitude = Mathf.Clamp01(_moveInput.magnitude);
        float animSpeed = moveMagnitude;
        if (_isRunning == true)
        {
            animSpeed = moveMagnitude * 2f;
        }
        OnMoveSpeedChanged?.Invoke(animSpeed);

        EntityAnimState newState = DecideState();
        if (newState != _currentState)
        {
            _currentState = newState;
            OnStateChanged?.Invoke(_currentState);
        }
    }

    private EntityAnimState DecideState()
    {
        if (_isGrounded == true)
        {
            return EntityAnimState.None;
        }

        if (Rigidbody_Player.linearVelocity.y > 0.1f)
        {
            return EntityAnimState.Jump;
        }

        return EntityAnimState.Fall;
    }

    private void OnDrawGizmos()
    {
        if (Transform_GroundCheck == null)
        {
            return;
        }

        if (_isGrounded == true)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(Transform_GroundCheck.position, _groundCheckRadius);
    }
}