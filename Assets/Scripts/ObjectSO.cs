using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu]
public class ObjectSO : ScriptableObject {
    public string objectName;
    public Sprite image;
    public int id;
    public Vector2Int size;
    public GameObject prefab;
    public int cost;
    public int costPerTurn;
    public int population;
    public int jobs;
    public int food;
    public int incomePerJob;
    public ObjectType type;
}
