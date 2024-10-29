using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool GroundCheck;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _jumpForce = 5f; 
    private Rigidbody _rigidbody;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumping;

    private float _cameraPitch = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && GroundCheck)
        {
            Jump();
        }
    }

    private void Move()
    {
        Vector3 forwardMovement = transform.forward * _moveInput.y * _moveSpeed * Time.deltaTime;
        Vector3 strafeMovement = transform.right * _moveInput.x * _moveSpeed * Time.deltaTime;
        transform.position += forwardMovement + strafeMovement;
    }

    private void Rotate()
    {
        float yaw = _lookInput.x * _rotationSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);

        _cameraPitch -= _lookInput.y * _rotationSpeed * Time.deltaTime;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -45f, 45f);
        cameraTransform.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        GroundCheck = false; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCheck = true; 
        }
    }
}
