using Unity.VisualScripting;
using UnityEngine;

public class LightBanditHHBoxes : MonoBehaviour
{
    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private bool _drawGizmo;
    [SerializeField] private LayerMask _CollisionLayer;
    private HHBoxesController _hhBoxesController;

    private void Awake()
    {
        _hhBoxesController = new HHBoxesController(LightBanditHHBoxesDictionary.Instance, transform, _CollisionLayer);
    }

    private void OnEnable()
    {
        _enemyAnimation.OnSpriteChange += OnSpriteChange;
    }

    private void OnDisable()
    {
        _enemyAnimation.OnSpriteChange -= OnSpriteChange;
    }


    private void OnSpriteChange(string sprite)
    {
        _hhBoxesController.UpdateBoxes(sprite);
    }

    private void Update()
    {
    }


    private void OnDrawGizmosSelected()
    {
        if (_drawGizmo)
        {
            if (_hhBoxesController == null) return;
            HHBox hitbox = _hhBoxesController.CurrentHitbox;
            HHBox[] hurtboxes = _hhBoxesController.CurrentHurtboxes;

            Gizmos.color = new Color(0, 1, 0, 0.3f);

            Gizmos.DrawCube(transform.position + hitbox.offset, hitbox.size);

            Gizmos.color = new Color(1, 0, 0, 0.3f);

            foreach (HHBox hurtbox in hurtboxes)
            {
                // in future we will handle
                Gizmos.DrawCube(transform.position + hurtbox.offset, hurtbox.size);
            }

        }
    }
}
