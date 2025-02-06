using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ColliderSensor : MonoBehaviour, IDamageble
{
    private Collider2D sensorCollider;

    public bool b_IsTarget { get; private set; }
    public Vector2 v_targetPosition { get; private set; }
    public bool b_IsDamageble() { return originDamageble != null; }
    private IDamageble originDamageble;

    [SerializeField] private GameObject originMaster;
    [SerializeField] private string tagTarget = "";
    [SerializeField] private string layerTarget = "";

    private void Awake()
    {
        sensorCollider = GetComponent<Collider2D>();
        sensorCollider.isTrigger = true;

        if (originMaster != null)
            originDamageble = originMaster.GetComponent<IDamageble>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (tagTarget != "" ? collision.CompareTag(tagTarget) : false ||
                layerTarget != "" ? collision.gameObject.layer == LayerMask.NameToLayer(layerTarget) : false)
            {
                b_IsTarget = true;
                v_targetPosition = collision.transform.position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (tagTarget != "" ? collision.CompareTag(tagTarget) : false ||
                layerTarget != "" ? collision.gameObject.layer == LayerMask.NameToLayer(layerTarget) : false)
            {
                b_IsTarget = false;
            }
        }
    }

    public Collider2D[] colliders()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, sensorCollider.bounds.size, 0.0f);

        return colliders;
    }

    public void Hit(IDamageble target) { }

    public void Damaged(float life)
    {
        originDamageble?.Damaged(life);
    }
}
