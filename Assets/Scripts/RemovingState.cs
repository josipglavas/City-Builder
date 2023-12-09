using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemovingState : IBuildingState {
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO databaseSO;
    GridData GridData;
    ObjectPlacer objectPlacer;
    RoadManger roadManager;

    public RemovingState(Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO databaseSO, GridData GridData, ObjectPlacer objectPlacer, RoadManger roadManager) {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.databaseSO = databaseSO;
        this.GridData = GridData;
        this.objectPlacer = objectPlacer;
        this.roadManager = roadManager;
        previewSystem.StartShowingRemovePreview();
    }

    public void EndState() {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos, Vector3 rotation) {
        GridData selectedData = null;
        if (!GridData.CanPlaceObject(gridPos, Vector2Int.one)) {
            selectedData = GridData;
        }

        if (selectedData == null) {

        } else {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPos);
            if (gameObjectIndex == -1)
                return;
            ObjectData objectData = objectPlacer.GetPlacedObjectsList()[gameObjectIndex].GetComponent<ObjectData>();
            selectedData.RemoveObject(gridPos);
            objectPlacer.DestroyObject(gameObjectIndex);

            if (objectData.GetObjectSO().type == ObjectType.Road) {
                roadManager.FixRoadAtPos(gridPos, objectData);
            }
        }
        Vector3 cellPos = grid.CellToWorld(gridPos);
        previewSystem.UpdatePos(cellPos, CheckIfSelectionIsValid(gridPos, rotation));

    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos, Vector3 gridRotation) {
        return !(GridData.CanPlaceObject(gridPos, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPos, Vector3 gridRotation, bool isRotating) {
        bool valid = CheckIfSelectionIsValid(gridPos, gridRotation);
        previewSystem.UpdatePos(grid.CellToWorld(gridPos), valid);
    }

}
