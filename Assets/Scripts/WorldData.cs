using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to store locations of Areas that can be sparyed/tagged.
/// </summary>
public class WorldData : MonoBehaviour
{
    /// <summary>
    /// The ID of the player that is currently running the client.
    /// </summary>
    public uint PlayerAttackID;

    /// <summary>
    /// Contains the locations where the players can tag
    /// </summary>
    public List<GameObject> tagLocations;

    /// <summary>
    /// Used to change UI
    /// </summary>
    public ScoreScript MyScoreScript;

    public void Start()
    {
        // grabs the score script
        GameObject goWscript = GameObject.Find("Score UI");
        MyScoreScript = goWscript.GetComponent<ScoreScript>();
    }

    public void Update()
    {
        int score = 0;
        for(int i = 0; i < tagLocations.Count; i++)
        {
            // get owner of tag location and compare it with player ID
            if (tagLocations[i].GetComponent<GraffityBoard>().playerOwnerID == PlayerAttackID)
            {
                score++;
            }
        }
        MyScoreScript.UpdateScore(score);
    }

}
