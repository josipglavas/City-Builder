using UnityEngine;

public interface IBuildingState {
    public void EndState();
    public void OnAction(Vector3Int gridPos, Vector3 rotation);
    public void UpdateState(Vector3Int gridPos, Vector3 gridRotation, bool isRotating);

}