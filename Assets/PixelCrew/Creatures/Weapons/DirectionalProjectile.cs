using PixelCrew.Creatures.Weapons;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Weapons
{
    public class DirectionalProjectile : Projectile
    {
        public void Launch(Vector2 direction)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
        }
    }
}