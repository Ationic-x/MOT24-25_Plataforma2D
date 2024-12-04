using System;
using UnityEngine;

public class GhosTrail : MonoBehaviour
{    
    private GameObject[] _pool;
    private int _index;

    [Header("Ghost Trail Settings")]
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _ghostLifetime;
    [SerializeField] private Color _trailColor;
    [SerializeField] private bool _available;

    private SpriteRenderer playerSR;
    private float timer;


    private void Awake()
    {
        playerSR = GetComponent<SpriteRenderer>();

        _pool = new GameObject[_poolSize];
		for (int i = 0; i < _poolSize; i++)
		{
            GameObject newObj = Instantiate(_ghostPrefab);
            newObj.SetActive(false);
            _pool[i] = newObj;
		}
    }

    private void Update()
    {
        if (!_available)
            return;

        timer += Time.deltaTime;

        if (timer >= _spawnInterval)
        {
            SpawnGhost();
            timer = 0f;
        }
    }

    private void SpawnGhost()
    {
        GameObject ghost = GetGhost();
        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
        Material material = ghostSR.material;

        ghost.transform.position = transform.position;
        ghostSR.sprite = playerSR.sprite;
        ghostSR.flipX = playerSR.flipX;

        material.SetFloat("_TimeLimit", Time.time + _ghostLifetime);
        material.SetFloat("_Duration", _ghostLifetime);
        material.SetColor("_Color", _trailColor);

        StartCoroutine(DeactivateGhost(ghost));
    }

    private System.Collections.IEnumerator DeactivateGhost(GameObject ghost)
    {
        yield return new WaitForSeconds(_ghostLifetime);
        ghost.SetActive(false);
    }

    public GameObject GetGhost()
    {
        _index = (_index + 1) % _poolSize;
        GameObject obj = _pool[_index];
        obj.SetActive(true);
        return obj;
    }

    public bool Available { get => _available; set => _available = value; }
}
