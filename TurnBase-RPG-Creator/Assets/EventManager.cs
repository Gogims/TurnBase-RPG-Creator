using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static EventManager instance;

    public delegate void OnStart();
    public event OnStart StatesOnStart;
    public delegate void OnBattle();
    public event OnBattle StatesOnBattle;
    public UnityEvent DebugLog;    

    private EventManager()
    {
        DebugLog = new UnityEvent();
    }

    public static EventManager Instance()
    {
        if (instance == null)
        {
            instance = new EventManager();
            
        }

        return instance;
    }
}
