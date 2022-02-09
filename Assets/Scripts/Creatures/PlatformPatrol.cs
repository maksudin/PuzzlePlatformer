using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using PixelCrew.Utils;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {
        private readonly Collider2D[] _collideResult = new Collider2D[2];
        [SerializeField] private Vector2 _boxSize;
        [SerializeField] private Vector2 _groundBoxOneOffset;
        [SerializeField] private Vector2 _groundBoxTwoOffset;
        [SerializeField] private float _raycastDistance;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string _tag;

        private Creature _creature;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawLine(
                new Vector3(transform.position.x + _groundBoxOneOffset.x, transform.position.y + _groundBoxOneOffset.y, transform.position.z),
                new Vector3(transform.position.x + _groundBoxOneOffset.x, transform.position.y + _groundBoxOneOffset.y + _boxSize.y, transform.position.z)
            );
            Handles.DrawLine(
                new Vector3(transform.position.x + _groundBoxTwoOffset.x, transform.position.y + _groundBoxTwoOffset.y, transform.position.z),
                new Vector3(transform.position.x + _groundBoxTwoOffset.x, transform.position.y + _groundBoxTwoOffset.y + _boxSize.y, transform.position.z)
            );

            //Vector3 raycastDirection = (_creature.Direction.x > 0) ? Vector3.right : Vector3.left;
            //var hit = Physics2D.Raycast(transform.position, Vector3.left, _raycastDistance, _mask);

            //if (hit.point != Vector2.zero)
            //    Handles.DrawLine(transform.position, hit.point);
        }

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
            var oneGroundExist = CheckPlatformExists(_groundBoxOneOffset, _boxSize);
            var twoGroundExist = CheckPlatformExists(_groundBoxTwoOffset, _boxSize);

            if (oneGroundExist && twoGroundExist)
            {
                if (originalDirection.x == 0)
                {
                    return new Vector2(-1, 0);
                }
                return originalDirection;

            }
            else if (oneGroundExist)
            {
                return new Vector2(-1, 0);
            }
            else if (twoGroundExist)
            {
                return new Vector2(1, 0);
            }
            else
            {
                return Vector2.zero;
            }
        }

        private bool CheckPlatformExists(Vector2 offset, Vector2 boxSize)
        {
            var boxPosition = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
            var size = Physics2D.OverlapBoxNonAlloc(boxPosition, boxSize, 0, _collideResult, _mask);

            for (var i = 0; i < size; i++)
            {
                Collider2D overlapResult = _collideResult[i];
                if (overlapResult.CompareTag(_tag))
                {
                    return true;
                }
            }

            return false;
        }

        private void AvoidWall()
        {
            Vector3 raycastDirection = (_creature.Direction.x > 0) ? Vector3.right : Vector3.left;
            var hit = Physics2D.Raycast(transform.position, raycastDirection, _raycastDistance, _mask);
            if (hit.collider == null) return;

            if (hit.collider.tag == _tag)
            {
                InvertDirection();
            }
        }

        private void InvertDirection()
        {
            if (_creature.Direction.x > 0)
            {
                _creature.Direction = Vector3.left;
            } 
            else if (_creature.Direction.x < 0)
            {
                _creature.Direction = Vector3.right;
            }
        }


    }
}