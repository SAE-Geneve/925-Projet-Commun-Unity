using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

using UnityEngine.UI;

public class UIScreenEffects : MonoBehaviour
{
    [Tooltip("Curve of the height movement speed")] [SerializeField]
    private AnimationCurve heightCurve;

    [Tooltip("Curve of the color graduation")] [SerializeField]
    private AnimationCurve colorCurve;

    [Header("Effects")] [SerializeField] private float textEffectDuration = 2.5f;

    [SerializeField] private float imageEffectDuration = 2f;

    private IObjectPool<Image> _imagePool;
    private Image _referenceImage;

    public void ImagePoolCreation(Image argImage)
    {
        _referenceImage = argImage;
        // Create a pool with the four core callbacks.
        _imagePool = new ObjectPool<Image>(
            createFunc: CreateItem,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: true, // helps catch double-release mistakes
            defaultCapacity: 10,
            maxSize: 50
        );
        Debug.Log("Image pool has been created");
    }

    private Image CreateItem()
    {
        Image image = Instantiate(_referenceImage, _referenceImage.transform.parent);
        image.name = "PooledImage";
        image.gameObject.SetActive(false);
        return image;
    }

    // Called when an item is taken from the pool.
    private void OnGet(Image image)
    {
        image.gameObject.SetActive(true);
    }

    // Called when an item is returned to the pool.
    private void OnRelease(Image image)
    {
        image.gameObject.SetActive(false);
        image.color = new Color(_referenceImage.color.r, _referenceImage.color.g, _referenceImage.color.b, 1f);
    }

    // Called when the pool decides to destroy an item (e.g., above max size).
    private void OnDestroyItem(Image image)
    {
        Destroy(image);
    }

    private TextMeshProUGUI TemporaryVariableMaker(TextMeshProUGUI textSample)
    {
        TextMeshProUGUI tempText = Instantiate(textSample, textSample.transform.parent);
        tempText.gameObject.SetActive(true);
        return tempText;
    }

    private Image TemporaryVariableMaker(Image imageSample)
    {
        Image tempImage = Instantiate(imageSample, imageSample.transform.parent);
        tempImage.gameObject.SetActive(true);
        return tempImage;
    }

    public IEnumerator DoImagePoolFade()
    {
        var tempImage = _imagePool.Get();

        Color startcolor = tempImage.color;
        Color endcolor = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
        float t = 0.0f;
        while (tempImage.color.a > 0)
        {
            tempImage.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / imageEffectDuration;
            }

            yield return null;
        }

        _imagePool.Release(tempImage);
        yield return null;
    }

    public IEnumerator DoImageFade(Image fadeImage)
    {
        var tempImage = TemporaryVariableMaker(fadeImage);

        Color startcolor = tempImage.color;
        Color endcolor = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
        float t = 0.0f;
        while (tempImage.color.a > 0)
        {
            tempImage.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / imageEffectDuration;
            }

            yield return null;
        }

        Destroy(tempImage);
        yield return null;
    }

    public IEnumerator DoTextFade(TextMeshProUGUI fadeText)
    {
        var tempText = TemporaryVariableMaker(fadeText);

        Color startcolor = tempText.color;
        Color endcolor = new Color(tempText.color.r, tempText.color.g, tempText.color.b, 0);
        float t = 0.0f;

        while (tempText.color.a > 0)
        {
            tempText.color = Color.Lerp(startcolor, endcolor, colorCurve.Evaluate(t));
            if (t < 1)
            {
                t += Time.deltaTime / textEffectDuration;
            }

            yield return null;
        }

        Destroy(tempText.gameObject);
        yield return null;
    }

    public IEnumerator DoTextMoveDown(TextMeshProUGUI fadeText)
    {
        var tempText = TemporaryVariableMaker(fadeText);

        float t = 0.0f;

        while (t < textEffectDuration)
        {
            tempText.rectTransform.anchoredPosition += Vector2.down * heightCurve.Evaluate(t);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(tempText.gameObject);
        yield return null;
    }

    public IEnumerator DoTextFadeMoveDown(TextMeshProUGUI fadeText)
    {
        var tempText = TemporaryVariableMaker(fadeText);

        float t = 0.0f;
        Color startcolor = tempText.color;
        Color endcolor = new Color(tempText.color.r, tempText.color.g, tempText.color.b, 0);

        while (tempText.color.a > 0)
        {
            tempText.rectTransform.anchoredPosition += new Vector2(0f, -0.5f) * heightCurve.Evaluate(t);
            tempText.color = Color.Lerp(startcolor, endcolor, colorCurve.Evaluate(t));

            if (t < 1)
            {
                t += Time.deltaTime / textEffectDuration;
            }

            yield return null;
        }

        Destroy(tempText.gameObject);
        yield return null;
    }
}