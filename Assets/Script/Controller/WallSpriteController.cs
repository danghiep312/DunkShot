using System;
using UnityEngine;

public class WallSpriteController : MonoBehaviour
{
    public SpriteRenderer wall;
    public Theme theme;
    public void Start()
    {
        this.RegisterListener(EventID.ThemeClick, (param) => OnThemeClick((int)param));
        wall = GetComponent<SpriteRenderer>();
        theme = GameSpriteMachine.Instance.FindThemeById(PlayerPrefs.GetInt("ThemeID", 10));
    }

    private void OnThemeClick(int id)
    {
        theme = GameSpriteMachine.Instance.FindThemeById(id);
    }


    public void Update()
    {
        wall.sprite = gameObject.name switch
        {
            "VeryLargeWall(Clone)" => theme.veryLargeWall,
            "LargeWall(Clone)" => theme.largeWall,
            "MediumWall(Clone)" => theme.mediumWall,
            "SmallWall(Clone)" => theme.smallWall,
            "Spin(Clone)" => theme.mediumWall,
            _ => wall.sprite
        };
    }
}
