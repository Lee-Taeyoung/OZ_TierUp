using UnityEngine;

public class TierUP_3DPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody_Player;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _runSpeed = 50f;

    private Vector2 _moveInput;
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
    }

    private void ReadInput()
    {
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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

        Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = Rigidbody_Player.linearVelocity.y;
        Rigidbody_Player.linearVelocity = velocity;
    }
}