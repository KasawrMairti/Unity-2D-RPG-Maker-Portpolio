using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        ObjectManager.Add("PlayerCamera", gameObject);
    }

    private void Start()
    {
        SystemManager.Instance.virtualCamera = virtualCamera;
    }
}
