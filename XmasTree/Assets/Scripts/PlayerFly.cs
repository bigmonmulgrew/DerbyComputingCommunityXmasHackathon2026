using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFly : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference flyForward;
    [SerializeField] private InputActionReference flyRight;
    [SerializeField] private InputActionReference flyUp;
    [SerializeField] private InputActionReference look;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lookSensitivity = 0.1f;
    [SerializeField] private float maxPitch = 89f;

    float pitch;

    void OnEnable()
    {
        flyForward.action.Enable();
        flyRight.action.Enable();
        flyUp.action.Enable();
        look.action.Enable();
    }

    void OnDisable()
    {
        flyForward.action.Disable();
        flyRight.action.Disable();
        flyUp.action.Disable();
        look.action.Disable();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        float forward = flyForward.action.ReadValue<float>();
        float right = flyRight.action.ReadValue<float>();
        float up = flyUp.action.ReadValue<float>();

        Vector3 move =
            transform.forward * forward +
            transform.right * right +
            transform.up * up;

        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleLook()
    {
        Vector2 delta = look.action.ReadValue<Vector2>() * lookSensitivity;

        // Yaw (left/right)
        transform.Rotate(Vector3.up, delta.x, Space.World);

        // Pitch (up/down)
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        Vector3 euler = transform.localEulerAngles;
        euler.x = pitch;
        transform.localEulerAngles = euler;
    }
}
