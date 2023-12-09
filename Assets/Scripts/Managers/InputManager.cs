using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    public event Action OnClick;
    public event Action OnExit;
    public event Action OnRotate;

    [SerializeField] private LayerMask buildableLayermask;

    private Camera mainCamera;
    private Vector3 lastPosition;
    private PlayerInputActions playerInputActions;
    public Vector2 MoveDir { get; private set; }
    public bool SpeedClicked { get; private set; }
    public bool Pan { get; private set; }
    public float Zoom { get; private set; }
    public Vector2 MousePos { get; private set; }

    private void Awake() {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        playerInputActions.Navigation.Move.performed += Move_performed;

        playerInputActions.Navigation.Pan.performed += Pan_Performed;
        playerInputActions.Navigation.Speed.performed += Speed_performed;
        playerInputActions.Navigation.Rotate.performed += Rotate_performed;
        playerInputActions.Navigation.Cancel.performed += Cancel_performed;
        playerInputActions.Navigation.Select.performed += Select_performed;
        playerInputActions.Navigation.Zoom.performed += Zoom_performed;
        playerInputActions.Navigation.Mouse.performed += Mouse_performed;
    }

    private void Mouse_performed(InputAction.CallbackContext obj) {
        MousePos = obj.ReadValue<Vector2>();
    }

    private void Zoom_performed(InputAction.CallbackContext obj) {
        Zoom = obj.ReadValue<float>();
    }

    private void Select_performed(InputAction.CallbackContext obj) {
        OnClick?.Invoke();
    }

    private void Cancel_performed(InputAction.CallbackContext obj) {
        OnExit?.Invoke();
    }

    private void Rotate_performed(InputAction.CallbackContext obj) {
        OnRotate?.Invoke();
    }

    private void Speed_performed(InputAction.CallbackContext obj) {
        SpeedClicked = obj.ReadValueAsButton();
    }

    private void Pan_Performed(InputAction.CallbackContext obj) {
        Pan = obj.ReadValueAsButton();
    }

    private void Move_performed(InputAction.CallbackContext obj) {
        MoveDir = obj.ReadValue<Vector2>();
    }

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {

    }

    public bool IsHoveringOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition() {
        float maxDistance = 100.0f;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.nearClipPlane;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, buildableLayermask)) {
            lastPosition = hit.point;
        }

        return lastPosition;

    }

    private void OnEnable() {
        playerInputActions.Enable();
    }
    private void OnDisable() {
        playerInputActions.Disable();
    }
}
