using System.Collections;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    // �� �̺�Ʈ Ÿ�� ����
    public enum Type 
    { ScreenDialogueEvent, TalkDialogueEvent, ChoiceEvent, MovingControllerEvent ,MovingOrderEvent, MovingTeloportEvent, CameraLimit, CameraMoving, 
    Wait, FadeIn, FadeOut, ScreenShake, BGM, SceneLoading, DoorEvent, SetActiveEvent, UIVisiableEvent, VariableEvent, VariableRandomEvent, VariableConditionEvent, ObjectSpawnEvent, GameExit, GameBadEnding, CustomScript }

    protected bool IsEventStart = false; // �ڷ�ƾ 1�� ���� Trigger() �� ����
    protected bool IsEventEnd = false; // �ڽ� Ŭ���� �ڷ�ƾ ���� ������ ����

    private void Awake()
    {
        Initialize();
    }

    public bool Trigger()
    {
        if (IsEventStart)
        {
            if (IsEventEnd)
            {
                IsEventStart = false;
                IsEventEnd = false;
                // print($"Event End : {GetType()}"); // ���� �̺�Ʈ �۵��ߴ��� ����� ��

                return true;
            }
            else return false;
        }
        else
        {
            StartCoroutine(Event());

            IsEventStart = true;

            return false;
        }
    }

    protected abstract void Initialize();

    protected abstract IEnumerator Event();


    // 1. ���̾�α� �̺�Ʈ | ĳ���� ��ȭ �̺�Ʈ
    // 2. ������ �̺�Ʈ
    // 3. ĳ������ �̵���� �̺�Ʈ | ���� �ڷ���Ʈ �̺�Ʈ
    // 4. ī�޶� ���� �̺�Ʈ // �÷��̾� ���� ī�޶� ��� ī�޶� �̵���Ű�� ��
    // 5. ĳ���� Enable/Disable �̺�Ʈ
    // 6. ���(Wait) ���� �̺�Ʈ // �̺�Ʈ �������� �ð��� ��� �־����� ���ð�
    // 7. ȭ���� ����Ʈ ��/�ƿ� �̺�Ʈ
    // 8. ȭ�� ��鸲 �̺�Ʈ
    // 9. BGM ���/���� �̺�Ʈ
    // 10. Ŀ���� ��ũ��Ʈ �̺�Ʈ
    // 11. �������� �̺�Ʈ // ���������� ���� ��Ű�� ���� ó�� �����й� �̺�Ʈ �۵��� �� �̺�Ʈ�� ������ �۵��Ǿ�� �Ѵ�.
    // 12. �����й� �̺�Ʈ // ��忣���� ó��
}
