using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private int day;
    [SerializeField] private int curPopulation;
    [SerializeField] private int curJobs;
    [SerializeField] private int curFood;
    [SerializeField] private int maxPopulation;
    [SerializeField] private int maxJobs;
    public int Money { get; private set; }

    private void Awake() {
        Instance = this;

    }
    private void Start() {
        ObjectPlacer.Instance.OnPlace += ObjectPlacer_OnPlace;
        ObjectPlacer.Instance.OnDestroy += ObjectPlacer_OnDestroy;
        Money = 2000;
        SetStatsText();

    }

    private void ObjectPlacer_OnPlace(object sender, ObjectSO objectSO) {
        maxPopulation += objectSO.population;
        maxJobs += objectSO.jobs;
        Money -= objectSO.cost;
        SetStatsText();
    }

    private void ObjectPlacer_OnDestroy(object sender, ObjectSO objectSO) {
        maxPopulation -= objectSO.population;
        maxJobs -= objectSO.jobs;
        Money += (int)(objectSO.cost * 0.7); // We are getting back 70% of the price paid
        SetStatsText();
    }

    private void CalculateMoney() {
        foreach (GameObject placedObject in ObjectPlacer.Instance.GetPlacedObjectsList()) {
            if (placedObject == null)
                continue;
            ObjectSO placedObjectSO = placedObject.GetComponent<ObjectData>().GetObjectSO();
            Money += (placedObjectSO.incomePerJob - placedObjectSO.costPerTurn);
        }
    }

    private void CalculatePopulation() {
        maxPopulation = 0;
        foreach (GameObject placedObject in ObjectPlacer.Instance.GetPlacedObjectsList()) {
            if (placedObject == null)
                continue;
            maxPopulation += placedObject.GetComponent<ObjectData>().GetObjectSO().population;
        }

        if (curFood >= curPopulation && curPopulation < maxPopulation) {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation);
        } else if (curFood < curPopulation) {
            curPopulation = curFood;
        }
    }

    private void CalculateJobs() {
        curJobs = 0;
        maxJobs = 0;
        foreach (GameObject placedObject in ObjectPlacer.Instance.GetPlacedObjectsList()) {
            if (placedObject == null)
                continue;
            maxJobs += placedObject.GetComponent<ObjectData>().GetObjectSO().jobs;
        }

        curJobs = Mathf.Min(curPopulation, maxJobs);
    }

    private void CalculateFood() {
        curFood = 0;
        foreach (GameObject placedObject in ObjectPlacer.Instance.GetPlacedObjectsList()) {
            if (placedObject == null)
                continue;
            curFood += placedObject.GetComponent<ObjectData>().GetObjectSO().food;

        }
        curFood -= curPopulation;
    }

    private void SetStatsText() {
        statsText.text = string.Format("Day: {0} | Money: ${1} | Pop: {2} / {3} | Jobs: {4} / {5} | Food: {6}", new object[7] { day, Money, curPopulation, maxPopulation, curJobs, maxJobs, curFood });
    }

    public void EndTurn() {
        day++;
        CalculateMoney();
        CalculatePopulation();
        CalculateJobs();
        CalculateFood();
        SetStatsText();
    }


}
