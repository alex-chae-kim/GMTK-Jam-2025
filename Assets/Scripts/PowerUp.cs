using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "PowerUp", menuName = "Scriptable Objects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string name;
    public string description;
    public Sprite image;
    public int count = 0;
    public float speedBuff;
    public float jumpBuff;
    public float lifeBuff;
    public string special;
    public Color color;
}
