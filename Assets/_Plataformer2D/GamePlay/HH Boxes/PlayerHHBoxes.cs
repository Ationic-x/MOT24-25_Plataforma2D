using Unity.VisualScripting;
using UnityEngine;

public class PlayerHHBoxes : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private HHBoxesController _hhBoxesController;

    private void OnEnable()
    {
        _playerAnimation.OnSpriteChange += UpdateBoxes;
    }
    
    private void OnDisable()
    {
        _playerAnimation.OnSpriteChange -= UpdateBoxes;
    }


    private void UpdateBoxes(string sprite)
    {
        _hhBoxesController.UpdateBoxes(sprite);
    }
}
