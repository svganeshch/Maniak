using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    ThrowForceIndicator forceIndicator;

    public float speed = 5f;
    public float jumpHeight = 10f;
    private float gravity = -9.81f;
    private Vector3 move;
    private Vector3 playerDirection;
    private Vector2 mousePos;
    private float acceleration;
    private Vector3 velocity = Vector3.zero;

    private Ray ray;
    public Transform groundDetectPoint;
    public LayerMask groundmask;
    public bool isGrounded;

    private WeaponMechanics weaponMechanics;
    private CrosshairPos crosshairPos;
    private Injection injection;

    private GameObject InjectionObj;
    public GameObject InjPrefab;
    public Transform InjHoldPos;
    public float throwForceIncrement = 10f;
    public float throwForceMax = 1000f;
    public float throwForce;

    private float _FpsTargetPitch;
    private float _rotationVelocity;

    [Header("Mouse")]
    public float mouseSensitivity = 1f;
    public float RotationSpeed = 0.8f;
    public bool invertX = false;
    public bool invertY = false;

    private bool isReleased = false;

    private void Awake()
    {
        crosshairPos = FindAnyObjectByType<CrosshairPos>();
        forceIndicator = FindAnyObjectByType<ThrowForceIndicator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        weaponMechanics = GetComponentInChildren<WeaponMechanics>();
        injection = FindObjectOfType<Injection>();

        InjectionObj = injection.InstantiateInj(InjPrefab, InjHoldPos, crosshairPos.transform);
    }

    private void FixedUpdate()
    {
        if (move.magnitude > 0.2f && move.magnitude < 0.5f)
        {
            acceleration += Time.deltaTime * speed;
            acceleration = acceleration > 0.5f ? 0.5f : acceleration;
        }

        if (move.magnitude > 0.5f)
        {
            acceleration += Time.deltaTime * speed;
            acceleration = acceleration > 1f ? 1f : acceleration;
        }

        if (move.magnitude < 0.2f)
        {
            acceleration -= Time.deltaTime * speed;
            acceleration = acceleration < 0f ? 0f : acceleration;
        }
        animator.SetFloat("Velocity", acceleration);

        ray.direction = Vector3.down;
        ray.origin = groundDetectPoint.position;

        isGrounded = Physics.Raycast(ray, 2f, groundmask);
    }

    private void Update()
    {
        //Jump
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
        }
        velocity.y += gravity * 2 * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Move
        if (move != Vector3.zero)
        {
            playerDirection = transform.right * move.x + transform.forward * move.z;
            controller.Move(playerDirection.normalized * speed * Time.deltaTime);
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            throwForce = 0f;
            animator.SetLayerWeight(1, Mathf.Lerp(0, 1, 1));
        }

        //shoot
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //weaponMechanics.Shoot();

            throwForce += throwForceIncrement;
            throwForce = Mathf.Clamp(throwForce, 0, throwForceMax);

            animator.SetFloat("ThrowForce", (throwForce / throwForceMax) / 2);

            forceIndicator.SetForceIndicator(throwForce);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            //raycastTrajectory.StopFiring();

            isReleased = true;

            InjectionObj.TryGetComponent<Injection>(out injection);
            if (injection != null) injection.Throw(crosshairPos.transform, throwForce);

            InjectionObj.transform.parent = null;

            forceIndicator.SetForceIndicator(0);
            InjectionObj = injection.InstantiateInj(InjPrefab, InjHoldPos, crosshairPos.transform);
        }

        if (isReleased)
        {
            animator.SetFloat("ThrowForce", Mathf.Lerp((throwForce / throwForceMax), 1, 0.5f));

            animator.SetLayerWeight(1, 1 - throwForce);

            if (animator.GetLayerWeight(1) < 0.1f)
                isReleased = false;
        }
    }
    private void LateUpdate()
    {
        CameraRotation();
    }

    //private void InstantiateInj()
    //{
    //    InjectionObj = Instantiate(InjPrefab, InjHoldPos.position, Quaternion.identity, InjHoldPos);
    //    InjectionObj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    //    InjectionObj.TryGetComponent<Injection>(out injection);
    //    injection.SetTarget(crosshairPos.transform);
    //}

    public void OnMove(InputValue value)
    {
        Vector3 moveVal = value.Get<Vector2>();
        move = new Vector3(moveVal.x, 0, moveVal.y).normalized;
    }

    public void OnLook(InputValue value)
    {
        mousePos = value.Get<Vector2>().normalized;
        mousePos = new Vector2(mousePos.x * mouseSensitivity * ((invertX) ? -1 : 1), mousePos.y * mouseSensitivity * ((invertY) ? -1 : 1)).normalized;
    }

    public void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            speed *= 1.5f;
            Debug.Log("Speed : " + speed);
        }
    }

    private void CameraRotation()
    {
        if (mousePos.sqrMagnitude > 0.01f)
        {
            _FpsTargetPitch += mousePos.y * RotationSpeed * 1;
            _rotationVelocity = mousePos.x * RotationSpeed * 1;
            _FpsTargetPitch = ClampAngle(_FpsTargetPitch, -90.0f, 90.0f);

            Camera.main.transform.localRotation = Quaternion.Euler(_FpsTargetPitch, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(groundDetectPoint.position, Vector3.down, Color.yellow);
    }
}
