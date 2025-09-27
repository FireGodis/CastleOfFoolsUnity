using UnityEngine;
using System.Collections;

public class MoverDelorean : MonoBehaviour
{
    // tempo da animação (quanto maior, mais lento)
    public float duration = 2f;

    RectTransform rt;
    Coroutine moveRoutine;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    // chama de outro script para ir suavemente até X = 0
    public void MoverParaZero()
    {
        Vector2 destino = new Vector2(-105f, rt.anchoredPosition.y);
        MoverPara(destino);
    }

    // chama de outro script para ir para qualquer posição
    public void MoverPara(Vector2 destino)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoverSuavemente(destino));
    }

    IEnumerator MoverSuavemente(Vector2 destino)
    {
        Vector2 inicio = rt.anchoredPosition;
        float tempo = 0f;

        while (tempo < duration)
        {
            tempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tempo / duration); // ease in/out
            rt.anchoredPosition = Vector2.Lerp(inicio, destino, t);
            yield return null;
        }

        rt.anchoredPosition = destino; // garante posição final exata
        moveRoutine = null;
    }
}
