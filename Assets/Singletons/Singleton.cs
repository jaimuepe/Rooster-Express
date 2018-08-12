using UnityEngine;

/// <summary>
/// Class to generate singletons of MonoBehaviour subclasses.
/// </summary>
/// <typeparam name="T">Subclass of the MonoBehaviour.</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    // Used for some weird interactions when trying to access the singleton when it has already been destroyed
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance " + typeof(T) + " already destroyed on application quit.");
                return null;
            }

            if (_instance == null)
            {
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<T>();
                singleton.name = "[Singleton] " + typeof(T).ToString();
                DontDestroyOnLoad(singleton);
            }

            return _instance;
        }
    }

    protected Singleton() { }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
