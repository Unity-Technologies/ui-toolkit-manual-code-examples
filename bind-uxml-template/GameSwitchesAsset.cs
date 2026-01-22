using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSwitchAsset.asset", menuName = "GameSwitchAsset")]
public class GameSwitchesAsset : ScriptableObject
{
    public GameSwitch useLocalServer;
    public GameSwitch showDebugMenu;
    public GameSwitch showFPSCounter;

    // Use the Reset function to provide default values
    public void Reset()
    {
        useLocalServer = new GameSwitch() { name = "Use Local Server", enabled = false };
        showDebugMenu = new GameSwitch() { name = "Show Debug Menu", enabled = false };
        showFPSCounter = new GameSwitch() { name = "Show FPS Counter", enabled = true };
    }

    [Serializable]
    public struct GameSwitch
    {
        public string name;
        public bool enabled;
    }
}
