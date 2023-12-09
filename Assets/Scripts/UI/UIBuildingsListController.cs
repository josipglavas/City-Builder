using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingsListController : MonoBehaviour {

    [SerializeField] private RectTransform expandRetractButton;
    [SerializeField] private GameObject listParent;
    [SerializeField] private Transform gridLayoutGroup;
    [SerializeField] private GameObject uiObjectParentPrefab;
    [SerializeField] private ObjectsDatabaseSO objectsDatabaseSO;

    [SerializeField] private Button all;
    [SerializeField] private Button house;
    [SerializeField] private Button factory;
    [SerializeField] private Button nature;
    [SerializeField] private Button road;
    [SerializeField] private Button fence;
    [SerializeField] private Button store;

    private void Awake() {
        all.onClick.AddListener(() => { ExpandListWithoutType(); });
        house.onClick.AddListener(() => { ExpandList(ObjectType.House); });
        factory.onClick.AddListener(() => { ExpandList(ObjectType.Factory); });
        nature.onClick.AddListener(() => { ExpandList(ObjectType.Nature); });
        road.onClick.AddListener(() => { ExpandList(ObjectType.Road); });
        fence.onClick.AddListener(() => { ExpandList(ObjectType.Fence); });
        store.onClick.AddListener(() => { ExpandList(ObjectType.Store); });

    }
    private void Start() {
        InputManager.Instance.OnExit += CollapseList;
    }

    public void ExpandList(ObjectType objectType) {
        listParent.SetActive(true);
        DestoryChildren();
        ShowObjectsOfType(objectType);
    }

    public void ExpandListWithoutType() {
        listParent.SetActive(true);
        DestoryChildren();
        InstantiateObjectWithoutType();
    }

    private void ShowObjectsOfType(ObjectType objectType) {
        switch (objectType) {
            default:
                InstantiateObjectWithoutType();
                break;
            case ObjectType.House:
                InstantiateObject(ObjectType.House);
                break;

            case ObjectType.Factory:
                InstantiateObject(ObjectType.Factory);
                break;

            case ObjectType.Store:
                InstantiateObject(ObjectType.Store);
                break;

            case ObjectType.Nature:
                InstantiateObject(ObjectType.Nature);
                break;

            case ObjectType.Fence:
                InstantiateObject(ObjectType.Fence);
                break;

            case ObjectType.Road:
                InstantiateObject(ObjectType.Road);
                break;
        }
    }

    private void InstantiateObjectWithoutType() {
        foreach (ObjectSO objectSO in objectsDatabaseSO.objectsData) {
            GameObject item = Instantiate(uiObjectParentPrefab, gridLayoutGroup);
            item.GetComponent<UIObjectController>().SetItemParameters(objectSO.cost, objectSO.id, objectSO.image);
        }
    }

    private void InstantiateObject(ObjectType objectType) {
        foreach (ObjectSO objectSO in objectsDatabaseSO.objectsData) {
            if (objectSO.type != objectType) {
                continue;
            }
            GameObject item = Instantiate(uiObjectParentPrefab, gridLayoutGroup);
            item.GetComponent<UIObjectController>().SetItemParameters(objectSO.cost, objectSO.id, objectSO.image);
        }
    }

    private void DestoryChildren() {
        if (gridLayoutGroup.childCount > 0) {
            foreach (Transform child in gridLayoutGroup) {
                Destroy(child.gameObject);
            }
        }
    }
    public void CollapseList() {
        listParent.SetActive(false);
        DestoryChildren();
    }
}
