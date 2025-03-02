using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private void Start()
    {
        playerController.PostUpdate += onUpdateController;
    }

    private void Update()
    {
        
        // animationController.SetFloat(ANIM_ID_MOVESPEED, currentSpeed);
    }
    private void onUpdateController(Vector3 vel, float arg2)
    {
        float val = vel.magnitude;
        animationController.SetFloat(ANIM_ID_MOVESPEED, val);
    }

    private const string ANIM_ID_MOVESPEED = "MoveSpeed";

    [SerializeField]
    private Animator animationController;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Transform targetTransform;
}
