using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_TaskText : MonoBehaviour
{
    [SerializeField] private Text taskTitle;
    [SerializeField] private Text taskText;
    private Color textColor = new Color(0, 0, 0, 0);

    private void Start()
    {
        Game_Manager.Instance.ChangeTask += ChangeTask;
    }

    public void ChangeTask()
    {
        taskText.text = Game_Manager.Instance.Task;
        taskTitle.color = textColor;
        taskText.color = textColor;

        taskTitle.DOFade(1, 1f);
        taskText.DOFade(1, 1f);
    }

    private void OnDestroy()
    {
        Game_Manager.Instance.ChangeTask -= ChangeTask;
    }
}
