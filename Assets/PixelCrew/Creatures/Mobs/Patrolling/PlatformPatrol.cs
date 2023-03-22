using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [Header("Constraints"), SerializeField]
        private Vector2 _boxSize = new Vector2(0.18f, 0.57f),
                        _leftConstraintOffset = new Vector2(-0.31f, -0.97f),
                        _rightConstraintOffset = new Vector2(0.31f, -0.97f);

        [Header("Raycast")]
        [SerializeField] private float _raycastDistance = 0.49f;
        [SerializeField] private Vector2 _raycastOffset;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private string[] _tags;
        private Creature _creature;
        private readonly Collider2D[] _collideResult = new Collider2D[2];

        private void Awake() => _creature = GetComponent<Creature>();

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawLine(
                new Vector3(transform.position.x + _leftConstraintOffset.x, transform.position.y + _leftConstraintOffset.y, transform.position.z),
                new Vector3(transform.position.x + _leftConstraintOffset.x, transform.position.y + _leftConstraintOffset.y + _boxSize.y, transform.position.z)
            );
            Handles.DrawLine(
                new Vector3(transform.position.x + _rightConstraintOffset.x, transform.position.y + _rightConstraintOffset.y, transform.position.z),
                new Vector3(transform.position.x + _rightConstraintOffset.x, transform.position.y + _rightConstraintOffset.y + _boxSize.y, transform.position.z)
            );
        }
#endif

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                _creature.Direction = ChooseDirection(_creature.Direction).normalized;
                AvoidWall();
                yield return null;
            }
        }

        private Vector2 ChooseDirection(Vector2 originalDirection)
        {
            var oneGroundExist = CheckPlatformExists(_leftConstraintOffset, _boxSize);
            var twoGroundExist = CheckPlatformExists(_rightConstraintOffset, _boxSize);

            if (oneGroundExist && twoGroundExist)
            {
                if (originalDirection.x == 0)
                    return new Vector2(-1, 0);
                return originalDirection;
            }
            else if (oneGroundExist)
                return new Vector2(-1, 0);
            else if (twoGroundExist)
                return new Vector2(1, 0);
            else
                return Vector2.zero;
        }

        private bool CheckPlatformExists(Vector2 offset, Vector2 boxSize)
        {
            var boxPosition = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
            var size = Physics2D.OverlapBoxNonAlloc(boxPosition, boxSize, 0, _collideResult, _layer);

            for (var i = 0; i < size; i++)
            {
                Collider2D overlapResult = _collideResult[i];
                foreach (var tag in _tags)
                    if (overlapResult.CompareTag(tag))
                        return true;
            }

            return false;
        }

        private void AvoidWall()
        {
            Vector3 raycastDirection = (_creature.Direction.x > 0) ? Vector3.right : Vector3.left;
            var rayPosition = new Vector2(transform.position.x + _raycastOffset.x, transform.position.y + _raycastOffset.y);
            var hit = Physics2D.Raycast(rayPosition, raycastDirection, _raycastDistance, _layer);
            if (hit.collider == null) return;

            foreach (var tag in _tags)
                if (hit.collider.tag == tag)
                    InvertDirection();
            
        }

        private void InvertDirection()
        {
            if (_creature.Direction.x > 0)
                _creature.Direction = Vector3.left;
            else if (_creature.Direction.x < 0)
                _creature.Direction = Vector3.right;
        }
    }
}