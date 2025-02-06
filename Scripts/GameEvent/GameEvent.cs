using System.Collections;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    // 각 이벤트 타입 지정
    public enum Type 
    { ScreenDialogueEvent, TalkDialogueEvent, ChoiceEvent, MovingControllerEvent ,MovingOrderEvent, MovingTeloportEvent, CameraLimit, CameraMoving, 
    Wait, FadeIn, FadeOut, ScreenShake, BGM, SceneLoading, DoorEvent, SetActiveEvent, UIVisiableEvent, VariableEvent, VariableRandomEvent, VariableConditionEvent, ObjectSpawnEvent, GameExit, GameBadEnding, CustomScript }

    protected bool IsEventStart = false; // 코루틴 1번 실행 Trigger() 용 변수
    protected bool IsEventEnd = false; // 자식 클래스 코루틴 종료 감지용 변수

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
                // print($"Event End : {GetType()}"); // 무슨 이벤트 작동했는지 디버그 용

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


    // 1. 다이얼로그 이벤트 | 캐릭터 대화 이벤트
    // 2. 선택지 이벤트
    // 3. 캐릭터의 이동명령 이벤트 | 강제 텔레포트 이벤트
    // 4. 카메라 제어 이벤트 // 플레이어 고정 카메라를 벗어나 카메라 이동시키는 것
    // 5. 캐릭터 Enable/Disable 이벤트
    // 6. 대기(Wait) 지연 이벤트 // 이벤트 실행전에 시간을 잠시 주어지는 대기시간
    // 7. 화면의 페이트 인/아웃 이벤트
    // 8. 화면 흔들림 이벤트
    // 9. BGM 재생/정지 이벤트
    // 10. 커스텀 스크립트 이벤트
    // 11. 게임종료 이벤트 // 정상적으로 종료 시키기 위한 처리 게임패배 이벤트 작동후 이 이벤트가 무조건 작동되어야 한다.
    // 12. 게임패배 이벤트 // 배드엔딩시 처리
}
