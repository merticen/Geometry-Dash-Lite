using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera followCamera;
    public Transform playerOnShip;
   
    public void SwitchToNewTarget()
    {
        followCamera.Follow = playerOnShip;
    }
}
