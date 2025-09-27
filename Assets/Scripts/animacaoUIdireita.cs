using UnityEngine;
using System.Collections;

public class animacaoUIdireita : MonoBehaviour
{
    // tempo da animação (quanto maior, mais lento)
    public float duration = 1f;

    RectTransform rt;
    Coroutine moveRoutine;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void MoverIdaVolta(float tempoEspera = 4f)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        // posição inicial e destino
        Vector2 destino = new Vector2(0f, rt.anchoredPosition.y);
        Vector2 origem = new Vector2(180f, rt.anchoredPosition.y);

        // starta a corrotina
        moveRoutine = StartCoroutine(MoverIdaVoltaCoroutine(origem, destino, tempoEspera));
    }

    private IEnumerator MoverIdaVoltaCoroutine(Vector2 origem, Vector2 destino, float tempoEspera)
    {
        // vai do origem → destino
        float tempo = 0f;
        while (tempo < duration)
        {
            tempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tempo / duration);
            rt.anchoredPosition = Vector2.Lerp(origem, destino, t);
            yield return null;
        }

        rt.anchoredPosition = destino;

        // espera o tempo de pausa
        yield return new WaitForSeconds(tempoEspera);

        // volta do destino → origem
        tempo = 0f;
        while (tempo < duration)
        {
            tempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tempo / duration);
            rt.anchoredPosition = Vector2.Lerp(destino, origem, t);
            yield return null;
        }

        rt.anchoredPosition = origem;
        moveRoutine = null;
    }
}
