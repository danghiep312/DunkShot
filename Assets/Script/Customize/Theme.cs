using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Asset/Theme")]
public class Theme : ScriptableObject
{
    public int id;
    public string themeName;
    public Sprite themeIcon;
    
    [Header("Background")]
    public Sprite lightBackground;
    public Sprite nightBackground;
    public Sprite lightOffsetBackground;
    public Sprite nightOffsetBackground;
    public Sprite secondNightOffsetBackground;
    
    
    [Header("Active Hoop")]
    public Sprite backHoop;
    public Sprite frontHoop;
    
    [Header("Inactive Hoop")]
    public Sprite backHoopInactive;
    public Sprite frontHoopInactive;
    
    [Header("Wall")]
    public Sprite veryLargeWall;
    public Sprite largeWall;
    public Sprite mediumWall;
    public Sprite smallWall;
    
    public int price;
}
