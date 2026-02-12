using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatientAnimation : MonoBehaviour
{
    [SerializeField] Sprite patientSprite; // 可选的Sprite资源，如需更改Sprite可在此赋值
    private SpriteRenderer spriteRenderer;
    public float fadeDuration = 1.0f; // 淡入淡出持续时间，可在Inspector中调整

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        if (patientSprite != null)
        {
            spriteRenderer.sprite = patientSprite;
        }
    }

    void OnEnable()
    {
        EventManager.CureSuccessfullyEvent += ChangeSprite;
    }

    void OnDisable()
    {
        EventManager.CureSuccessfullyEvent -= ChangeSprite;
    }

    void ChangeSprite()
    {
        StartCoroutine(ChangeColorAndFade());
    }

    IEnumerator ChangeColorAndFade()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Color randomColor = GenerateRandomColor();
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            spriteRenderer.color = new Color(randomColor.r, randomColor.g, randomColor.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(randomColor.r, randomColor.g, randomColor.b, 1f);
    }

    Color GenerateRandomColor()
    {
        float hue = Random.Range(0f, 1f);
        float saturation = Random.Range(0.3f, 0.7f);
        float value = Random.Range(0.3f, 0.8f);
        return Color.HSVToRGB(hue, saturation, value);
    }
}