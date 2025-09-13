using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class script_botoes_menu : MonoBehaviour
{
    public Canvas CanvasPlayer;
    public Canvas Tela_menu;
    public GameObject Player;
    public GameObject teleport1;
    public float fadeDuration = 1f;
    public Image preto;
    

    public GameObject Inimigo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Tempo_de_espera()
    {
        yield return new WaitForSeconds(10);
    }

    private IEnumerator FadeIn(Image img)
    {
        Color cor = img.color;
        float startAlpha = cor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsed / fadeDuration);
            img.color = new Color(cor.r, cor.g, cor.b, newAlpha);
            yield return null;
        }

        img.color = new Color(cor.r, cor.g, cor.b, 1f); // garante alpha 1
    }
    private IEnumerator FadeOut(Image img)
    {
        Color cor = img.color;
        float startAlpha = cor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            img.color = new Color(cor.r, cor.g, cor.b, newAlpha);
            yield return null;
        }

        img.color = new Color(cor.r, cor.g, cor.b, 0f); // garante alpha 0
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut(preto));
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn(preto));
    }

    private IEnumerator JogarRoutine()
    {
        // Fade in
        yield return StartCoroutine(FadeIn(preto));

        // Ativa player e teleporta
        Player.SetActive(true);
        Player.transform.position = teleport1.transform.position;

        // Espera 1 segundo (pode trocar por 10 se quiser)
        yield return new WaitForSeconds(2f);
        Tela_menu.gameObject.SetActive(false);

        // Fade out
        yield return StartCoroutine(FadeOut(preto));

        // Mais uma espera se precisar
        yield return new WaitForSeconds(1f);

        // Desativa menu
        Tela_menu.gameObject.SetActive(false);
    }

    public void Jogar()
    {
        StartCoroutine(JogarRoutine());
    }
}
