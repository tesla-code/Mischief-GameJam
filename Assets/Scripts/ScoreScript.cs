using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class ScoreScript : MonoBehaviour
{

    /// <summary>
    /// The text that displays how many tags the player currently owns
    /// </summary>
    [SerializeField] public Text Score;

    // Start is called before the first frame update
    void Start()
    {
        Score = GetComponentInChildren<Text>();

        UpdateScore(0);
    }

    public void UpdateScore(int points)
    {
        Score.text = "Score: " + points;
    }

}
