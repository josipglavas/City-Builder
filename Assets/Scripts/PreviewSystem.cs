using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PreviewSystem : MonoBehaviour {
    [SerializeField] private float previewYOffset = 0.06f;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Material previewMaterialPrefab;
    private GameObject previewObj;
    private Material previewMaterialInstance;

    public Vector3 ObjectRotation { get; private set; }

    private MeshRenderer cellIndicatorRenderer;
    private void Start() {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<MeshRenderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size) {
        previewObj = Instantiate(prefab);
        PreparePreview(previewObj);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size) {
        if (size.x > 0 || size.y > 0) {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObj) {
        MeshRenderer[] renderers = previewObj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers) {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++) {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview() {
        cellIndicator.SetActive(false);
        if (previewObj != null) {
            Destroy(previewObj);
        }
    }

    public void UpdatePos(Vector3 position, bool valid) {

        if (previewObj != null) {
            MovePreview(position);
            ApplyFeedbackToPreview(valid);
        }

        if (previewObj != null && GameManager.Instance.Money < previewObj.GetComponent<ObjectData>().GetObjectSO().cost) {
            MovePreview(position);
            ApplyFeedbackToPreview(false);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(valid);

    }

    public void UpdateRot(Vector3 rotation, bool valid) {
        if (previewObj != null) {
            RotatePreview(rotation);
            RotateCursor(rotation);
            ApplyFeedbackToPreview(valid);
            ApplyFeedbackToCursor(valid);
        }
    }

    private void ApplyFeedbackToPreview(bool valid) {
        Color color = valid ? Color.white : Color.red;
        color.a = 0.65f;
        previewMaterialInstance.color = color;
    }

    private void ApplyFeedbackToCursor(bool valid) {
        Color color = valid ? Color.white : Color.red;
        color.a = 0.5f;
        cellIndicatorRenderer.material.color = color;
    }

    private void MoveCursor(Vector3 position) {
        cellIndicator.transform.position = position;
    }

    private void RotateCursor(Vector3 rotation) {
        Transform gridIndicator = cellIndicatorRenderer.transform;
        gridIndicator.localRotation = Quaternion.Euler(90, gridIndicator.localRotation.eulerAngles.y + rotation.y, 0);

    }

    private void MovePreview(Vector3 position) {
        previewObj.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    private void RotatePreview(Vector3 rotation) {
        Transform firstChild = previewObj.transform.GetChild(0);
        foreach (Transform child in firstChild) {
            Vector3 currentRotation = child.rotation.eulerAngles;
            if (currentRotation.y == 360) {
                currentRotation.y = 0;
            }
            ObjectRotation = currentRotation + rotation;
            child.rotation = Quaternion.Euler(ObjectRotation);
        }
    }

    public void ResetRotation() {
        ObjectRotation = Vector3.zero;
    }

    public void StartShowingRemovePreview() {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
