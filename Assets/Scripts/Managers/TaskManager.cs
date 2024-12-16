using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [SerializeField] private List<AnswerBlock> possibleAnswers;
    [SerializeField] private Color[] possibleColorsForBlocks;

    public List<AnswerBlock> PossibleAnswers => possibleAnswers;
    public Color[] PossibleColors => possibleColorsForBlocks;
}
