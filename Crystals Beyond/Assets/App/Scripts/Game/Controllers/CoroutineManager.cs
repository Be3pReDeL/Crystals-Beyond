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

    // Потокобезопасный запуск корутины
    public static void StartRoutine(IEnumerator coroutine)
    {
        if (coroutine == null) return;
        
        if (_instance == null)
        {
            var manager = new GameObject("CoroutineManager");
            _instance = manager.AddComponent<CoroutineManager>();
            DontDestroyOnLoad(manager);
        }

        _instance.StartCoroutineSafely(coroutine);
    }

    private void StartCoroutineSafely(IEnumerator coroutine)
    {
        if (this != null && gameObject != null)
            StartCoroutine(coroutine);
    }
}
