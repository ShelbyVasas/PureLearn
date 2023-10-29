using UnityEngine;

public class MissMarksManager : MonoBehaviour
{
    public SpriteRenderer[] missMarks; // Drag your 3 SpriteRenderers here in the inspector

    private int currentMisses = 0;

public bool RegisterMiss()
{
    if (currentMisses < missMarks.Length)
    {
        switch (currentMisses)
        {
            case 0:
                missMarks[currentMisses].color = Color.green;
                break;
            case 1:
                missMarks[currentMisses].color = Color.yellow;
                break;
            case 2:
                missMarks[currentMisses].color = Color.red;
                break;
        }

        currentMisses++;
    }

    if (currentMisses >= 3)
    {
        return true;  // Indicating that the game should end
    }
    return false;  // Game should not end yet
}


    public void ResetMisses()
    {
        for (int i = 0; i < missMarks.Length; i++)
        {
            missMarks[i].color = Color.white; // reset to original color or any other color
        }
        currentMisses = 0;
    }
}
