using UnityEngine;

[CreateAssetMenu(fileName = "Challenge", menuName = "Asset/Challenge")]
public class Level : ScriptableObject
{
    public int id;
    public int totalHoop;
    public int target;
    public int reward;
    public string type;
    
    [TextArea]
    public string description;
}