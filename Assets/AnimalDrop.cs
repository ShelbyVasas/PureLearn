using UnityEngine;
using TMPro;

public class AnimalDrop : MonoBehaviour
{
    public string animalName; // Name of the animal, should match sprite
    public float fallSpeed = 1.0f; // Adjust as necessary
    public TMP_InputField inputField; // Assign the user input field
    private ScoreManager scoreManager;
    private MissMarksManager missMarksManager;


    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
        if (transform.position.y <= -5) // Adjust this value based on your screen size
        {
            scoreManager.SubtractScore(10);
            scoreManager.SubtractIndividualScore(animalName, 10);
            Destroy(gameObject);  
        }
    }

    public bool CheckName(string inputName, bool destroyOnMatch = true)
    {
        if (inputName == animalName)
        {
            if (destroyOnMatch)
            {
                DestroyAnimal();
            }
            return true;
        }
        return false;
    }

    public void DestroyAnimal()
    {
        scoreManager.AddScore(10);
        scoreManager.AddIndividualScore(animalName, 10);
        Destroy(gameObject);
    }

}
