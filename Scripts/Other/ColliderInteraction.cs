using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class ColliderInteraction : MonoBehaviour
{
    private ColliderSensor colliderSensor;

    [SerializeField] private GameEventData gameEventData;
    private bool isUse = false;

    private void Awake()
    {
        colliderSensor = GetComponentInChildren<ColliderSensor>();

        ObjectManager.Add("admiral", gameObject);
    }

    private void OnEnable()
    {
        transform.position = new(transform.position.x, transform.position.y, transform.position.y / 100f);
    }

    private void Update()
    {
        if (colliderSensor.b_IsTarget && SystemManager.Instance.Is_Touched && SystemManager.isControl == SystemManager.Control.CONTROL && !isUse)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(SystemManager.Instance.moveMousePoint, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<ColliderInteraction>(out var item))
                {
                    if (item == this)
                    {
                        GameEventManager.Instance.EventTrigger(item.gameEventData);
                        isUse = true;
                        break;
                    }
                }
            }
        }
    }
}
