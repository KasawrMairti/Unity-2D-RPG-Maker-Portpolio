using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SystemManager : GameManager<SystemManager>
{
    public enum Control { CONTROL, NOTCONTROL }
    public static Control isControl;

    public Vector2 moveMousePoint { get; private set; }
    public bool Is_Touched { get; private set; }

    public CinemachineVirtualCamera virtualCamera;

    protected override void Awake()
    {
        base.Awake();

        isControl = Control.CONTROL;
    }

    private void OnMove(InputValue value)
    {
        Is_Touched = value.isPressed;
    }

    private void OnMovePos(InputValue value)
    {
        moveMousePoint = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }
}
