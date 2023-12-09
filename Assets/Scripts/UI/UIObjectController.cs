using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIObjectController : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Image itemImage;
    private int ItemId;

    private void Awake() {
        button.onClick.AddListener(() => {
            PlacementSystem.Instance.StartPlacement(ItemId);
        });
    }

    public void SetItemParameters(int price, int id, Sprite sprite = null) {
        if (sprite != null) {
            itemImage.sprite = sprite;
        }
        ItemId = id;
        itemPrice.text = "$ " + price.ToString();
    }

}
