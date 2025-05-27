using System;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItemObject  
{
    public List<Item> items = new();

    public void Reset()
    {
        items = new List<Item>{
            new() { name = "Use Local Server", enabled = false },
            new() { name = "Show Debug Menu", enabled = false },
            new() { name = "Show FPS Counter", enabled = true },
        };
    }

    public struct Item
    {
        public bool enabled;
        public string name;
    }
}
