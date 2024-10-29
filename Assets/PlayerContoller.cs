using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _jumpForce = 5f; 
    private float _cameraPitch = 0f;

    private Rigidbody _rigidbody;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumping;
    public bool GroundCheck;

    private void Awake()
    {   
        Cursor.visible = false;
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

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftButton();
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

    private void LeftButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); 
        if (Physics.Raycast(ray, out RaycastHit hit, 100f)) 
        {
            Debug.Log("Hit: " + hit.collider.name); 
            
            // if (hit.collider.CompareTag("Interactable")) // Проверка тега объекта
            // {

            // }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCheck = true; 
        }
    }
}
