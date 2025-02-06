using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : GameManager<LoadingManager>
{
    private string beforeScene;
    private string nextScene;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image blackObject;
    [SerializeField] private GameObject sliderObject;

    public Vector2 lastPos { get; private set; }
    public bool IsSceneLoad { get; private set; }

    protected override void Awake()
    {
        beforeScene = SceneManager.GetActiveScene().name;

        //LoadScene("PrologueScene");
        LoadScene("LobbyScene");
        //LoadScene("BattleScene_Stage1");
    }

    public void LoadScene(string nextName)
    {
        IsSceneLoad = true;
        nextScene = nextName;
        lastPos = Vector2.zero;

        StartCoroutine(FadeIn());
    }

    public void LoadScene(string nextName, Vector2 pos)
    {
        LoadScene(nextName);

        lastPos = pos;
    }

    IEnumerator LoadScene()
    {
        yield return null;

        // Next Scene Load
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);

                if (progressBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);

                if (progressBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;


                    progressBar.value = 0.0f;
                }
            }
        }

        // Before Scene UnLoad
        //SceneManager.UnloadSceneAsync(beforeScene);
        beforeScene = nextScene;

        IsSceneLoad = false;

        yield return null;

        if (lastPos != Vector2.zero)
            ObjectManager.objects["Player"].transform.position = lastPos;

        yield return null;

        StartCoroutine(FadeOut());


        yield break;
    }

    IEnumerator FadeOut()
    {
        sliderObject.SetActive(false);

        for (float i = 1.0f; i > 0.0f; i -= 0.1f)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            blackObject.color = new Color(0.0f, 0.0f, 0.0f, i);
        }

        canvas.SetActive(false);

        progressBar.value = 0.0f;

        yield break;
    }

    IEnumerator FadeIn()
    {
        canvas.SetActive(true);

        for (float i = 0.0f; i < 1.1f; i += 0.1f)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            blackObject.color = new Color(0.0f, 0.0f, 0.0f, i);
        }

        sliderObject.SetActive(true);
        SystemManager.isControl = SystemManager.Control.CONTROL;

        nextSceneInitialize();
        StartCoroutine(LoadScene());

        yield break;
    }

    private void nextSceneInitialize()
    {
        StageManager.Instance.ClearFuction();
    }
}
