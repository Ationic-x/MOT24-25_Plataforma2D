using UnityEngine;

public class EnemyController : ActorController
{
    [SerializeField] private EnemyData _data;

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

    public EnemyData Data { get { return _data; } }
#endif
}
