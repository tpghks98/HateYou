using UnityEngine;
using System.Collections;

public abstract class SingleTon<T> : MonoBehaviour where T : MonoBehaviour {

    private static T m_pInstance = null;

    public static T Instance
    {
        get
        {
            if (m_pInstance == null)
            {
                m_pInstance = (T)FindObjectOfType(typeof(T));
                if (m_pInstance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).ToString();
                    m_pInstance = go.AddComponent<T>();
                }
            }
            return m_pInstance;
        }
    }


    virtual public void Initialize()
    {
        Full_Static();
    }
    protected void Full_Static()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
