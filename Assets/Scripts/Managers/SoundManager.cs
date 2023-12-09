using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioClip onPlaceClip;
    [SerializeField] private AudioClip onDestroyClip;
    [SerializeField] private AudioSource gameLoopSource;
    [SerializeField] private AudioSource effectsSource;

    private void Start() {
        ObjectPlacer.Instance.OnPlace += ObjectPlacer_OnPlace;
        ObjectPlacer.Instance.OnDestroy += ObjectPlacer_OnDestroy;
    }

    private void ObjectPlacer_OnPlace(object sender, ObjectSO e) {
        //play sound for placing an object
        effectsSource.pitch = Random.Range(0.8f, 1.1f);
        effectsSource.clip = onPlaceClip;
        effectsSource.Play();
    }

    private void ObjectPlacer_OnDestroy(object sender, ObjectSO e) {
        //play sound for removing an object
        effectsSource.pitch = Random.Range(0.8f, 1.1f);
        effectsSource.clip = onDestroyClip;
        effectsSource.Play();
    }
}
