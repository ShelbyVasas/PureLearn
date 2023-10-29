using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public TMP_InputField inputField;
    public List<GameObject> additionalAnimalPrefabs = new List<GameObject>();
    public TextMeshProUGUI levelUpText;
    public MissMarksManager missMarksManager;  // Reference to MissMarksManager
    public GameObject gameOverPanel;
    private ScoreManager scoreManager;
    private float levelUpDisplayTime = 2.0f;

    private void Start()
    {
        StartCoroutine(SpawnAnimalsRandomly());
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        CheckForNewAnimals();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnimalName(inputField.text);
            inputField.text = "";
            SetFocusToInputField();
        }
    }

    private void SetFocusToInputField()
    {
        inputField.ActivateInputField();
        inputField.Select();
    }

    IEnumerator SpawnAnimalsRandomly()
    {
        while (true)
        {
            SpawnRandomAnimal();
            
            // float randomDelay = Random.Range(5.0f); // You can adjust these values for desired spawn delay range
            yield return new WaitForSeconds(3.0f);
        }
    }
    
    public void SpawnRandomAnimal()
    {
        int randomIndex = Random.Range(0, animalPrefabs.Length);
        float randomXPosition = Random.Range(-5, 5);
        GameObject animal = Instantiate(animalPrefabs[randomIndex], new Vector2(randomXPosition, 8), Quaternion.identity); 
        animal.tag = "animal";
        animal.name = animalPrefabs[randomIndex].name;
    }

    void CheckAnimalName(string inputName)
    {
        GameObject closestAnimal = null; // To store the closest animal to the bottom
        float lowestY = float.MaxValue; // To keep track of the lowest Y value (closest to the bottom)

        foreach (var animal in GameObject.FindGameObjectsWithTag("animal"))
        {
            AnimalDrop animalDrop = animal.GetComponent<AnimalDrop>();
            if (animalDrop.CheckName(inputName, false)) // Check if the name matches without destroying the animal
            {
                if (animal.transform.position.y < lowestY) // Check if this animal is closer to the bottom
                {
                    closestAnimal = animal;
                    lowestY = animal.transform.position.y;
                }
            }
        }

        // Destroy the closest matching animal if any
        if (closestAnimal != null)
        {
            closestAnimal.GetComponent<AnimalDrop>().DestroyAnimal();
            ResetMissedAnimals();
        }
        else if (lowestY == 8) 
        {
            
        }
        else
        {
            AnimalMissed(); // If no animal was destroyed, it was a miss
        }
    }

    void CheckForNewAnimals()
    {
        if (additionalAnimalPrefabs.Count <= 0) return; // If there are no more animals to add, just return

        // Here we're checking the score of the last animal in the current animalPrefabs array
        string lastAnimalInList = animalPrefabs[animalPrefabs.Length - 1].GetComponent<AnimalDrop>().animalName;

        if (scoreManager.GetIndividualScore(lastAnimalInList) >= 50) // If the last animal in the current list has a score of 50 or more
        {
            foreach (var animal in animalPrefabs)
            {
                animal.GetComponent<AnimalDrop>().fallSpeed += 0.25f;
            }

            List<GameObject>tempList = new List<GameObject>(animalPrefabs);
            tempList.Add(additionalAnimalPrefabs[0]); // Add the first animal from the additional list
            animalPrefabs = tempList.ToArray(); // Convert back to array for the main animal list
            additionalAnimalPrefabs.RemoveAt(0);  // Remove it from the additional list
            ShowLevelUpText();
        }
    }

    void ShowLevelUpText()
    {
        levelUpText.gameObject.SetActive(true);
        Invoke("HideLevelUpText", levelUpDisplayTime);
    }

    void HideLevelUpText()
    {
        levelUpText.gameObject.SetActive(false);
    }

    public void AnimalMissed()
    {
        if (missMarksManager.RegisterMiss())
        {
            EndGame();
        }
    }

    void ResetMissedAnimals()
    {
        missMarksManager.ResetMisses();
    }

    void EndGame()
    {
        // gameOverPanel.SetActive(true);
        foreach (var animal in animalPrefabs)
        {
            animal.GetComponent<AnimalDrop>().fallSpeed = 1f;
        }
        StartCoroutine(EndGameAfterDelay());
    }

    IEnumerator EndGameAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        #endif
    }
}
