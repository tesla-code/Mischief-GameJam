using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// Progress Bar
/// </summary>
public class ProgrssBar : MonoBehaviour
{
    /// <summary>
    /// Indicates if the player is currently attacking
    /// </summary>
    public bool isAttacking = true;

    /// <summary>
    /// How fast the player fills the progress bar
    /// </summary>
    private float attackSpeed = 0.3f;

    /// <summary>
    /// The image we want to fill.
    /// </summary>
    private Image imageComp;

    /// <summary>
    /// 
    /// </summary>
    private RectTransform rectComponent;

    /// <summary>
    /// UI component that displays the loading bar. 
    /// </summary>
    [SerializeField]
    public GameObject AttackUI;

    /// <summary>
    /// 
    /// </summary>
    public bool CompletedSpray = false;

    /// <summary>
    /// The Current Target beeing Sprayed.
    /// </summary>
    public GameObject Target;

    /// <summary>
    /// 
    /// </summary>
    public uint PlayerAttackID;

    /// <summary>
    /// Reference to the script that contais information about ammounition
    /// </summary>
    public Ammo AmmoScript;


    public void Awake()
    {
        // Get ref to ammo script
        GameObject gbAscript = GameObject.Find("AmmoSystem");
        AmmoScript = gbAscript.GetComponent<Ammo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();

        // Get refrence to UI
        AttackUI = GameObject.Find("Attacking");
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking)
        {
            Progress(attackSpeed  * Time.deltaTime);
        }
    }

    /// <summary>
    /// Progress on the spraying
    /// </summary>
    private void Progress(float attSpeed)
    {
        imageComp.fillAmount += attSpeed;

        if(imageComp.fillAmount == 1)
        {
            GraffityBoard gb = Target.GetComponent<GraffityBoard>();
            gb.playerOwnerID = PlayerAttackID;

            AmmoScript.AmmoLeft--;
            AmmoScript.UpdateAmmotext(AmmoScript.AmmoLeft);

            Debug.Log("Attack Finished");
            CompletedSpray = true;
            ResetAttack();
        }
    }

    /// <summary>
    /// Resets the attack progress
    /// </summary>
    public void ResetAttack()
    {
        isAttacking = false;
        imageComp.fillAmount = 0;
        AttackUI.SetActive(false);
    }
}
