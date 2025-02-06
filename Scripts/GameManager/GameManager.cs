using UnityEngine;

public class GameManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = FindObjectOfType<T>();
        else if (instance != this)
            Destroy(gameObject);


        if (transform.parent != null && transform.root != null)
            DontDestroyOnLoad(this.transform.root.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }
}