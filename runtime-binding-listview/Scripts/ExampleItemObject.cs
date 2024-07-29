using System;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItemObject  
{
    public List<Item> items = new();

    public void Reset()
    {
        items = new List<Item>{
            new() { name = "Use Local Serverdfsdfsd", enabled = false },
            new() { name = "Show Debug Menu", enabled = false },
            new() { name = "Show FPS Counter", enabled = true },
        };
    }

    // Use a struct instead of a class to ensure that the ListView can create items 
    // when the + button is clicked.
    public struct Item
    {
        public bool enabled;
        public string name;
    }
}
