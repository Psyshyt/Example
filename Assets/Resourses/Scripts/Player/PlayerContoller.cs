using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude > _moveSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _moveSpeed;
        }
        
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
    // Получаем движение на основе ввода
    Vector3 forwardMovement = transform.forward * _moveInput.y * _moveSpeed;
    Vector3 strafeMovement = transform.right * _moveInput.x * _moveSpeed;

    // Комбинируем движения
    Vector3 movement = forwardMovement + strafeMovement;

    // Устанавливаем скорость Rigidbody
    if (GroundCheck)
    {
        _rigidbody.velocity = new Vector3(movement.x, _rigidbody.velocity.y, movement.z); // Сохраняем вертикальную скорость
    }
    else
    {
        // В воздухе, мы можем уменьшить скорость
        _rigidbody.velocity = new Vector3(movement.x * 0.5f, _rigidbody.velocity.y, movement.z * 0.5f); // Уменьшаем скорость в воздухе
    }
}


    private void Rotate()
    {
        float yaw = _lookInput.x * _rotationSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);

        _cameraPitch -= _lookInput.y * _rotationSpeed * Time.deltaTime;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -75f, 75f);
        cameraTransform.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
    }

   private void Jump()
    {
       
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
        GroundCheck = false; 
    }


    private void LeftButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); 
        if (Physics.Raycast(ray, out RaycastHit hit, 100f)) 
        {
            Button button = hit.collider.GetComponent<Button>(); 
            if (button != null)
            {
                button.onClick.Invoke();
            } 
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
