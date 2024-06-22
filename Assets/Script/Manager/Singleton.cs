//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Singleton<T> : MonoBehaviour where T : Component
//{
//    protected static T instance;

//    //public static bool HasInstance => instance != null;
//    //public static T TryGetInstance() => HasInstance ? instance : null;

//    public static T Instance
//    {
//        get
//        {
//            //if (instance == null)
//            //{
//            //    instance = FindAnyObjectByType<T>();
//            //    if (instance == null)
//            //    {
//            //        var go = new GameObject(name: typeof(T).Name + " (Auto-Generated)");
//            //        instance = go.AddComponent<T>();
//            //    }
//            //}
//            return instance;
//        }
//    }

//    protected virtual void Awake()
//    {
//        InitializeSingleton();
//    }

//    protected virtual void InitializeSingleton()
//    {
//        //if (!Application.isPlaying) return;

//        instance = this as T;
//    }

//    //protected virtual void OnDestroy()
//    //{
//    //    if (instance == this)
//    //    {
//    //        instance = null;
//    //    }
//    //}
//}

using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    public static T Instance;

    protected virtual void Awake()
    {
        Instance = (T)this;
    }
}