using UnityEngine;

public class SecretWallBehaviour : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private float alphaVal;
    [SerializeField] private float fadeOutSpeed;
    private Material _secretTileMaterial;
    private bool showSecretWall = false;

    private void Awake()
    {
        _secretTileMaterial = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_tag))
            showSecretWall = true;
    }

    private void changeColor()
    {
        Color oldColor = _secretTileMaterial.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        Color lerpedColor = Color.Lerp(oldColor, newColor, fadeOutSpeed * Time.deltaTime);
        _secretTileMaterial.SetColor("_Color", lerpedColor);
    }

    private void Update()
    {
        if (showSecretWall)
            changeColor();
    }
}
