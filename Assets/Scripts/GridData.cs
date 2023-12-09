using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData {
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objSize, int Id, int placedObjIndex) {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objSize);
        PlacementData data = new PlacementData(positionsToOccupy, Id, placedObjIndex);
        foreach (var pos in positionsToOccupy) {
            if (placedObjects.ContainsKey(pos)) {
                throw new Exception($"Dictionary already contains this position: {pos}");

            }
            placedObjects[pos] = data;
        }

    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objSize) {
        List<Vector3Int> returnValue = new();
        for (int x = 0; x < objSize.x; x++) {
            for (int y = 0; y < objSize.y; y++) {
                returnValue.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }

        return returnValue;
    }

    public bool CanPlaceObject(Vector3Int gridPosition, Vector2Int objSize) {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objSize);
        foreach (var pos in positionsToOccupy) {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPos) {
        if (!placedObjects.ContainsKey(gridPos)) {
            return -1;
        }
        return placedObjects[gridPos].PlacedObjIndex;
    }

    public void RemoveObject(Vector3Int gridPos) {
        foreach (var pos in placedObjects[gridPos].occupiedPos) {
            placedObjects.Remove(pos);

        }
    }

    public class PlacementData {
        public List<Vector3Int> occupiedPos;

        public int Id { get; private set; }

        public int PlacedObjIndex { get; private set; }

        public PlacementData(List<Vector3Int> occupiedPos, int id, int placedObjIndex) {
            this.occupiedPos = occupiedPos;
            Id = id;
            PlacedObjIndex = placedObjIndex;
        }
    }
}