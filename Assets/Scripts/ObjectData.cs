using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {
    [SerializeField] private ObjectSO objectSO;
    private ParticleSystem constructionParticles;

    private void Awake() {
        constructionParticles = GetComponent<ParticleSystem>();
    }

    public ObjectSO GetObjectSO() {
        return objectSO;
    }

    public void ReplacePrefab(GameObject prefab, Quaternion rotation) {
        Vector3 position = Vector3.zero;

        foreach (Transform child in transform) {
            position = child.transform.localPosition;
            Destroy(child.gameObject);
        }

        GameObject structure = Instantiate(prefab, transform);
        structure.transform.localPosition = position;
        structure.transform.localRotation = rotation;
    }

    public void EmitParticles() {
        constructionParticles.Play();
    }
}
