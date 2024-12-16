using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Manager : Singleton<Game_Manager>
{
    [SerializeField] private Image loadingScreen;
    [Space]
    [SerializeField] private int buttonsOnLevel = 3;
    [Space]
    [SerializeField] private AnswerButton buttonPrefab;

    private int currentRightAnswerID = 0;
    private int levelNum = 1;
    private List<AnswerButton> spawnedButtons;


    public Action<bool> FinishedLastLevel;
    public Action ChangeTask;

    public string Task => GetCurrentAnswer().blockSymbol;

    private void Start()
    {
        PrepareTheGame();
    }


    private void SetLevel()
    {
        currentRightAnswerID = UnityEngine.Random.Range(0, TaskManager.Instance.PossibleAnswers.Count);
        ResetLevel();
        ChangeTask?.Invoke();
        int amountToSpawn = (buttonsOnLevel * levelNum) - spawnedButtons.Count;

        List<AnswerBlock> temp = new List<AnswerBlock>();
        temp.AddRange(TaskManager.Instance.PossibleAnswers);

        for (int i = 0; i < amountToSpawn; i++)
        {
            spawnedButtons.Add(CreateButton(temp));
        }
        
        AddRightAnswer();

        BounceButtons();
    }

    private AnswerButton CreateButton(List<AnswerBlock> temp)
    {
        AnswerButton button = Instantiate(buttonPrefab, AnswersLayout.Instance.transform);
        int randomAnswerID = UnityEngine.Random.Range(0, temp.Count);

        if (randomAnswerID >= temp.Count)
        {
            StopTheGame();
            return null;
        }

        button.SetAnswer(temp[randomAnswerID], 
            TaskManager.Instance.PossibleColors[UnityEngine.Random.Range(0, TaskManager.Instance.PossibleColors.Length)]);

        temp.RemoveAt(randomAnswerID);
        return button;
    }
    private void AddRightAnswer()
    {
        bool havingRightAnswer = false;
        foreach (var b in spawnedButtons)
            if (b.AnswerSymbol == GetCurrentAnswer().blockSymbol)
                havingRightAnswer = true;

        if (!havingRightAnswer)
            spawnedButtons[UnityEngine.Random.Range(0, spawnedButtons.Count)].SetAnswer(GetCurrentAnswer()
                          , TaskManager.Instance.PossibleColors[UnityEngine.Random.Range(0, TaskManager.Instance.PossibleColors.Length)]);
    }

    private AnswerBlock GetCurrentAnswer()
    {
        return TaskManager.Instance.PossibleAnswers[currentRightAnswerID];
    }

    private void ResetLevel()
    {
        foreach (var b in spawnedButtons)
            Destroy(b.gameObject);

        spawnedButtons.Clear();
    }

    private void BounceButtons()
    {
        foreach (var b in spawnedButtons)
            b.transform.DOScale(1f, 0.5f);
    }

    public bool CheckAnswer(AnswerBlock answer)
    {
        if (answer == GetCurrentAnswer())
            return true;
        else
            return false;
    }

    public void PrepareTheGame()
    {
        StartCoroutine(FadeOutLoadingScreen());
        spawnedButtons = new List<AnswerButton>();
        SetLevel();
    }

    public void RestartTheGame()
    {
        StartCoroutine(ReloadScene());
    }

    public void NextLevel()
    {
        TaskManager.Instance.PossibleAnswers.Remove(GetCurrentAnswer());

        if (levelNum >= 3)
        {
            FinishedLastLevel?.Invoke(true);
            return;
        }
        else
            levelNum++;

        SetLevel();
    }

    public void StopTheGame()
    {
        Debug.Log("You won!");
    }

    private System.Collections.IEnumerator ReloadScene()
    {
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private System.Collections.IEnumerator FadeOutLoadingScreen()
    {
        loadingScreen.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        loadingScreen.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
