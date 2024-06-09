using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHorizon
{
    public class Player : SoundManager
    {
        [SerializeField] private float speed;

        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float crouchSpeed;

        [SerializeField] private float jumpForce;
        [SerializeField] private float crouchHeight;
        [SerializeField] private float crouchCamSpeed;
        bool isGrounded = false;

        private Rigidbody _rb;

        public GameObject main;

        public Transform respawnPoint;

        float hor;
        float ver;

        private CapsuleCollider col;
        float colcent;

        bool isCrouch = false;
        Vector3 nextPos;
        Vector3 V3;

        public Camera cam;
        float camPositionY;

        public Image staminaBar;
        public Image healthBar;
        public Image satietyBar;

        public static float currentHealth = 100;
        public static float currentSatiety = 100;
        public static float timeToOverHunger = 60;
        public static float hungerDeath = 60;

        public GameObject diedPanel;
        public Animator dieAnimation;
        public float stepTime;

        private bool isAlive = true;
        public bool canNoise = false;

        [SerializeField] Animator mainAnim;


        private void Start()
        { 
            _rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            colcent = col.center.y;
            camPositionY = cam.transform.localPosition.y;
            currentStamina = maxStamina;
            col.height *= crouchHeight;
            col.center = new Vector3(col.center.x, colcent - (col.height / crouchHeight - col.height) / 2f, col.center.z);
            isCrouch = true;
            nextPos = new Vector3(0, camPositionY - (col.height / crouchHeight - col.height), 0);
            col.height /= crouchHeight;
            col.center = new Vector3(col.center.x, colcent, col.center.z);
            isCrouch = false;
            nextPos = new Vector3(0, camPositionY, 0);
            Starting();
            stepTime = 0.33f;
            StartCoroutine(Stepsound(stepTime));
        }

        private void FixedUpdate()
        {
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");

            _rb.velocity = new Vector3(0f,_rb.velocity.y,0f) + transform.forward * ver * speed + transform.right * hor * speed;

            mainAnim.SetFloat("x", hor);
            mainAnim.SetFloat("y", ver);

        }
        private void Update()
        {
            Jump();
            Crouch();
            SpeedControl();

            StaminaChecked();
            StaminaKeys();

            StateControl();
            ChangeVolume();

            V3 = this.transform.position;
            if(V3.y < -30) Kill();
        }

        private void StateControl()
        {
            if (currentHealth > 0)
            {
                if (currentSatiety <= 0)
                {
                    currentSatiety = 0;
                    currentHealth -= 100 / hungerDeath * Time.deltaTime;
                }
                else
                {
                    currentSatiety -= 100 / timeToOverHunger * Time.deltaTime;
                }


                if(currentSatiety >= 85 && currentHealth < 100)
                {
                    currentHealth += 100 / hungerDeath + Time.deltaTime;
                }

                satietyBar.fillAmount = currentSatiety / 100;
                healthBar.fillAmount = currentHealth / 100;
            }
            else
            {
                if (isAlive)
                {
                    Kill();
                }
            }
        }
        void OnCollisionEnter()
        {
            isGrounded = true;
            if (canNoise)
            {
                PlaySound(0);
                canNoise = false;
            }
        }

        private void Jump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded) 
            {
                isGrounded = false;
                _rb.AddForce(0,jumpForce,0);

                mainAnim.SetTrigger("Jump");
            }
        }
        
        private void Crouch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                col.height *= crouchHeight;
                col.center = new Vector3(col.center.x, colcent - (col.height/crouchHeight - col.height) / 2f, col.center.z);
                isCrouch = true;
                nextPos = new Vector3(0, camPositionY - (col.height / crouchHeight - col.height),0);

                mainAnim.SetBool("isCrouch", isCrouch);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                col.height /= crouchHeight;
                col.center = new Vector3(col.center.x, colcent, col.center.z);
                isCrouch = false;
                nextPos = new Vector3(0, camPositionY , 0);

                mainAnim.SetBool("isCrouch", isCrouch);
            }
            if(isCrouch)
            {
                cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, nextPos, crouchCamSpeed);
            } else
            {
                cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, nextPos, crouchCamSpeed);
            }
        }

        private void SpeedControl()
        {
            speed = Input.GetKey("left shift") && !isCrouch && currentStamina > 1 ? runSpeed : walkSpeed;
            speed = isCrouch ? crouchSpeed : speed;

            mainAnim.SetBool("isRun", Input.GetKey("left shift"));
        }

        //----------------------------------------
        [Space(20)]
        [Space(10)]
        [SerializeField] private float currentStamina;
        [SerializeField] private float maxStamina;
        [SerializeField] private float minStamina;

        [SerializeField] private float addStamina;
        [SerializeField] private float removeStamina;


        private void StaminaChecked()
        {
            if (currentStamina <= minStamina)
                currentStamina = minStamina;

            if (currentStamina >= maxStamina)
                currentStamina = maxStamina;
        }

        private void StaminaKeys()
        {
            if (speed == runSpeed && (_rb.velocity.x > 0.2f || _rb.velocity.z > 0.2f || _rb.velocity.x < -0.2f || _rb.velocity.z < -0.2f))
            {
                currentSatiety -= Time.deltaTime * 100 / timeToOverHunger * 2f ;
                currentStamina -= Time.deltaTime * removeStamina;
            }
            else if (!Input.GetKey("left shift") || isCrouch)
                currentStamina += Time.deltaTime * addStamina;
            staminaBar.fillAmount = currentStamina/100;
        }


        private void Kill()
        {
            isAlive = false;
            diedPanel.SetActive(true);
            dieAnimation.Play("die");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            main.GetComponent<RotatePl>().enabled = false;

            mainAnim.SetTrigger("death");
        }
        public void Respawn()
        {

            isAlive = true;
            diedPanel.SetActive(false);
            main.GetComponent<RotatePl>().enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            this.transform.position = respawnPoint.transform.position;
            currentHealth = 100;
            currentSatiety = 100;
            currentStamina = 100;
            _rb.velocity = Vector3.zero;

            mainAnim.SetTrigger("respawn");

        }

        IEnumerator Stepsound(float _stepTime)
        {
            yield return new WaitForSeconds(_stepTime);
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && isGrounded)
            {
                PlayRandomSound(1);
            }
            StartCoroutine(Stepsound(stepTime));

        }


    }
}

