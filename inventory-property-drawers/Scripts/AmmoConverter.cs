using UnityEditor.UIElements;

public class AmmoConverter : UxmlAttributeConverter<Ammo>
{
    public override Ammo FromString(string value)
    {
        var ammo = new Ammo();
        var values = value.Split('/');
        if (values.Length == 2)
        {
            int.TryParse(values[0], out ammo.count);
            int.TryParse(values[1], out ammo.maxCount);
        }
        return ammo;
    }

    public override string ToString(Ammo value)
    {
        return $"{value.count}/{value.maxCount}";
    }
}

