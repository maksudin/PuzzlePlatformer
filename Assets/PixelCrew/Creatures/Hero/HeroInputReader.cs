using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnCallMenu(InputAction.CallbackContext obj)
        {
            if (obj.performed)
                _hero.CallMenu();
        }

        public void NextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.NextItem();
        }


        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnSaySomething(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.Interact();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.Attack();
        }

        public void OnUseInventory(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.UseInventory(context.interaction);
        }

        public void OnUseCandle(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.UseCandle();
        }
    }
}

