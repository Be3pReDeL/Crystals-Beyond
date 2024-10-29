using UnityEngine;
using System.Collections;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;

    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var manager = new GameObject("CoroutineManager");
                _instance = manager.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(manager); // Оставляем менеджер между сценами
            }
            return _instance;
        }
    }

    // Метод для запуска корутины извне
    public void StartExternalCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
