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

    public static Action OnUp(this KeyCode code, Action doWhat)
    {
        tracker.OnUpdate += temp;
        void temp()
        {
            if (Input.GetKeyUp(code))
                doWhat();
        }
        return new Action(() => tracker.OnUpdate -= temp);
    }
    
    public static Action OnDown(this KeyCode code, Action doWhat)
    {
        tracker.OnUpdate += temp;
        void temp()
        {
            if (Input.GetKeyDown(code))
                doWhat();
        }
        return new Action(() => tracker.OnUpdate -= temp);
    }
}