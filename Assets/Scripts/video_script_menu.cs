using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

public class video_script_menu : MonoBehaviour
{
    public VideoPlayer video_menu;
    
    public AudioSource audio_menu;
    public Image fundo1;
    public Image fundo2;
    public RawImage tela;
    public Button botao_jogar;
    public Button botao_op;
    public Button botao_sair;
    public float tempo_video = 1f;

    public float fadeDuration = 2f; // tempo do fade em segundos
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        video_menu.source = VideoSource.Url;
        video_menu.url = Application.streamingAssetsPath + "/introcastleoffoolscoridiga.mp4";
        StartCoroutine(Tempo_de_espera());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Tempo_de_espera()
    {

        yield return new WaitForSeconds(1 * tempo_video);  // espera 1 segundos
        video_menu.Play();
        audio_menu.Play();
        yield return new WaitForSeconds(5 * tempo_video);
        fundo1.gameObject.SetActive(false);
        yield return new WaitForSeconds(14 * tempo_video);
        video_menu.Pause();
        tela.gameObject.SetActive(false);

        yield return new WaitForSeconds(1 * tempo_video);
        yield return StartCoroutine(FadeOut(fundo2)); // aqui chamamos o fade
        fundo2.gameObject.SetActive(false); // desativa no final
        botao_jogar.interactable = true;
        botao_op.interactable = true;
        botao_sair.interactable = true;



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
}
