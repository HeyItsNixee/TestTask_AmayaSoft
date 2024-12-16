using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScreen : MonoBehaviour
{
    [SerializeField] private Image restartScreen;
    [SerializeField] private Image restartButton;

    private Color disabledScreenColor = new Color(0, 0, 0, 0);
    private Color disabledButtonColor = new Color(255, 255, 255, 0);

    void Start()
    {
        ShowTheScreen(false);
        Game_Manager.Instance.FinishedLastLevel += ShowTheScreen;
    }

    public void ShowTheScreen(bool value)
    {
        restartScreen.enabled = value;
        restartButton.enabled = value;

        if (value)
        {
            restartScreen.DOFade(0.85f, 1f);
            restartButton.DOFade(1f, 1f);
        }
        else
        {
            restartButton.color = disabledButtonColor;
            restartScreen.color = disabledScreenColor;
        }
    }

    private void OnDestroy()
    {
        Game_Manager.Instance.FinishedLastLevel -= ShowTheScreen;
    }
}
