using UnityEngine;
using System.Collections.Generic;

public struct HHBox
{
    public enum HHBoxType
    {
        HitBox,
        HurtBox,
        Both,
    }

    public Vector3 offset;
    public Vector2 size;
    public HHBoxType type;

    public HHBox(Vector2 newOffset, Vector2 newSize, HHBoxType newType)
    {
        offset = newOffset;
        size = newSize;
        type = newType;
    }
}

public interface IHHBoxesDictionary
{
    public Dictionary<string, HHBox> Hitbox { get;}
    public Dictionary<string, HHBox[]> Hurtbox { get;}
}
