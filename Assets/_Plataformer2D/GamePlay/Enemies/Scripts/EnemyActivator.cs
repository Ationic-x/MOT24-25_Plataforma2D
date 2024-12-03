using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField] private float _maxDistance;

    [SerializeField] private string _targetTag = "Player";
    private Transform _target;

    Transform[] _children;

    [SerializeField, Range(1,30)] private float frameRate = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _target = GameObject.Find(_targetTag).transform;
        if (_target == null) Debug.LogError($"Tag {_targetTag} not found");

        _children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _children[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % frameRate == 0)
        {
            for (int i = 0; i < _children.Length; i++)
            {
                _children[i].gameObject.SetActive(Vector2.Distance(_target.position, _children[i].position) < _maxDistance);
            }
        }
    }
}
