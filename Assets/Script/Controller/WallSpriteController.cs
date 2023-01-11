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

        wall.sprite = gameObject.name.Contains("VeryLargeWall") ? theme.veryLargeWall : wall.sprite;
        wall.sprite = gameObject.name.Contains("LargeWall") ? theme.largeWall : wall.sprite;
        wall.sprite = gameObject.name.Contains("MediumWall") ? theme.mediumWall : wall.sprite;
        wall.sprite = gameObject.name.Contains("SmallWall") ? theme.smallWall : wall.sprite;
        wall.sprite = gameObject.name.Contains("Spin") ? theme.mediumWall : wall.sprite;
    }
}
