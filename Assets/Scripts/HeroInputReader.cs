using PixelCrew.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{
    public class HeroInputReader : MonoBehaviour
    {

        [SerializeField] private Hero _hero;
        private HeroInputAction _inputActions;

        private void Awake()
        {
            _inputActions = new HeroInputAction();
            _inputActions.Hero.Movement.performed += OnMovement;
            _inputActions.Hero.Movement.canceled += OnMovement;
            _inputActions.Hero.SaySomething.performed += OnSaySomething;
            _inputActions.Hero.Interact.canceled += OnInteract;
            _inputActions.Hero.Attack.canceled += OnAttack;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();

            _hero.SetDirection(direction);
        }

        private void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.SaySomething();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {

                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {

                _hero.Attack();
            }
        }
    }
}

