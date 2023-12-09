using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour {
    public static ObjectPlacer Instance { get; private set; }

    public event EventHandler<ObjectSO> OnPlace;
    public event EventHandler<ObjectSO> OnDestroy;

    [SerializeField] private List<GameObject> placedObjects = new();

    private void Awake() {
        Instance = this;
    }

    public int PlaceObject(GameObject prefab, Vector3 position, Vector3 rotation) {
        ObjectSO objectSO = prefab.GetComponent<ObjectData>().GetObjectSO();
        GameObject objectToPlace = Instantiate(prefab);
        if (objectToPlace.TryGetComponent(out AnimationManager animationManager)) {
            animationManager.PlaySpawnAnimation();
        }
        objectToPlace.transform.position = position;
        Transform firstChild = objectToPlace.transform.GetChild(0);
        foreach (Transform child in firstChild) {
            Vector3 currentRotation = child.rotation.eulerAngles;
            child.rotation = Quaternion.Euler(rotation + currentRotation);
        }
        OnPlace?.Invoke(this, objectSO);
        placedObjects.Add(objectToPlace);
        return placedObjects.Count - 1;
    }
    public void DestroyObject(int gameObjectIndex) {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;

        Destroy(placedObjects[gameObjectIndex]);
        OnDestroy?.Invoke(this, placedObjects[gameObjectIndex].GetComponent<ObjectData>().GetObjectSO());
        placedObjects[gameObjectIndex] = null;
    }

    public ObjectType GetGameObjectTypeAtGridPos(Vector3Int position) {
        GameObject placedObject = GetGameObjectAtGridPos(position);
        if (placedObject != null) {
            return placedObject.GetComponent<ObjectData>().GetObjectSO().type;
        }
        return default;
    }

    public GameObject GetGameObjectAtGridPos(Vector3Int position) {
        GameObject placedObject = placedObjects.FirstOrDefault(gameObject => {
            if (gameObject != null) {
                return Mathf.Approximately(gameObject.transform.position.x, position.x) && Mathf.Approximately(gameObject.transform.position.z, position.z);
            }
            return false;
        });

        if (placedObject != null) {
            return placedObject;
        }
        return null;
    }



    public List<GameObject> GetPlacedObjectsList() {
        return placedObjects;
    }
}
