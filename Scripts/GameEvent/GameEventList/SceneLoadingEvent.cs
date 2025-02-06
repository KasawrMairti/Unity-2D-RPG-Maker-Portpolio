using System.Collections;
using UnityEngine;

public class SceneLoadingEvent : GameEvent
{
    public string sceneName;
    // �߰��� �ε� �� �÷��̾ �̵��ؾ��� ��ġ �����
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
