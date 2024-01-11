using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Scenes {
    MainMenu,
    Gameplay
}

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private Button playButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            SceneManager.LoadScene(Scenes.Gameplay.ToString());
        });
    }
}
