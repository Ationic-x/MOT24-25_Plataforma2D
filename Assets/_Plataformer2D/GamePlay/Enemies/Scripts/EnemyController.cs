using UnityEngine;

public class EnemyController : ActorController
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private EnemyStates _enemyStates;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
		if (_enemyData == null) Debug.LogError("Enemy controller doesn't have an EnemyData associated");
		// This method must be changed, when we have save values in a db
		stats.SetupStats(_enemyData.baseHP, _enemyData.basePhysic, _enemyData.baseMagic, _enemyData.baseDefense, _enemyData.basePush);
	}

#if UNITY_EDITOR
    void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
    void _OnValidate()
    {
        if (this == null) return;
        if (_enemyData == null) return;
        _enemyData.ApplyGraphics2D(gameObject);
    }
#endif

    public EnemyData Data { get { return _enemyData; } }

    public void SwapFacingPosition(bool isFacingRight)
    {
        bool frConversion = _enemyData.isFacingRight ? isFacingRight : !isFacingRight;
        if (_enemyStates.IsFacingRight != frConversion)
            _enemyStates.IsFacingRight = frConversion;
    }

    public EnemyStates ES { get => _enemyStates; }
}
