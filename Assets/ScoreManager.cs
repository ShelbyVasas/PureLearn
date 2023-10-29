using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // Assign the score display field
    private int score = 0;
    public Dictionary<string, int> individualAnimalScores = new Dictionary<string, int>();


    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    public void SubtractScore(int value)
    {
        score -= value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    public void AddIndividualScore(string animalName, int amount)
    {
        if (!individualAnimalScores.ContainsKey(animalName))
        {
            individualAnimalScores[animalName] = 0;
        }
        
        individualAnimalScores[animalName] += amount;
    }

    public void SubtractIndividualScore(string animalName, int amount)
    {
        if (!individualAnimalScores.ContainsKey(animalName))
        {
            individualAnimalScores[animalName] = 0;
        }
        
        individualAnimalScores[animalName] -= amount;
        individualAnimalScores[animalName] = Mathf.Clamp(individualAnimalScores[animalName], 0, individualAnimalScores[animalName]);
        // Optionally, you can clamp scores to not go below 0 or above a certain threshold
        // individualAnimalScores[animalName] = Mathf.Clamp(individualAnimalScores[animalName], 0, maxScoreForAnimal);
    }


    public int GetIndividualScore(string animalName)
    {
        if (individualAnimalScores.ContainsKey(animalName))
        {
            return individualAnimalScores[animalName];
        }
        return 0;
    }
}
