using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "quiz question", fileName = "new question")]


public class questionSO : ScriptableObject
{
    [TextArea(2, 3)]
    [SerializeField] string question = "enter new text";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnsIndex;


    public string GetQuestion()
    {
        return question;
    }

    public string getAnswer(int index)
    {
        return answers[index];
    }

    public int getCorrectAnsIndex()
    {
        return correctAnsIndex;
    }




}
