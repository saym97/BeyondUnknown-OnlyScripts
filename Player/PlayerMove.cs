using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum state {
    walking,
    sprinting,
    jumping,
    jumpingSprint,
    crouching,
    shooting,
    scanning
}

public class PlayerMove : MonoBehaviour {

    public CharacterController controller;

    public float speed = 12f;
    public float speedModifier = 1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float staminaDrain = 0.2f;
    public float staminaRecover = 0.5f;
    public static float stamina = 100f;
    public bool canScan;
    float timer;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Image staminaBar;
    public GameObject anomalyBar;

    [SerializeField]
    AudioSource playerSounds;
    [SerializeField]
    AudioClip walkingClip;
    [SerializeField]
    AudioClip runningClip;
    int currentClip = 0;


    Vector3 velocity;
    bool isGrounded;
    bool infiniteStaminaCheat;
    bool nelioMode;

    public state currentState;

    //New bool created by Saym
    public bool idle;
    public Animator playerAnimator;
    
    
    [Header("Crouching")]
    public bool isCrouched = false;
    public GameObject playerHead;
    void Start() {
        currentState = state.walking;
        timer = Time.time;
        canScan = false;
        infiniteStaminaCheat = false;
        nelioMode = false;
    }

    // Update is called once per frame
    void Update() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        staminaBar.fillAmount = (stamina / 100f);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        //Infinite Stamina cheat
        if (Input.GetKeyDown(KeyCode.F3)) {
            infiniteStaminaCheat = !infiniteStaminaCheat;
        }

        //Nelio mode
        if (Input.GetKeyDown(KeyCode.F5)) {
            nelioMode = !nelioMode;
        }

        //Idle 
        if (currentState == state.walking) {
            if(!idle && (Mathf.Abs(x) == 0 && Mathf.Abs(z) == 0)) {
                idle = true;
                playerAnimator.SetBool("walk", false);
            } else if (idle && (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)) {
                playerAnimator.SetBool("walk", true);
                idle = false;
                
            }
        }
        //Sprint toggle on
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && currentState != state.shooting) {
            if (stamina > 0f) {
                currentState = state.sprinting;
                speedModifier = 2f;
            }   
        }

        //Crouch toggle on
        if (Input.GetKey(KeyCode.C) && isGrounded) {
            currentState = state.crouching;
            speedModifier = 0.5f;
            if (!isCrouched) {
                isCrouched = true;
                playerAnimator.SetBool("walk", false);
                LeanTween.moveLocalY(playerHead, 0.5f, 0.5f);
            }
            
        }

        //Crouch/Sprint toggle off
        if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.C)) && isGrounded) {
            currentState = state.walking;
            speedModifier = 1f;
            if (isCrouched) {
                isCrouched = false;
                LeanTween.moveLocalY(playerHead, 1.1f, 0.5f);
            }
            
        }

        //Shoot
        if (currentState == state.shooting) {
            speedModifier = 0.6f;
        } 

        //Near anomaly check
        if (canScan) {
            if (Input.GetKey(KeyCode.E)) {
                anomalyBar.SetActive(true);
                currentState = state.scanning;
            } else {
                anomalyBar.SetActive(false);
                currentState = state.walking;
            }
        } else {
            if(currentState == state.scanning) {
                anomalyBar.SetActive(false);
                currentState = state.walking;
            }
        }

        //State change on landing
        if (isGrounded) {
            if (currentState == state.jumping) {
                currentState = state.walking;
                speedModifier = 1f;
            }else if (currentState == state.jumpingSprint) {
                if(Input.GetKey(KeyCode.LeftShift) && currentState != state.shooting) {
                    currentState = state.sprinting;
                    speedModifier = 2f;
                } else {
                    currentState = state.walking;
                    speedModifier = 1f;
                }
            }
        }

        //Stamina drain
        if(currentState == state.sprinting && Time.time - timer > staminaDrain && stamina > 0f && !infiniteStaminaCheat && !nelioMode) {
            stamina--;      
            timer = Time.time;
        }

        //State change on stamina drained
        if(currentState == state.sprinting && stamina == 0f && !infiniteStaminaCheat && !nelioMode) {
            currentState = state.walking;
            speedModifier = 1f;
        }

        //Stamina recovery walking 
        if (currentState == state.walking && Time.time - timer > staminaRecover && stamina < 100f && !infiniteStaminaCheat && !nelioMode) {
            stamina += 2;
            if (stamina > 100f) stamina = 100f;
            timer = Time.time;
        }

        //Stamina revovery crouching 
        if (currentState == state.crouching && Time.time - timer > staminaRecover && stamina < 100f) {
            stamina += 4;
            if (stamina > 100f) stamina = 100f;
            timer = Time.time;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if(currentState == state.sprinting && move == Vector3.zero) {
            currentState = state.walking;
        }

        //Sound effects
        if(move != Vector3.zero) {
            if ((currentState == state.walking || currentState == state.shooting || currentState == state.crouching) && (currentClip != 1 || !playerSounds.isPlaying)) {
                currentClip = 1;
                playerSounds.Stop();
                playerSounds.clip = walkingClip;
                playerSounds.Play();
            } else if (currentState == state.sprinting && currentClip != 2) {
                currentClip = 2;
                playerSounds.Stop();
                playerSounds.clip = runningClip;
                playerSounds.Play();
            } else if(!isGrounded){
                currentClip = 0;
                playerSounds.Stop();
            }
        } else {
            currentClip = 0;
            playerSounds.Stop();
        }  

        controller.Move(move * speed * speedModifier * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded) {
            if(currentState == state.sprinting) {
                currentState = state.jumpingSprint;
                speedModifier = 2f;
            } else {
                currentState = state.jumping;
                speedModifier = 1f;
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void shootingStance(bool type) {
        if (type) {
            currentState = state.shooting;
        } else {
            currentState = state.walking;
            speedModifier = 1f;
        }
    }
}
