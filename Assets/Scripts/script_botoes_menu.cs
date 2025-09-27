using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class script_botoes_menu : MonoBehaviour
{
    public GameObject CanvasPlayer;
    public Canvas Tela_menu;
    public VideoPlayer video_loading;
    public GameObject loadingImage;
    public GameObject Player;
    public GameObject teleport1;
    public AudioSource audio_menu;
    public AudioSource audio_sonho;
    public AudioSource audio_sonho_intenso;
    public float fadeDuration = 1f;
    public Image preto;
    public GameObject colisor_dialogo;
    [Header("Cursor personalizado")]
    public Texture2D cursorTexture;   // arraste sua imagem aqui no Inspector
    public Vector2 hotspot = Vector2.zero; // ponto de clique do cursor (0,0 é o canto superior esquerdo)


    public GameObject Inimigo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        video_loading.source = VideoSource.Url;
        video_loading.url = Application.streamingAssetsPath + "/loadingBloodNeon.mp4";
        loadingImage.gameObject.SetActive(false);
        colisor_dialogo.SetActive(false);
        // Troca o cursor no início do jogo
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
        audio_sonho.Play();
        audio_sonho_intenso.Play();
        audio_sonho.volume = 0;
        audio_sonho_intenso.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Tempo_de_espera()
    {
        yield return new WaitForSeconds(10);
    }

    public void fadein_audio(AudioSource audio)
    {
        StartCoroutine(FadeInAudio(audio));
    }

    public void fadeout_audio(AudioSource audio)
    {
        StartCoroutine(FadeOutAudio(audio));
    }

    private IEnumerator FadeInAudio(AudioSource audio)
    {
        audio.volume = 0f;
        
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audio.volume = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }

        audio.volume = 1f; // garante volume máximo
    }

    private IEnumerator FadeOutAudio(AudioSource audio)
    {
        float startVolume = audio.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        audio.volume = 0f; // garante volume zero
        
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
        fadeout_audio(audio_menu);
        yield return StartCoroutine(FadeIn(preto));

        // Ativa player e teleporta
        
        Player.transform.position = teleport1.transform.position;

        // Espera 1 segundo (pode trocar por 10 se quiser)
        yield return new WaitForSeconds(2f);
        loadingImage.gameObject.SetActive(true);
        video_loading.Play();
        yield return new WaitForSeconds(7f);
        Player.SetActive(true);
        loadingImage.gameObject.SetActive(false);
        Tela_menu.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        audio_sonho.Stop();
        audio_sonho_intenso.Stop();
        audio_sonho.Play();
        audio_sonho_intenso.Play();
        fadein_audio(audio_sonho);
        
        



        // Fade out
        yield return StartCoroutine(FadeOut(preto));
        
        CanvasPlayer.gameObject.SetActive(true);
        colisor_dialogo.SetActive(true);


    }

    public void Jogar()
    {
        StartCoroutine(JogarRoutine());
    }
}
