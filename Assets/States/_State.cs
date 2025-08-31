using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game State/Bool Variable")]
public class BoolVariable : ScriptableObject
{
    [SerializeField] private bool value;
    private bool lastValue;
    private List<Action<bool>> listeners = new List<Action<bool>>() { };

    public bool Value
    {
        get => value;
        set
        {
            if (value == lastValue) return;
            lastValue = value;
            foreach (var listener in listeners)
            {
                listener?.Invoke(value);
            }
        }
    }

    private void Awake()
    {
        lastValue = !value;
    }

    public void AddListener(Action<bool> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(Action<bool> listener)
    {
        listeners.Remove(listener);
    }
}