using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minXRot;
    [SerializeField] private float maxXRot;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float cameraBounds = 20f;

    private float curXRot;
    private float curZoom;
    private Camera cam;

    private void Start() {
        cam = Camera.main;
        curZoom = cam.transform.localPosition.y;
        curXRot = -50;
    }

    private void Update() {
        Zoom();
        Rotate();
        Move();
    }

    private void Zoom() {
        curZoom += InputManager.Instance.Zoom * -zoomSpeed;
        curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);
        cam.transform.localPosition = Vector3.up * curZoom;

    }

    private void Rotate() {
        if (InputManager.Instance.Pan) {
            float x = InputManager.Instance.MousePos.x;
            float y = InputManager.Instance.MousePos.y;
            curXRot += -y * rotateSpeed;
            curXRot = Mathf.Clamp(curXRot, minXRot, maxXRot);
            transform.eulerAngles = new Vector3(curXRot, transform.eulerAngles.y + (x * rotateSpeed), 0.0f);
        }
    }

    private void Move() {
        float cameraSpeed;
        if (InputManager.Instance.SpeedClicked) {
            cameraSpeed = moveSpeed * 1.75f;
        } else {
            cameraSpeed = moveSpeed;
        }

        Vector3 fwd = cam.transform.forward;
        fwd.y = 0.0f;
        fwd.Normalize();
        Vector3 camRight = cam.transform.right.normalized;
        float moveX = InputManager.Instance.MoveDir.x;
        float moveZ = InputManager.Instance.MoveDir.y;
        Vector3 dir = fwd * moveZ + camRight * moveX;
        dir.Normalize();
        dir *= cameraSpeed * Time.deltaTime;
        transform.position += dir;

        if (transform.position.x > cameraBounds) {
            transform.position = new Vector3(cameraBounds, transform.position.y, transform.position.z);
        } else if (transform.position.x < -cameraBounds) {
            transform.position = new Vector3(-cameraBounds, transform.position.y, transform.position.z);
        }

        if (transform.position.z > cameraBounds) {
            transform.position = new Vector3(transform.position.x, transform.position.y, cameraBounds);
        } else if (transform.position.z < -cameraBounds) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -cameraBounds);
        }

    }
}
