using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneButtonsUpdater : MonoBehaviour
{
    [SerializeField] private Image _continueButtonCloseImage;
    [SerializeField] private Button _continueButton;

    private void Start()
    {
        if (SaveSystem.Instance.SaveData.IsHaveGame)
        {
            ActivateContinueButton();
        }
    }

    private void ActivateContinueButton()
    {
        _continueButtonCloseImage.enabled = false;
        _continueButton.interactable = true;
    }
}
