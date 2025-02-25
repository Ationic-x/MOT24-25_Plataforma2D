using Unity.VisualScripting;
using UnityEngine;

public class PlayerHHBoxes : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private bool _drawGizmo;
    private HHBoxesController _hhBoxesController;

	private void Awake()
	{
		_hhBoxesController = new HHBoxesController(PlayerHHBoxesDictionary.Instance, transform);
	}

	private void OnEnable()
    {
        _playerAnimation.OnSpriteChange += OnSpriteChange;
    }
    
    private void OnDisable()
    {
        _playerAnimation.OnSpriteChange -= OnSpriteChange;
    }


    private void OnSpriteChange(string sprite)
    {
        _hhBoxesController.UpdateBoxes(sprite);
    }


	private void OnDrawGizmosSelected()
	{
		if (_drawGizmo)
		{
			if (_hhBoxesController == null) return;

			Gizmos.color = new Color(0, 1, 0, 0.3f);

			HHBox currentBox = _hhBoxesController.CurrentBox;
			Gizmos.DrawCube(transform.position + currentBox.offset, currentBox.size);

			Gizmos.color = Color.red;
		}
	}
}
