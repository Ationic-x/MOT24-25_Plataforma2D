using UnityEngine;

public class EnemyController : ActorController
{
    [SerializeField] private EnemyData _data;
    [SerializeField] private EnemyStates _es;

    public override IStats Stats {
        get => _data;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_data == null) Debug.LogError("Enemy controller doesn't have an EnemyData associated");
    }

#if UNITY_EDITOR
    void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
    void _OnValidate()
    {
        if (this == null) return;
        _data.ApplyGraphics2D(gameObject);
    }
#endif

    public EnemyData Data { get { return _data; } }

    public void SwapFacingPosition(bool isFacingRight)
    {
        bool frConversion = _data.isFacingRight ? isFacingRight : !isFacingRight;
        if (_es.IsFacingRight != frConversion)
            _es.IsFacingRight = frConversion;
    }

    public EnemyStates ES { get => _es; }
}
