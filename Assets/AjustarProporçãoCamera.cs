using UnityEngine;

public class AjustarCameraProporcao : MonoBehaviour
{
    public float pixelsPorUnidade = 100; // Ajuste este valor com base na escala dos seus assets

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float proporcaoAlvo = 9f / 16f;
        float proporcaoTela = (float)Screen.width / Screen.height;
        float escalaAltura = proporcaoTela / proporcaoAlvo;

        if (escalaAltura < 1)
        {
            camera.orthographicSize = camera.orthographicSize / escalaAltura;
        }
    }
}