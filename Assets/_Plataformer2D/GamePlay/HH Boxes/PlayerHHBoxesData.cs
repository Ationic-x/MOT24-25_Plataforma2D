using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHHBoxesData : IHHBoxesData
{
    private static PlayerHHBoxesData _instance;
    public static PlayerHHBoxesData Instance => _instance ??= new PlayerHHBoxesData();

    public Dictionary<Tuple<string, string>, HHBox> Hitbox { get; private set; }
    public Dictionary<Tuple<string, string>, HHBox[]> Hurtbox { get; private set; }

	private const string BaseName = "HeroKnight_";
	private enum AnimStates
	{
		Idle,
		Run,
		IdleBlock,
		Block,
		WallSlide,
		Fall,
		Attack1,
		Attack2,
		Attack3,
		Roll,
		Dash,
		Hurt,
		Death,
		DeathNoBlood,
		Jump
	}

	private readonly Dictionary<AnimStates, string> _clip = new Dictionary<AnimStates, string>
	{
		{ AnimStates.Idle, BaseName + "Idle" },
		{ AnimStates.Run, BaseName + "Run" },
		{ AnimStates.IdleBlock, BaseName + "Idle Block" },
		{ AnimStates.Block, BaseName + "Block" },
		{ AnimStates.WallSlide, BaseName + "Wall Slide" },
		{ AnimStates.Fall, BaseName + "Fall" },
		{ AnimStates.Attack1, BaseName + "Attack1" },
		{ AnimStates.Attack2, BaseName + "Attack2" },
		{ AnimStates.Attack3, BaseName + "Attack3" },
		{ AnimStates.Roll, BaseName + "Roll" },
		{ AnimStates.Dash, BaseName + "Dash" },
		{ AnimStates.Hurt, BaseName + "Hurt" },
		{ AnimStates.Death, BaseName + "Death" },
		{ AnimStates.DeathNoBlood, BaseName + "DeathNoBlood" },
		{ AnimStates.Jump, BaseName + "Jump" }
	};

	private PlayerHHBoxesData()
    {
        Setup();
    }

    private void Setup()
    {
		Hitbox = new Dictionary<Tuple<string, string>, HHBox>();
		Hurtbox = new Dictionary<Tuple<string, string>, HHBox[]>();

		string clip;

		clip = _clip[AnimStates.Idle];
		HHBox basicBox = new HHBox(new Vector2(0, 0.69f), new Vector2(0.7f, 1.25f));
        for (int i = 0; i <= 7; i++)
        {
			Hitbox.Add(Tuple.Create(clip,BaseName + i), basicBox);
        }

		clip = _clip[AnimStates.Run];

		for (int i = 8; i <= 17; i++)
        {
            Hitbox.Add(Tuple.Create(clip, BaseName + i), basicBox);
        }


        // Attack Animations
		HHBox attack = new HHBox(new Vector2(0.5f, 0.95f), new Vector2(2f, 1.5f));
		HHBox[] attackboxes = { attack };
		clip = _clip[AnimStates.Attack1];

		for (int i = 18; i <= 23; i++)
		{
			Hitbox.Add(Tuple.Create(clip, BaseName + i), basicBox);
            if (i == 21) Hurtbox.Add(Tuple.Create(clip, BaseName + i), attackboxes);
            if (i == 22) Hurtbox.Add(Tuple.Create(clip, BaseName + i), attackboxes);
            if (i == 23) Hurtbox.Add(Tuple.Create(clip, BaseName + i), attackboxes);
		}
	}
}
