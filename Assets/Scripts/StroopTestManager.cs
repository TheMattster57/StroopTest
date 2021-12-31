using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StroopTestManager : MonoBehaviour
{
    // The colours you wish to use in the Stroop Test
    public Colour[] colours;

    // Current time in the run
    float currentTime;

    // Counter of how many questions completed & how many to finish
    public int requiredToFinish;
    int currentQuestionNumber;

    // Audio
    public AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    // UI
    public GameObject questionCanvas;
    public GameObject endGameCanvas;
    public Sprite correctAnswer;
    public Sprite wrongAnswer;
    public Image AnswerImage;
    public TextMeshProUGUI currentTimeUI;
    public TextMeshProUGUI endTimeUI;
    public TextMeshProUGUI question;
    public TextMeshProUGUI[] answers;

    // This is the current Question's colour
    Colour currentColour;
    // This is the current Question's text
    Colour currentColourName;

    // These are temporay values to determine what answers are avaliable to choose
    List<Colour> currentAnswerColours = new List<Colour>();
    List<Colour> avaliableColours = new List<Colour>();
    bool answerAvaliable = false;
    bool textAnswerAvaliable = false;

    // Start is called before the first frame update
    void Start()
    {
        NewQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentQuestionNumber < requiredToFinish)
        {
            currentTime += Time.deltaTime;
            currentTimeUI.text = Mathf.Round(currentTime).ToString();
        }
    }

    public void NewQuestion()
    {
        if(colours.Length > 1)
        {
            // Choosing a random colour for the question
            currentColour = colours[Random.Range(1, colours.Length) - 1];

            // Choosing and checking if a random name doesn't match the question colour
            Colour tempColour = colours[Random.Range(1, colours.Length) - 1];
            while (CheckColour(currentColour, tempColour))
            {
                tempColour = colours[Random.Range(1, colours.Length) - 1];
            }
            currentColourName = tempColour;

            // Setting the Question UI
            question.color = currentColour.color;
            question.text = currentColourName.colorName;

            // Resetting the answers
            if (currentAnswerColours.Count > 0)
            {
                currentAnswerColours.Clear();
                answerAvaliable = false;
                textAnswerAvaliable = false;
            }
            for (int c = 0; c < colours.Length; c++)
            {
                colours[c].alreadyUsed = false;
            }

            int correctAnswerInt = 0;
            int correctTextAnswerInt = 0;

            // Setting the answers
            for (int i = 0; i < answers.Length; i++)
            {
                avaliableColours.Clear();
                for (int c = 0; c < colours.Length; c++)
                {
                    if (!colours[c].alreadyUsed)
                        avaliableColours.Add(colours[c]);
                }
                if(avaliableColours.Count > 0)
                    currentAnswerColours.Add(avaliableColours[Random.Range(1, avaliableColours.Count) - 1]);
                for (int n = 0; n < colours.Length; n++)
                {
                    if (currentAnswerColours.Count > 0)
                    {
                        if (colours[n] == currentAnswerColours[currentAnswerColours.Count - 1])
                        {
                            colours[n].alreadyUsed = true;
                            break;
                        }
                    }
                }
                
                // Checking if the new colour matches either the visual colour or the text of the questions and remembering it's position in the array
                if (CheckColour(currentColour, currentAnswerColours[i]))
                {
                    answerAvaliable = true;
                    correctAnswerInt = i;
                }

                if (CheckColour(currentColourName, currentAnswerColours[i]))
                {
                    textAnswerAvaliable = true;
                    correctTextAnswerInt = i;
                }
            }

            // Check if the correct answer & text answer are avaliable options and if not then set them
            if (!answerAvaliable)
            {
                correctAnswerInt = Random.Range(1, currentAnswerColours.Count) - 1;
                if(textAnswerAvaliable)
                {
                    while (correctTextAnswerInt == correctAnswerInt)
                    {
                        correctAnswerInt = Random.Range(1, currentAnswerColours.Count) - 1;
                    }
                }
                currentAnswerColours[correctAnswerInt] = currentColour;
            }
            if (!textAnswerAvaliable)
            {
                correctTextAnswerInt = Random.Range(1, currentAnswerColours.Count) - 1;
                while(correctTextAnswerInt == correctAnswerInt)
                {
                    correctTextAnswerInt = Random.Range(1, currentAnswerColours.Count) - 1;
                }
                currentAnswerColours[correctTextAnswerInt] = currentColourName;
            }

            // Set Answer UIs
            for (int ui = 0; ui < answers.Length; ui++)
            {
                answers[ui].text = currentAnswerColours[ui].colorName;
            }
        }
    }

    // answerNumber is which UI it is refering to
    public void CheckAnswer(int answerNumber)
    {
        if(CheckColour(currentAnswerColours[answerNumber - 1], currentColour))
        {
            StartCoroutine(Answer(true));
            // check if this is the last question
            if(currentQuestionNumber < requiredToFinish)
                NewQuestion();
            else
            {
                ChangeCanvas(true);
            }
        }
        else
        {
            StartCoroutine(Answer(false));
        }
    }

    // change to the end game canvas or back to the strrop test to try again
    public void ChangeCanvas(bool endGame)
    {
        if(endGame)
        {
            questionCanvas.SetActive(false);
            endGameCanvas.SetActive(true);
            endTimeUI.text = Mathf.Round(currentTime).ToString();
        }
        else
        {
            questionCanvas.SetActive(true);
            endGameCanvas.SetActive(false);
            currentQuestionNumber = 0;
            currentTime = 0;
            NewQuestion();
        }
    }

    // Give a response to the player about whether they got the correct or wrong answer
    IEnumerator Answer(bool correct)
    {
        if(correct)
        {
            AnswerImage.gameObject.SetActive(true);
            AnswerImage.sprite = correctAnswer;
            audioSource.clip = correctClip;
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
            AnswerImage.gameObject.SetActive(false);
            currentQuestionNumber++;
        }
        else
        {
            AnswerImage.gameObject.SetActive(true);
            AnswerImage.sprite = wrongAnswer;
            audioSource.clip = wrongClip;
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
            AnswerImage.gameObject.SetActive(false);
        }    
        yield return null;
    }

    bool CheckColour(Colour colour1, Colour colour2)
    {
        if (colour1 == colour2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
