using System;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlObject]
public partial class HealthPack : Item
{
    [UxmlAttribute]
    public float healAmount = 100;

    public HealthPack()
    {
        name = "Health Pack";
    }
}

[UxmlObject]
public partial class Sword : Item
{
    [UxmlAttribute, Range(1, 100)]
    public float slashDamage;
}

[Serializable]
public class Ammo
{
    public int count;
    public int maxCount;
}

[UxmlObject]
public partial class Gun : Item
{
    [UxmlAttribute]
    public float damage;

    [UxmlAttribute]
    public Ammo ammo = new Ammo { count = 10, maxCount = 10 };
}

