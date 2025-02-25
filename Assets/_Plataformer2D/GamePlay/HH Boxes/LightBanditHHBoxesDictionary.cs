using System.Collections.Generic;
using UnityEngine;

public class LightBanditHHBoxesDictionary : IHHBoxesDictionary
{
    private static LightBanditHHBoxesDictionary _instance;
    public static LightBanditHHBoxesDictionary Instance => _instance ??= new LightBanditHHBoxesDictionary();

    public Dictionary<string, HHBox> Hitbox { get; private set; } = new Dictionary<string, HHBox>();
    public Dictionary<string, HHBox[]> Hurtbox { get; private set; } = new Dictionary<string, HHBox[]>();

    private const string BaseName = "LightBandit_";

    private LightBanditHHBoxesDictionary()
    {
        Setup();
    }

    private void Setup()
    {
        HHBox idleBox = new HHBox(new Vector2(0, 0.69f), new Vector2(0.7f, 1.25f), HHBox.HHBoxType.HitBox);
        for (int i = 0; i <= 20; i++)
        {
            Hitbox.Add(BaseName + i, idleBox);
        }
    }
}
