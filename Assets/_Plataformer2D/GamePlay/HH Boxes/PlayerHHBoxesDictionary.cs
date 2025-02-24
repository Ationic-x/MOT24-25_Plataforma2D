using System.Collections.Generic;
using UnityEngine;

public class PlayerHHBoxesDictionary : MonoBehaviour, IHHBoxesDictionary
{
    private Dictionary<string, HHBox> _hitbox;
    private Dictionary<string, HHBox> _hurtbox;
    private const string BaseName = "HeroKnight_";

    private void Awake()
    {
        _hitbox = new Dictionary<string, HHBox>();
        _hurtbox = new Dictionary<string, HHBox>();
        Setup();
    }

    private void Setup()
    {
        #region Idle
        // Range between 0 - 7
        HHBox idleBox = new (new Vector2(0, 0), new Vector2(1, 1), HHBox.HHBoxType.HitBox);
        _hitbox.Add(BaseName + 0, idleBox);
        _hitbox.Add(BaseName + 1, idleBox);
        _hitbox.Add(BaseName + 2, idleBox);
        _hitbox.Add(BaseName + 3, idleBox);
        _hitbox.Add(BaseName + 4, idleBox);
        _hitbox.Add(BaseName + 5, idleBox);
        _hitbox.Add(BaseName + 6, idleBox);
        _hitbox.Add(BaseName + 7, idleBox);
        #endregion
    }

    public Dictionary<string, HHBox> Hitbox { get => _hitbox;}

    public Dictionary<string, HHBox> Hurtbox { get => _hurtbox;}
}
