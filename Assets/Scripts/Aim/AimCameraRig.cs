using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class AimCameraRig : CinemachineCameraManagerBase, Unity.Cinemachine.IInputAxisOwner
{
    public InputAxis AimMode = InputAxis.DefaultMomentary;

    [SerializeField]
    private CinemachineVirtualCameraBase aimCamera;
    [SerializeField]
    private CinemachineVirtualCameraBase freeCamera;

    bool IsAiming => AimMode.Value > 0.5f;
    public void GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
    {
        axes.Add(new() { DrivenAxis = () =>ref AimMode, Name = "Aim" });
    }

    protected override CinemachineVirtualCameraBase ChooseCurrentCamera(Vector3 worldUp, float deltaTime)
    {
        CinemachineVirtualCameraBase oldCam = (CinemachineVirtualCameraBase)LiveChild;
        CinemachineVirtualCameraBase newCam = IsAiming ? aimCamera : freeCamera;
        if(oldCam != newCam)
        {
            // 바라보는거 세팅을 해줘야 됨.
            Debug.Log("바라봐");
        }
        return newCam;
    }
}
