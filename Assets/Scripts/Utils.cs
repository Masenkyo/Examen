using System;
using UnityEngine;

public static class Utils
{
    static Tracker tracker => _tracker ??= new GameObject("Tracker").AddComponent<Tracker>();
    static Tracker _tracker;
    
    public class Tracker : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public Action OnUpdate = () => { };
        void Update()
        {
            OnUpdate();
        }
    }

    /// <returns>Cancellation token: Invoke to cancel</returns>
    public static Action AddToUpdate(this Action action)
    {
        tracker.OnUpdate += action;
        return new Action(() => tracker.OnUpdate -= action);
    }
}