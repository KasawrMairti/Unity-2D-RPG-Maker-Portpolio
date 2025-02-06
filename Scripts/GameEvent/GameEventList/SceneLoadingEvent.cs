using System.Collections;
using UnityEngine;

public class SceneLoadingEvent : GameEvent
{
    public string sceneName;
    // 추가로 로딩 후 플레이어가 이동해야할 위치 만들기
    public Vector2 movePos;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        LoadingManager.Instance.LoadScene(sceneName, movePos);

        yield return null;

        IsEventEnd = true;
    }
}
