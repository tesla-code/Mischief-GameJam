using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    /// <summary>
    /// How many times the player can tag.
    /// </summary>
    public int AmmoLeft = 3;

    /// <summary>
    /// The text that displays how many tags the player currently owns
    /// </summary>
    [SerializeField] public Text AmmoText;

    void Start()
    {
        GameObject go = GameObject.Find("AmmoUI");
        AmmoText = go.GetComponentInChildren<Text>();

        UpdateAmmotext(AmmoLeft);
    }

    public void UpdateAmmotext(int ammo)
    {
        AmmoText.text = "Ammo: " + ammo;
    }

}
