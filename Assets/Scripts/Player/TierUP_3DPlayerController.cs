using UnityEngine;

public class TierUP_3DPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody_Player;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _runSpeed = 15f;
    [SerializeField] private float _rotateSpeed = 700f;

    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private bool _isRunning;

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
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void ReadInput()
    {
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    private void MovePlayer()
    {
        float currentSpeed = _moveSpeed;
        if (_isRunning == true)
        {
            currentSpeed = _runSpeed;
        }

        Vector3 velocity = _moveDirection * currentSpeed;
        velocity.y = Rigidbody_Player.linearVelocity.y;
        Rigidbody_Player.linearVelocity = velocity;
    }

    private void RotatePlayer()
    {
        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            Rigidbody_Player.rotation = Quaternion.RotateTowards(Rigidbody_Player.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);
        }
    }
}