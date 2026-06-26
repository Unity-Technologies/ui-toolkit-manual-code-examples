using UnityEngine;

public class Car : MonoBehaviour
{
    public string make = "Toyota";
    public int yearBuilt = 1980;
    public Color color = Color.black;

    // This car has four tires.
    public Tire[] tires = new Tire[4];
}