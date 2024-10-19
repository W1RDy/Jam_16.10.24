using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SceneGoods : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ActivateGoods()
    {
        _image.enabled = true;
    }
}
