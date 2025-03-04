using System.Collections.Generic;
using UnityEngine;
using System;

public class LightBanditHHBoxesData : IHHBoxesData
{
    private static LightBanditHHBoxesData _instance;
    public static LightBanditHHBoxesData Instance => _instance ??= new LightBanditHHBoxesData();

    public Dictionary<Tuple<string, string>, HHBox> Hitbox { get; private set; } = new Dictionary<Tuple<string, string>, HHBox>();
    public Dictionary<Tuple<string, string>, HHBox[]> Hurtbox { get; private set; } = new Dictionary<Tuple<string, string>, HHBox[]>();

    private const string BaseName = "LightBandit_";

    private LightBanditHHBoxesData()
    {
        Setup();
    }

    private void Setup()
    {
        HHBox idleBox = new HHBox(new Vector2(0, 0.69f), new Vector2(0.7f, 1.25f));
        for (int i = 0; i <= 20; i++)
        {
            Hitbox.Add(Tuple.Create("", BaseName + i), idleBox);
        }
    }
}
