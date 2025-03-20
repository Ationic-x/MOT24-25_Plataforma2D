using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerStates _playerStates;

    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new (0.49f, 0.03f);
    [SerializeField] private Transform _forwardCheckPoint;
    [SerializeField] private Transform _backwardCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new (0.5f, 1f);
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _drawGizmo;

    // Start is called before the first frame update
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

	private void OnEnable()
	{
        _playerStates = _playerController.PlayerStates;
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        // False default values
        bool isGrounded = false, isCollidedForward = false, isCollidedBackward = false;

        // Exceptions
        if (!_playerStates.IsDashing && !_playerStates.IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
                isGrounded = true;
        }

        if (!_playerStates.IsWallJumping)
        {
            isCollidedForward = Physics2D.OverlapBox(_forwardCheckPoint.position, _wallCheckSize, 0, _groundLayer);
            isCollidedBackward = Physics2D.OverlapBox(_backwardCheckPoint.position, _wallCheckSize, 0, _groundLayer);

            // Avoid cases where the character is buried
            if (isCollidedForward && isCollidedBackward)
                isCollidedBackward = isCollidedForward = false;
        }


        _playerStates.IsGrounded = isGrounded;
        _playerStates.IsCollidedForward = isCollidedForward;
        _playerStates.IsCollidedBackward = isCollidedBackward;
    }

	void OnDrawGizmosSelected()
    {
        if (_drawGizmo)
        {
            Gizmos.color = Color.yellow;
        
            Gizmos.DrawWireCube(_forwardCheckPoint.position, _wallCheckSize);
            Gizmos.DrawWireCube(_backwardCheckPoint.position, _wallCheckSize);
        
            Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);        
        }
    }
}
