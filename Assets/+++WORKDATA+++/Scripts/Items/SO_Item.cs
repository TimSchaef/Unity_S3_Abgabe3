using UnityEngine;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable Objects/SO_Item")]
public class SO_Item : ScriptableObject
{
    public string itemID;
    public string itemName;
    [Multiline] public string description;
    public Sprite itemSprite;
}
