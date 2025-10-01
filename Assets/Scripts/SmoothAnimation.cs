using UnityEngine;

public class SmoothAnimator : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color startColor;
    private Color targetColor;
    private Vector3 startScale;
    private Vector3 targetScale;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float duration;
    private float elapsed;
    private bool animating = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void AnimateTo(Color color, Vector3 scale, Vector3 position, float time)
    {
        startColor = sr.color;
        targetColor = color;
        startScale = transform.localScale;
        targetScale = scale;
        startPos = transform.position;
        targetPos = position;
        duration = time;
        elapsed = 0f;
        animating = true;
    }

    void Update()
    {
        if (!animating) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        sr.color = Color.Lerp(startColor, targetColor, t);
        transform.localScale = Vector3.Lerp(startScale, targetScale, t);
        transform.position = Vector3.Lerp(startPos, targetPos, t);

        if (t >= 1f) animating = false;
    }
}
