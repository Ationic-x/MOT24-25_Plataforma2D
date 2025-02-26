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
        // Idle animations
        for (int i = 0; i <= 7; i++)
        {
            Hitbox.Add(BaseName + i, basicBox);
        }

        // Run animations
        for (int i = 8; i <= 17; i++)
        {
            Hitbox.Add(BaseName + i, basicBox);
        }


        // Attack Animations
		HHBox attack = new HHBox(new Vector2(0.5f, 0.95f), new Vector2(2f, 1.5f), HHBox.HHBoxType.HitBox);
		HHBox[] attackboxes = { attack };

		for (int i = 18; i <= 23; i++)
		{
			Hitbox.Add(BaseName + i, basicBox);
            if (i == 21) Hurtbox.Add(BaseName + i, attackboxes);
            if (i == 22) Hurtbox.Add(BaseName + i, attackboxes);
            if (i == 23) Hurtbox.Add(BaseName + i, attackboxes);
		}
	}
}
