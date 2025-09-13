using UnityEngine;
using UnityEngine.UI;

public class logoNeon : MonoBehaviour
{
    public float rotationAmplitude = 10f; // ângulo máximo (10 significa -10 até +10)
    public float rotationSpeed = 1f;      // velocidade da oscilação
    public float colorSpeed = 4f;         // velocidade da troca de cor

    private Image image;
    private Color color1 = Color.gray;
    private Color color2 = Color.white;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        // Rotação oscilando entre -amplitude e +amplitude
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationAmplitude;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);

        // Troca suave de cor (cinza ↔ branco)
        float t = (Mathf.Sin(Time.time * colorSpeed) + 1f) / 2f;
        image.color = Color.Lerp(color1, color2, t);
    }
}
