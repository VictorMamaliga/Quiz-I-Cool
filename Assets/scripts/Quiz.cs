using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{

    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<questionSO> questions = new List<questionSO>();
    questionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true ;
    
    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswSpirte;
    [SerializeField] Sprite correctAnswSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;


    public bool isComplete;


    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
       
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {

            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            getNextQuestion();
            timer.loadNextQuestion = false;

        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion )
        {
            displayAnswer(-1);
            setButtonState(false);
        }

    }

    public void OnAnswSelected(int index)
    {
        hasAnsweredEarly = true;
        displayAnswer(index);
        setButtonState(false);
        timer.cancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";


    }

    void displayAnswer(int index)
    {
        Image buttonImage;

        if (index == currentQuestion.getCorrectAnsIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswSprite;
            scoreKeeper.IncrementCorrectAnswer();
        }
        else
        {
            correctAnswerIndex = currentQuestion.getCorrectAnsIndex();
            string correctAnswer = currentQuestion.getAnswer(correctAnswerIndex);
            questionText.text = "Sorry , the correct answer was :\n" + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswSprite;

        }
    }

    void getNextQuestion()
    {

        if (questions.Count > 0)
        {
            setButtonState(true);
            setDefaultButtonSprite();
            GetRandomQuestion();
            displayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }


   


    void displayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();


        for (int i = 0; i < answerButtons.Length; i++)
        {

            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.getAnswer(i);
        }
    }
    void setButtonState (bool state)
    {
        for ( int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void setDefaultButtonSprite()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswSpirte;
        }
    }



}
