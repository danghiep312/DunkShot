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
    
    [HorizontalGroup("Flame Data", 75)]
    [PreviewField(75)]
    [HideLabel]
    public Sprite perfectSprite;
    [VerticalGroup("Flame Data/Perfect")]
    [HideLabel] 
    public Color colorOfFlame;
    
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
