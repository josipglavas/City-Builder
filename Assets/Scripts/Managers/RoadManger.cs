using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManger : MonoBehaviour {

    [SerializeField] private GameObject roadStraight;
    [SerializeField] private GameObject deadEnd;
    [SerializeField] private GameObject corner;
    [SerializeField] private GameObject threeWay;
    [SerializeField] private GameObject fourWay;
    [SerializeField] private ObjectPlacer objectPlacer;

    private List<Vector3Int> roadPositionsToReCheck = new List<Vector3Int>();

    private void ReplaceRoad(Vector3Int position, ObjectData objectData) {
        int roadCount = 0;
        Vector3Int[] neighbourCells = GetNeighbourCells(position);
        foreach (Vector3Int neighbourCell in neighbourCells) {
            ObjectType neighbourObjectType = ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCell);
            if (neighbourObjectType == ObjectType.Road) {
                roadCount++;// we found a road around this object we need to replace its prefab
            }
        }
        if (roadCount == 0 || roadCount == 1) {
            CreateDeadEnd(neighbourCells, objectData);
        } else if (roadCount == 2) {
            if (CreateStraightRoad(neighbourCells, objectData))
                return;
            CreateCorner(neighbourCells, objectData);
        } else if (roadCount == 3) {
            CreateThreeWay(neighbourCells, objectData);
        } else {
            CreateFourWay(neighbourCells, objectData);
        }

    }


    public void FixRoadAtPos(Vector3Int position, ObjectData objectData) {
        roadPositionsToReCheck.Clear();
        ReplaceRoad(position, objectData);

        List<Vector3Int> neighbours = GetNeighbourCellsPosOfType(position, ObjectType.Road);
        foreach (Vector3Int roadPos in neighbours) {
            roadPositionsToReCheck.Add(roadPos);
        }

        foreach (Vector3Int posToFix in roadPositionsToReCheck) {
            GameObject road = objectPlacer.GetGameObjectAtGridPos(posToFix);
            if (road != null) {
                if (road.TryGetComponent(out ObjectData data)) {
                    ReplaceRoad(posToFix, data);
                }
            }
        }

    }

    private void CreateFourWay(Vector3Int[] neighbourCells, ObjectData objectData) {
        objectData.ReplacePrefab(fourWay, Quaternion.identity);
    }

    // [left, up, right, down]
    private void CreateThreeWay(Vector3Int[] neighbourCells, ObjectData objectData) {
        if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[1]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[2]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[3]) == ObjectType.Road) {
            objectData.ReplacePrefab(threeWay, Quaternion.Euler(0, 180, 0));

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[2]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[3]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[0]) == ObjectType.Road) {
            objectData.ReplacePrefab(threeWay, Quaternion.Euler(0, 270, 0));

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[3]) == ObjectType.Road
              && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[0]) == ObjectType.Road
              && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[1]) == ObjectType.Road) {
            objectData.ReplacePrefab(threeWay, Quaternion.identity);

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[0]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[1]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[2]) == ObjectType.Road) {
            objectData.ReplacePrefab(threeWay, Quaternion.Euler(0, 90, 0));
        }

    }

    // [left, up, right, down]
    private void CreateDeadEnd(Vector3Int[] neighbourCells, ObjectData objectData) {
        if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[1]) == ObjectType.Road) {
            objectData.ReplacePrefab(deadEnd, Quaternion.Euler(0, 180, 0));

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[2]) == ObjectType.Road) {
            objectData.ReplacePrefab(deadEnd, Quaternion.Euler(0, 270, 0));

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[3]) == ObjectType.Road) {
            objectData.ReplacePrefab(deadEnd, Quaternion.identity);

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[0]) == ObjectType.Road) {
            objectData.ReplacePrefab(deadEnd, Quaternion.Euler(0, 90, 0));

        }
    }

    // [left, up, right, down]
    private bool CreateStraightRoad(Vector3Int[] neighbourCells, ObjectData objectData) {
        if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[1]) == ObjectType.Road
                && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[3]) == ObjectType.Road) {
            objectData.ReplacePrefab(roadStraight, Quaternion.identity);
            return true;
        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[0]) == ObjectType.Road
                && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[2]) == ObjectType.Road) {
            objectData.ReplacePrefab(roadStraight, Quaternion.Euler(0, 90, 0));
            return true;
        }
        return false;
    }

    // [left, up, right, down]
    private void CreateCorner(Vector3Int[] neighbourCells, ObjectData objectData) {
        if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[1]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[2]) == ObjectType.Road) {
            objectData.ReplacePrefab(corner, Quaternion.Euler(0, 90, 0));

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[2]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[3]) == ObjectType.Road) {
            objectData.ReplacePrefab(corner, Quaternion.Euler(0, 180, 0));

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[3]) == ObjectType.Road
              && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[0]) == ObjectType.Road) {
            objectData.ReplacePrefab(corner, Quaternion.Euler(0, 270, 0));

        } else if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[0]) == ObjectType.Road
            && ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(neighbourCells[1]) == ObjectType.Road) {
            objectData.ReplacePrefab(corner, Quaternion.identity);
        }
    }

    private Vector3Int[] GetNeighbourCells(Vector3Int gridPos) {
        Vector3Int[] neighbourCells = new Vector3Int[4];
        int x = gridPos.x;
        int y = gridPos.z;

        //up x0, y1
        //down x0, y-1
        //left x-1, y0
        //right x1, y0

        neighbourCells[0] = new Vector3Int(x - 1, 0, y); // left
        neighbourCells[1] = new Vector3Int(x, 0, y + 1); // up
        neighbourCells[2] = new Vector3Int(x + 1, 0, y); // right
        neighbourCells[3] = new Vector3Int(x, 0, y - 1); // down

        // returns an array with the positions of neighbour cells relative to the gridPos provided in this order: [left, up, right, down]

        return neighbourCells;
    }

    private List<Vector3Int> GetNeighbourCellsPosOfType(Vector3Int gridPos, ObjectType type) {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        Vector3Int[] neighbourCells = GetNeighbourCells(gridPos);
        foreach (Vector3Int position in neighbourCells) {
            if (ObjectPlacer.Instance.GetGameObjectTypeAtGridPos(position) == type) {
                neighbours.Add(position);
            }
        }
        return neighbours;
    }
}
