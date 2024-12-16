using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image symbolSprite;
    private AnswerBlock answer;
    private Quaternion originalRotation;

    public string AnswerSymbol => answer.blockSymbol;

    public void SetAnswer(AnswerBlock new_answer, Color bg_color)
    {
        answer = new_answer;
        background.color = bg_color;
        if (answer.rotationZAmount != 0)
        {
            originalRotation = symbolSprite.transform.rotation;
            symbolSprite.transform.Rotate(0f, 0f, answer.rotationZAmount, Space.Self);
        }
        else
            symbolSprite.transform.rotation = originalRotation;


        symbolSprite.sprite = answer.letterSprite;
    }

    public void OnClick()
    {
        bool result = Game_Manager.Instance.CheckAnswer(answer);
        if (result)
        {
            StartCoroutine(CorrectAnswer());
        }
        else
        {
            symbolSprite.transform.DOShakePosition(1f , new Vector3(10f, 0f, 0f), 5, 0, false, false, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InBounce);
        }
    }

    private IEnumerator CorrectAnswer()
    {
        //Particles;

        symbolSprite.transform.DOScale(0.5f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        symbolSprite.transform.DOScale(1f, 0.25f);
        yield return new WaitForSeconds(0.45f);
        Game_Manager.Instance.NextLevel();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
