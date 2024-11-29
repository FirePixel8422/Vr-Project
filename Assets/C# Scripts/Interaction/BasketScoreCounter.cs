using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScoreCounter : MonoBehaviour
{
    public int score;
    
    public int number1;
    public int number2;

    public GameObject[] scoreNumber1Parts;
    public GameObject[] scoreNumber2Parts;

    public BasketScoreFormation[] numberFormations;


    public float updateDelay;
    public float partChangeInterval;
    public float endDelay;


    public void UpdateScore()
    {
        score += 1;

        if (score == 100)
        {
            score = 0;
        }

        //if number has changed
        bool number1Changed = number1 != score / 10;

        number1 = score / 10;
        number2 = score % 10;

        StartCoroutine(UpdateScoreBoard(number1Changed));
    }

    private IEnumerator UpdateScoreBoard(bool number1Changed)
    {
        yield return new WaitForSeconds(updateDelay);


        WaitForSeconds waitPartChangeInterval = new WaitForSeconds(partChangeInterval);


        for (int i = 0; i < scoreNumber2Parts.Length; i++)
        {
            if (scoreNumber2Parts[i].activeInHierarchy)
            {
                scoreNumber2Parts[i].SetActive(false);

                yield return waitPartChangeInterval;
            }
        }

        if (number1Changed)
        {
            for (int i = 0; i < scoreNumber1Parts.Length; i++)
            {
                if (scoreNumber1Parts[i].activeInHierarchy)
                {
                    scoreNumber1Parts[i].SetActive(false);

                    yield return waitPartChangeInterval;
                }
            }
        }


        yield return new WaitForSeconds(endDelay);


        if (number1Changed)
        {
            for (int i = 0; i < scoreNumber1Parts.Length; i++)
            {
                if (numberFormations[number1].states[i])
                {
                    scoreNumber1Parts[i].SetActive(true);

                    yield return waitPartChangeInterval;
                }
            }
        }


        for (int i = 0; i < scoreNumber2Parts.Length; i++)
        {
            if (numberFormations[number2].states[i])
            {
                scoreNumber2Parts[i].SetActive(true);

                yield return waitPartChangeInterval;
            }
        }
    }



    [System.Serializable]
    public struct BasketScoreFormation
    {
        public bool[] states;
    }
}
