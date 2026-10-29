using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _moveSpeed = 0.4f;

    [Header("Visual")]
    [SerializeField] private GameObject _monsterVisual;

    private RoomMonsterRoot _root;
    private bool _isMoving;
    private bool _hasReachedPlayer;

    public bool IsMoving => _isMoving;
    public bool HasReachedPlayer => _hasReachedPlayer;
    
    public float Progress
    {
        get
        {
            float totalDistance = Vector3.Distance(_startPoint.position,_endPoint.position);

            if (totalDistance <= 0f)
                return 1f;

            float remainingDistance = Vector3.Distance(transform.position,_endPoint.position);
            return Mathf.Clamp01(1f - remainingDistance / totalDistance);
        }
    }

    public void Initialize(RoomMonsterRoot root)
    {
        _root = root;

        ResetMonster();
    }

    private void Update()
    {
        if (!_isMoving || _hasReachedPlayer)
            return;

        transform.position = Vector3.MoveTowards(transform.position,_endPoint.position,_moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _endPoint.position) <= 0.01f)
        {
            _hasReachedPlayer = true;
            _isMoving = false;

            _root.OnPlayerCaught();
        }
    }

    public void StartMoving()
    {
        if (_hasReachedPlayer)
            return;

        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    public void ResetMonster()
    {
        _isMoving = false;
        _hasReachedPlayer = false;

        if (_startPoint != null)
        {
            transform.position = _startPoint.position;
            transform.rotation = _startPoint.rotation;
        }

        if (_monsterVisual != null)
            _monsterVisual.SetActive(true);
    }
}


