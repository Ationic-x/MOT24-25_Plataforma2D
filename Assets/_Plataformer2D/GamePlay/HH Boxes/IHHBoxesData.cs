using UnityEngine;
using System.Collections.Generic;
using System;

public struct HHBox
{

    public Vector3 offset;
    public Vector2 size;

    public HHBox(Vector2 newOffset, Vector2 newSize)
    {
        offset = newOffset;
        size = newSize;
    }
}

public interface IHHBoxesData
{
	public static PlayerHHBoxesData Instance;
	public Dictionary<Tuple<string, string>, HHBox> Hitbox { get;}
    public Dictionary<Tuple<string, string>, HHBox[]> Hurtbox { get;}
}
