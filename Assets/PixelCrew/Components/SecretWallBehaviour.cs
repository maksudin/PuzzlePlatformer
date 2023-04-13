using UnityEngine;

namespace Assets.PixelCrew.Components
{
    public class SecretWallBehaviour : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private float _alphaVal, _fadeOutSpeed;
        private Material _secretTileMaterial;
        private bool _showSecretWall = false;

        private void Awake() =>
            _secretTileMaterial = GetComponent<Renderer>().material;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
                _showSecretWall = true;
        }

        private void СhangeColor()
        {
            var oldColor = _secretTileMaterial.color;
            var newColor = new Color(oldColor.r, oldColor.g, oldColor.b, _alphaVal);
            var lerpedColor = Color.Lerp(oldColor, newColor, _fadeOutSpeed * Time.deltaTime);
            _secretTileMaterial.SetColor("_Color", lerpedColor);
        }

        private void Update()
        {
            if (_showSecretWall)
                СhangeColor();
        }
    }
}