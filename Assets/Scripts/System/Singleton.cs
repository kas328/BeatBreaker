using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if ((object)_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).ToString());
                    _instance = singletonObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}