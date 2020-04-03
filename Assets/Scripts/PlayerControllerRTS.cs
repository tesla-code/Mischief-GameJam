using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

using UnityEngine.UI;

/* Code is based on the example room code from the mirror website */

namespace RTS.Networking
{
    /// <summary>
    /// This scripts makes the player controll their own character and makes sure 
    /// it is on their client.
    /// </summary>
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControllerRTS : NetworkBehaviour
    {
        /// <summary>
        /// UI component that displays the loading bar. 
        /// </summary>
        [SerializeField]
        public GameObject AttackUI;

        /// <summary>
        /// WorldData
        /// </summary>
        [SerializeField]
        public GameObject objectWithScript;

        /// <summary>
        /// Script that contains information about the world the player operates in
        /// </summary>
        WorldData worldDataScript;

        /// <summary>
        /// Gameobject with progress bar script
        /// </summary>
        [SerializeField]
        public GameObject objectWithProgressScript;

        /// <summary>
        /// Used to track progress of graffiting
        /// </summary>
        ProgrssBar progressBarScript;

        /// <summary>
        /// How The distance the drone can spray and have an effect on target.
        /// </summary>
        float _attackRange = 7f;

        /// <summary>
        /// The particles that are sprayed when the player is attacking
        /// </summary>
        [SerializeField]
        public ParticleSystem sprayEffect;

        /// <summary>
        /// How many Points the player currently has
        /// </summary>
        public int Points = 0;

        /// <summary>
        /// Standard unity controller
        /// </summary>
        public CharacterController characterController;

        /// <summary>
        /// Reference to the script that contais information about ammounition
        /// </summary>
        public Ammo AmmoScript;

        void OnValidate()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            Camera.main.orthographic = false;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0f, 3f, -8f);
            Camera.main.transform.localEulerAngles = new Vector3(10f, 0f, 0f);

            // Grab refrence of world data script
            objectWithScript = GameObject.Find("WorldData");
            worldDataScript = objectWithScript.GetComponent<WorldData>();
            worldDataScript.PlayerAttackID = netId;

            // Get refrence to Progress bar script
            objectWithProgressScript = GameObject.Find("vica");
            progressBarScript = objectWithProgressScript.GetComponent<ProgrssBar>();

            // Get refrence to UI
            AttackUI = GameObject.Find("Attacking");
            AttackUI.SetActive(false);

            // Use as player ID in game
            progressBarScript.PlayerAttackID = netId;

            // Get ref to ammo script
            GameObject gbAscript = GameObject.Find("AmmoSystem");
            AmmoScript = gbAscript.GetComponent<Ammo>();
        }

        private void OnDisable()
        {
            if(isLocalPlayer && Camera.main != null)
            {
                Camera.main.orthographic = true;
                Camera.main.transform.SetParent(null);
                Camera.main.transform.localPosition = new Vector3(0f, 70f, 0f);
                Camera.main.transform.localEulerAngles = new Vector3(90f, 0f, 0f);                 
            }
        }

        [Header("Movement Settings")]
        public float moveSpeed = 8f;
        public float turnSensitivity = 5f;
        public float maxTurnSpeed = 150f;

        [Header("Diagnostics")]
        public float horizontal = 0f;
        public float vertical = 0f;
        public float turn = 0f;
        public float jumpSpeed = 0f;
        public bool isGrounded = true;
        public bool isFalling = false;
        public Vector3 velocity;

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            // Q and E cancel each other out, reducing the turn to zero
            if (Input.GetKey(KeyCode.Q))
                turn = Mathf.MoveTowards(turn, -maxTurnSpeed, turnSensitivity);
            if (Input.GetKey(KeyCode.E))
                turn = Mathf.MoveTowards(turn, maxTurnSpeed, turnSensitivity);
            if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
                turn = Mathf.MoveTowards(turn, 0, turnSensitivity);
            if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                turn = Mathf.MoveTowards(turn, 0, turnSensitivity);

            if (isGrounded)
                isFalling = false;

            if ((isGrounded || !isFalling) && jumpSpeed < 1f && Input.GetKey(KeyCode.Space))
                jumpSpeed = Mathf.Lerp(jumpSpeed, 1f, 0.5f);
            else if (!isGrounded)
            {
                isFalling = true;
                jumpSpeed = 0;
            }

            // to do: Tell other clients to enable effect
            if (Input.GetMouseButtonDown(0))
            {
                sprayEffect.Play();
                Debug.Log("Player Attacked.");

                // Check if in range of graffity target
                for(int i = 0; i < worldDataScript.tagLocations.Count; i++)
                {
                    if(Vector3.Distance(worldDataScript.tagLocations[i].transform.position, this.transform.position) <= _attackRange)
                    {

                        if(AmmoScript.AmmoLeft == 0)
                        {
                            Debug.Log("Not enough ammo");
                        }
                        else
                        {
                            progressBarScript.Target = worldDataScript.tagLocations[i];

                            AttackUI.SetActive(true);
                            Debug.Log("In range of graffity board");
                            progressBarScript.isAttacking = true;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                progressBarScript.Target = null;

                AttackUI.SetActive(false);
                progressBarScript.ResetAttack();

                sprayEffect.Stop();
                Debug.Log("Player Stopped Attacking.");
            }
        }

        void FixedUpdate()
        {
            if (!isLocalPlayer || characterController == null)
                return;

            transform.Rotate(0f, turn * Time.fixedDeltaTime, 0f);

            Vector3 direction = new Vector3(horizontal, jumpSpeed, vertical);
            direction = Vector3.ClampMagnitude(direction, 1f);
            direction = transform.TransformDirection(direction);
            direction *= moveSpeed;

            if (jumpSpeed > 0)
                characterController.Move(direction * Time.fixedDeltaTime);
            else
                characterController.SimpleMove(direction);

            isGrounded = characterController.isGrounded;
            velocity = characterController.velocity;
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Debug.Log("Player controller collided. ");

            if(hit.collider.tag == "ammo")
            {
                Destroy(hit.gameObject);
                Debug.Log("Hit ammo");
                AmmoScript.AmmoLeft++;
                AmmoScript.UpdateAmmotext(AmmoScript.AmmoLeft);
            }
        }
    }
}