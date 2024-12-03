using UnityEngine;

public class EnemyController : ActorController
{
    [SerializeField] protected PlayerData stats;

    public override IStats Stats {
        get => stats;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (stats == null) Debug.LogError("Enemy controller doesn't have an EnemyData associated");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
