 using Sirenix.OdinInspector;
 using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Asset/Skin")]
public class Skin : ScriptableObject
{
    [HorizontalGroup("Ball Data", 75)]
    public int id;
    [PreviewField]
    public Sprite ballSprite;
    [PreviewField]
    public Sprite offsetSprite;
    
    [PreviewField(75)]
    public Sprite perfectSprite;
    public Gradient colorOfFlame;
    public Gradient smokeColor;
    
    [TextArea]
    public string description;

    public int price;

    /// <summary>
    /// 0 => "PlayGame",
    /// 1 => "Bounce",
    /// 2 => "PerfectStreak",
    /// 3 => "Star",
    /// 4 => "BestScore",
    /// </summary>
    public int paramToCheck;

}
