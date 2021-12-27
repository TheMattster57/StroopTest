using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StroopTestManager : MonoBehaviour
{
    // The colours you wish to use in the Stroop Test
    public Colour[] colours;

    // Audio
    public AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    // UI
    public Sprite correctAnswer;
    public Sprite wrongAnswer;
    public Image AnswerImage;
    public TextMeshProUGUI question;
    public TextMeshProUGUI[] answers;

    // This is the current Question's colour
    Colour currentColour;
    // This is the current Question's text
    Colour currentColourName;

    // These are temporay values to determine what answers are avaliable to choose
    List<Colour> currentAnswerColours = new List<Colour>();
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
        
    }

    public void NewQuestion()
    {
        if(colours.Length > 1)
        {
            // Choosing a random colour for the question
            currentColour = colours[Random.Range(0, colours.Length)];

            // Choosing and checking if a random name doesn't match the question colour
            Colour tempColour = colours[Random.Range(0, colours.Length)];
            while (CheckColour(currentColour, tempColour))
            {
                tempColour = colours[Random.Range(0, colours.Length)];
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
                for (int c = 0; c < colours.Length; c++)
                {
                    colours[c].alreadyAvaliable = false;
                }
            }

            int correctAnswerInt = 0;
            int correctTextAnswerInt = 0;
            // Setting the answers
            for (int i = 0; i < answers.Length; i++)
            {
                int newColourInt = Random.Range(0, colours.Length);
                while(colours[newColourInt].alreadyAvaliable)
                {
                    newColourInt = Random.Range(0, colours.Length);
                }
                currentAnswerColours.Add(colours[newColourInt]);
                colours[newColourInt].alreadyAvaliable = true;

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
                correctAnswerInt = Random.Range(0, currentAnswerColours.Count);
                if(textAnswerAvaliable)
                {
                    while (correctTextAnswerInt == correctAnswerInt)
                    {
                        correctAnswerInt = Random.Range(0, currentAnswerColours.Count);
                    }
                }
                currentAnswerColours[correctAnswerInt] = currentColour;
            }
            if (!textAnswerAvaliable)
            {
                correctTextAnswerInt = Random.Range(0, currentAnswerColours.Count);
                while(correctTextAnswerInt == correctAnswerInt)
                {
                    correctTextAnswerInt = Random.Range(0, currentAnswerColours.Count);
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

    public void CheckAnswer(int answerNumber)
    {
        if(CheckColour(currentAnswerColours[answerNumber], currentColour))
        {
            StartCoroutine(Answer(true));
            NewQuestion();
        }
        else
        {
            StartCoroutine(Answer(false));
        }
    }

    IEnumerator Answer(bool correct)
    {
        if(correct)
        {
            AnswerImage.gameObject.SetActive(true);
            AnswerImage.sprite = correctAnswer;
            audioSource.clip = correctClip;
            yield return new WaitForSeconds(0.5f);
            AnswerImage.gameObject.SetActive(false);
        }
        else
        {
            AnswerImage.gameObject.SetActive(true);
            AnswerImage.sprite = wrongAnswer;
            audioSource.clip = wrongClip;
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
