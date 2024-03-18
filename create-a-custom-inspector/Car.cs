using UnityEngine;

public class Car : MonoBehaviour
{
    public string m_Make = "Toyota";
    public int m_YearBuilt = 1980;
    public Color m_Color = Color.black;

    // This car has four tires.
    public Tire[] m_Tires = new Tire[4];
}