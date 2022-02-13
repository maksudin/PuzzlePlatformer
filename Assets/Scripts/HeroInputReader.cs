using PixelCrew.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
            _inputActions.Hero.Interact.performed += OnInteract;
            _inputActions.Hero.Attack.performed += OnAttack;
            _inputActions.Hero.Throw.performed += OnThrow;
        }

        private void OnDestroy()
        {
            _inputActions.Hero.Movement.performed -= OnMovement;
            _inputActions.Hero.Movement.canceled -= OnMovement;
            _inputActions.Hero.SaySomething.performed -= OnSaySomething;
            _inputActions.Hero.Interact.performed -= OnInteract;
            _inputActions.Hero.Attack.performed -= OnAttack;
            _inputActions.Hero.Throw.performed -= OnThrow;
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
            if (context.performed)
            {
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (context.interaction is PressInteraction)
                {
                    _hero.Throw(true);
                }

                else if (context.interaction is HoldInteraction)
                {
                    _hero.ThrowBurst();
                }
            }
        }
    }
}

