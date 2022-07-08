using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace Assets.PixelCrew.Utils
{
    public class AntiFlipUtil : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        private void Update()
        {
            if (_hero.Direction == Vector2.right)
                transform.localScale = Vector3.one;
            else if (_hero.Direction == Vector2.left)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}