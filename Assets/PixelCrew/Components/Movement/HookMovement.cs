using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace Assets.PixelCrew.Components.Movement
{
    public class HookMovement : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _speed;
        private Hero _hero;

        private void Start() => _hero = FindObjectOfType<Hero>();

        public void HookTo(GameObject go)
        {
            var positions = new Vector3[2] { _hero.transform.position,
                                             go.transform.position };
            DrawLine(positions);
            _hero.HookTo(go);
        }

        private void DrawLine(Vector3[] positions)
        {
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
        }

        public void ResetLine() => _lineRenderer.positionCount = 0;
    }
}