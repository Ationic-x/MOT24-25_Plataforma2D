using System.Collections.Generic;
using UnityEngine;

public class PlayerHHBoxesDictionary : IHHBoxesDictionary
{
    private static PlayerHHBoxesDictionary _instance;
    public static PlayerHHBoxesDictionary Instance => _instance ??= new PlayerHHBoxesDictionary();

    public Dictionary<string, HHBox> Hitbox { get; private set; } = new Dictionary<string, HHBox>();
    public Dictionary<string, HHBox[]> Hurtbox { get; private set; } = new Dictionary<string, HHBox[]>();
    
    private const string BaseName = "HeroKnight_";

    private PlayerHHBoxesDictionary()
    {
        Setup();
    }

    private void Setup()
    {
        HHBox basicBox = new HHBox(new Vector2(0, 0.69f), new Vector2(0.7f, 1.25f), HHBox.HHBoxType.HitBox);
        HHBox runBox = new HHBox(new Vector2(0, 0.69f), new Vector2(1.7f, 2.25f), HHBox.HHBoxType.HitBox);
        // Idle animations
        for (int i = 0; i <= 7; i++)
        {
            Hitbox.Add(BaseName + i, basicBox);
        }

        HHBox test = new HHBox(new Vector2(0.3f, 0.69f), new Vector2(0.7f, 1.25f), HHBox.HHBoxType.HitBox);
        HHBox[] hurtest = {test};
        // Run animations
        for (int i = 8; i <= 18; i++)
        {
            Hitbox.Add(BaseName + i, runBox);
            Hurtbox.Add(BaseName + i, hurtest);
        }
    }
}
