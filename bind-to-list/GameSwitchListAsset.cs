using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
  
[CreateAssetMenu(fileName = "GameSwitchListAsset.asset", menuName = "GameSwitchListAsset")]
public class GameSwitchListAsset : ScriptableObject
{
    public List<GameSwitch> switches = new();

    public void Reset()
    {
        switches = new List<GameSwitch>{
            new() { name = "Use Local Server", enabled = false },
            new() { name = "Show Debug Menu", enabled = false },
            new() { name = "Show FPS Counter", enabled = true },
        };
    }

    public bool IsSwitchEnabled(string switchName) => switches.Find(s => s.name == switchName).enabled;

    [Serializable]
    public struct GameSwitch
    {
        public bool enabled;
        public string name;
    }
}
