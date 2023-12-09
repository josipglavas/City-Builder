using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlacementSystem : MonoBehaviour {
    public static PlacementSystem Instance { get; private set; }

    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabaseSO databaseSO;
    [SerializeField] private GameObject gridVisual;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private RoadManger roadManger;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private UIBuildingsListController buildingsListController;

    private GridData GridData;
    private int lastSelectedId = 0;
    private Vector3Int lastDetectedPos = Vector3Int.zero;
    private bool isRotating = false;


    private IBuildingState buildingState;
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StopPlacement();
        GridData = new GridData();
    }

    public void StartPlacement(int Id) {
        StopPlacement();
        gridVisual.SetActive(true);
        buildingState = new PlacementState(Id, grid, preview, databaseSO, GridData, objectPlacer, roadManger);
        lastSelectedId = Id;
        InputManager.Instance.OnClick += PlaceObject;
        InputManager.Instance.OnRotate += RotateObject;
        InputManager.Instance.OnExit += StopPlacement;
    }

    public void StartRemoving() {
        StopPlacement();
        gridVisual.SetActive(true);
        buildingState = new RemovingState(grid, preview, databaseSO, GridData, objectPlacer, roadManger);
        InputManager.Instance.OnClick += PlaceObject;
        InputManager.Instance.OnExit += StopPlacement;
    }

    private void RotateObject() {
        if (InputManager.Instance.IsHoveringOverUI() || buildingState == null || databaseSO.objectsData[lastSelectedId].type == ObjectType.Road) {
            return;
        }
        isRotating = true;
        Vector3 mousePos = InputManager.Instance.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        buildingState.UpdateState(gridPos, new Vector3(0, 90, 0), isRotating);
        isRotating = false;
    }

    private void PlaceObject() {
        if (InputManager.Instance.IsHoveringOverUI()) {
            return;
        }
        Vector3 mousePos = InputManager.Instance.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        buildingState.OnAction(gridPos, preview.ObjectRotation);

    }

    public void StopPlacement() {
        if (buildingState == null)
            return;
        lastSelectedId = 0;
        gridVisual.SetActive(false);
        buildingState.EndState();
        preview.ResetRotation();

        InputManager.Instance.OnClick -= PlaceObject;
        InputManager.Instance.OnRotate -= RotateObject;
        InputManager.Instance.OnExit -= StopPlacement;
        lastDetectedPos = Vector3Int.zero;
        buildingState = null;
    }
    private void Update() {
        if (buildingState == null)
            return;

        Vector3 mousePos = InputManager.Instance.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        if (lastDetectedPos != gridPos || !isRotating) {
            buildingState.UpdateState(gridPos, preview.ObjectRotation, isRotating);
            lastDetectedPos = gridPos;
        }

    }
}
