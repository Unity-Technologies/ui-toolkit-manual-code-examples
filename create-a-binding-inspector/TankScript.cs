using UnityEngine;

[ExecuteInEditMode]
public class TankScript : MonoBehaviour
{
    public string tankName = "Tank";
    public float tankSize = 1;

    private void Update()
    {
        gameObject.name = tankName;
        gameObject.transform.localScale = new Vector3(tankSize, tankSize, tankSize);
    }
}