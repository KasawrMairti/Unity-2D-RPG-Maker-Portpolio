using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCollider : MonoBehaviour
{
    public enum Type { Teleporting, Lighting, SceneLoading }
    public Type type;

    [field: Header("DoorEvent")]
    [field: SerializeField] public bool IsDoor;
    [SerializeField] private EventCollider o_door;

    [Header("Moving")] // type = Type.DoorMoving, Type.Teleporting 에서만 작동
    [Tooltip("IsWorld를 false로 할 경우 오브젝트 기준 위치에서 Local로 이동합니다.")]
    [SerializeField] private bool IsWorld;
    [SerializeField] private Vector2 movePos;

    [Header("Lighting")] // type = Type.DoorMoving, Type.Teleporting, Type.Lighting 에서만 작동
    [SerializeField] private GameObject     objDisable;
    [SerializeField] private GameObject[]   objEnsable;
    private List<GameObject> objDisablechild = new List<GameObject>();
    
    [Header("Scene")] // type = Type.SceneLoading 에서만 작동
    [SerializeField] private string sceneName;

    public Vector2 MovePos
    {
        get 
        {
            if (IsWorld)
            {
                float x = movePos.x != 0.0f ? movePos.x : transform.position.x;
                float y = movePos.y != 0.0f ? movePos.y : transform.position.y;

                return new Vector2(x, y);
            }
            else
            {
                return (Vector2)transform.position + movePos;
            }
        }
    }

    private Animator anim;
    public bool B_AnimOpened
    {
        get
        {
            if (o_door != null)
                return o_door.anim.GetBool("IsOpened");
            else
                return anim.GetBool("IsOpened");
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (ObjectManager.objects.ContainsKey(gameObject.name)) ObjectManager.objects[gameObject.name] = gameObject;
        else ObjectManager.objects.Add(gameObject.name, gameObject);

        transform.position = new(transform.position.x, transform.position.y, transform.position.y / 100f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && SystemManager.isControl == SystemManager.Control.CONTROL)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SystemManager.isControl = SystemManager.Control.NOTCONTROL;
                StartCoroutine(DoorEvent(collision.gameObject));
            }
        }
    }

    private IEnumerator DoorEvent(GameObject obj)
    {
        Player player = obj.GetComponent<Player>();

        switch (type)
        {
            case Type.Teleporting:

                if (IsDoor)
                {
                    DoorEvent(true);
                    yield return new WaitForSeconds(0.5f);
                }

                UIManager.Instance.FadeInEvent(0.5f);
                yield return new WaitUntil(() => UIManager.Instance.b_EventFadeInOut);

                player.navMeshAgent.enabled = false;
                enableEvent();
                player.transform.position = MovePos;
                yield return new WaitForFixedUpdate();
                player.navMeshAgent.enabled = true;
                player.Order(MovePos);
                yield return new WaitForSeconds(0.5f);

                UIManager.Instance.FadeOutEvent(0.5f);
                yield return new WaitUntil(() => UIManager.Instance.b_EventFadeInOut);

                if (IsDoor) DoorEvent(false);

                SystemManager.isControl = SystemManager.Control.CONTROL;
                break;

            case Type.Lighting:
                SystemManager.isControl = SystemManager.Control.CONTROL;
                enableEvent();
                break;

            case Type.SceneLoading:
                if (IsDoor)
                {
                    DoorEvent(true);
                    yield return new WaitForSeconds(0.5f);
                }
                enableEvent();
                break;
        }
    }

    public void enableEvent()
    {
        switch (type)
        {
            case Type.Teleporting:
            case Type.Lighting:
                objDisablechild.Clear();
                for (int i = 0; i < objDisable.transform.childCount; i++)
                {
                    objDisablechild.Add(objDisable.transform.GetChild(i).gameObject);
                }

                foreach (var obj in objDisablechild)
                {
                    obj.SetActive(false);
                }
                foreach (var obj in objEnsable)
                {
                    obj.SetActive(true);
                }
                break;

            case Type.SceneLoading:
                if (IsWorld) LoadingManager.Instance.LoadScene(sceneName, movePos);
                else LoadingManager.Instance.LoadScene(sceneName);
                break;
        }
    }

    public void DoorEvent(bool b_IsOpened)
    {
        EventCollider obj;

        if (o_door != null) obj = o_door;
        else obj = this;

        bool isOpend = obj.anim.GetBool("IsOpened");
        if (b_IsOpened && !isOpend)
            obj.anim.SetBool("IsOpened", true);
        else if (!b_IsOpened && isOpend)
            obj.anim.SetBool("IsOpened", false);
    }
}
