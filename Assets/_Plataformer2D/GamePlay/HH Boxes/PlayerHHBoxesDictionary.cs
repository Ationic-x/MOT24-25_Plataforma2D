using System.Collections.Generic;
using UnityEngine;

public class PlayerHHBoxesDictionary : IHHBoxesDictionary
{
    private static PlayerHHBoxesDictionary _instance;
    public static PlayerHHBoxesDictionary Instance => _instance ??= new PlayerHHBoxesDictionary();

    public Dictionary<string, HHBox> Hitbox { get; private set; } = new Dictionary<string, HHBox>();
    public Dictionary<string, HHBox> Hurtbox { get; private set; } = new Dictionary<string, HHBox>();

    private PlayerHHBoxesDictionary()
    {
        Setup();
    }

    private void Setup()
    {
        HHBox idleBox = new HHBox(new Vector2(0, 0.69f), new Vector2(0.7f, 1.25f), HHBox.HHBoxType.HitBox);
        for (int i = 0; i <= 7; i++)
        {
            Hitbox.Add("HeroKnight_" + i, idleBox);
        }
    }
}
