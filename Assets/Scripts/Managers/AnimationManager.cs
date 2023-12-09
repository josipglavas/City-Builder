using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class AnimationManager : MonoBehaviour {

    private const string SPAWN = "Spawn";
    private Animator animator;
    private int spawnHash;

    private void Awake() {
        animator = GetComponent<Animator>();
        spawnHash = Animator.StringToHash(SPAWN);
        animator.applyRootMotion = false;
    }

    public void PlaySpawnAnimation() {
        animator.SetTrigger(spawnHash);

    }

    public void OnAnimationEndEvent() {
        // animator.applyRootMotion = false;
        //Vector3 currentPosition = transform.position;
        //transform.position = new Vector3(currentPosition.x, 0, currentPosition.y);
    }

}
