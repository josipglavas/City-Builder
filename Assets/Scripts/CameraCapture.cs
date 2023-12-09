using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CameraCapture : MonoBehaviour {
    public int fileCounter;
    public KeyCode screenshotKey;
    [SerializeField] private GameObject Grid;
    [SerializeField] private Camera Camera;

    private void Start() {
        Grid.SetActive(false);
    }

    private void LateUpdate() {
        if (Input.GetKeyDown(screenshotKey)) {
            Capture();
        }
    }

    public void Capture() {
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;

        Camera.Render();

        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        Destroy(image);

        string path = Application.dataPath + "/Art/Images/" + fileCounter + ".png";

        File.WriteAllBytes(path, bytes);

        Debug.Log($"Image created with name: {fileCounter}.png");

        fileCounter++;
    }
}
