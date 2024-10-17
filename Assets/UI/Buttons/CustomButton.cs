using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : MonoBehaviour
{
    private Button _button;

    protected void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }
}
