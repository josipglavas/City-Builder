using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementState : IBuildingState {
    private int selectedObjectIndex = -1;
    int Id;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO databaseSO;
    GridData GridData;
    ObjectPlacer objectPlacer;
    RoadManger roadManager;
    public PlacementState(int id,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO databaseSO,
                          GridData GridData,
                          ObjectPlacer objectPlacer,
                          RoadManger roadManager) {
        Id = id;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.databaseSO = databaseSO;
        this.GridData = GridData;
        this.objectPlacer = objectPlacer;
        this.roadManager = roadManager;

        selectedObjectIndex = databaseSO.objectsData.FindIndex(data => data.id == Id);
        if (selectedObjectIndex > -1) {
            previewSystem.StartShowingPlacementPreview(databaseSO.objectsData[selectedObjectIndex].prefab, databaseSO.objectsData[selectedObjectIndex].size);
        } else {
            throw new System.Exception($"No object found with id: {id}");
        }

    }

    public void EndState() {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos, Vector3 rotation) {
        bool placementValid = CheckPlacement(gridPos, selectedObjectIndex);
        if (!placementValid || databaseSO.objectsData[selectedObjectIndex].cost > GameManager.Instance.Money)
            return;
        int index = 0;
        if (databaseSO.objectsData[selectedObjectIndex].type == ObjectType.Road) {
            index = objectPlacer.PlaceObject(databaseSO.objectsData[selectedObjectIndex].prefab, grid.CellToWorld(gridPos), rotation);
            ObjectData objectData = objectPlacer.GetPlacedObjectsList()[index].GetComponent<ObjectData>();
            roadManager.FixRoadAtPos(gridPos, objectData);
        } else {
            index = objectPlacer.PlaceObject(databaseSO.objectsData[selectedObjectIndex].prefab, grid.CellToWorld(gridPos), rotation);
        }

        GridData selectedData = GridData;
        selectedData.AddObjectAt(gridPos, databaseSO.objectsData[selectedObjectIndex].size, databaseSO.objectsData[selectedObjectIndex].id, index);
        previewSystem.UpdatePos(grid.CellToWorld(gridPos), false);
    }
    private bool CheckPlacement(Vector3Int gridPos, int selectedObjectIndex) {
        GridData selectedData = GridData;
        return selectedData.CanPlaceObject(gridPos, databaseSO.objectsData[selectedObjectIndex].size);
    }

    public void UpdateState(Vector3Int gridPos, Vector3 gridRotation, bool isRotating) {
        bool placementValid = CheckPlacement(gridPos, selectedObjectIndex);
        previewSystem.UpdatePos(grid.CellToWorld(gridPos), placementValid);
        if (isRotating)
            previewSystem.UpdateRot(gridRotation, placementValid);

    }

}
