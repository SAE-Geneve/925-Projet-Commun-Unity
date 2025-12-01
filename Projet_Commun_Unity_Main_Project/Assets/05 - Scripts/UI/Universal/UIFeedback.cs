using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIFeedback : MonoBehaviour
{
    [Tooltip("Curve of the height movement speed")]
    [SerializeField] private AnimationCurve heightCurve;
    
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
        image.gameObject.transform.position = _referenceImage.transform.position;
    }

    // Called when the pool decides to destroy an item (e.g., above max size).
    private void OnDestroyItem(Image image)
    {
        Destroy(image);
    }
    
    public IEnumerator ImageFade(Image imageArg, float effectDuration)
    {
        var tempImage=_imagePool.Get();
        tempImage.sprite = imageArg.sprite;
        tempImage.color = imageArg.color;
        
        Color startcolor = tempImage.color;
        Color endcolor = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
        float t = 0.0f;
        while (tempImage.color.a > 0)
        {
            tempImage.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        _imagePool.Release(tempImage);
        yield return null;
    }
    
    public IEnumerator ImageUpFade(Image imageArg, float effectDuration)
    {
        var tempImage=_imagePool.Get();
        tempImage.sprite = imageArg.sprite;
        tempImage.color = imageArg.color;
        
        Color startcolor = tempImage.color;
        Color endcolor = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
        float t = 0.0f;
        while (tempImage.color.a > 0)
        {
            tempImage.rectTransform.anchoredPosition += new Vector2(0f, 0.1f) * heightCurve.Evaluate(t);
            tempImage.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        
        _imagePool.Release(tempImage);
        yield return null;
    }
}
