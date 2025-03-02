using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AnimationType<T>
{
    public string Key;
    public T Value;
    public override bool Equals(object obj)
    {
        if(obj is not AnimationType<T>)
        {
            return false;
        }

        return Key.Equals(((AnimationType<T>)obj).Key);
    }
    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}
[Serializable]
public struct AnimationData<T>
{
    public List<AnimationType<T>> AnimationDatas;
}
[Serializable]
public struct AnimationDatas
{
    public AnimationData<bool> boolData;
    public AnimationData<float> floatData;
    public AnimationData<string> stringData;
    public AnimationData<int> intData;
}

public class AnimationController : MonoBehaviour
{
    private void Start()
    {
        playerController.PostUpdate += onUpdateController;
    }

    private void Update()
    {
        
        // animationController.SetFloat(ANIM_ID_MOVESPEED, currentSpeed);

        //if(Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    animationController.SetBool("Aiming",true);
        //}

        //if(Input.GetKeyUp(KeyCode.Mouse1))
        //{
        //    animationController.SetBool("Aiming", false);
        //}
        //if(Input.GetKeyDown(KeyCode.Backspace))
        //{
            
        //}
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
    [SerializeField]
    private AnimationClip testClip;
}
