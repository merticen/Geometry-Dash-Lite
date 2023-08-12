using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerShipController : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI winText, loseText;
    private bool isDead;
    private bool isWin;
    public GameObject playerShipSprite;
    private float thrust = 1000f;
    float speedValue = 15.6f;
    [SerializeField] ParticleSystem finishParticle, crashShipParticle, shipMovementParticle, confettiParticle;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isDead && isWin == false)
        {
            PlayerShipMovement();
            if ((Input.GetMouseButton(0) && !IsMouseOverUI()))
            {
                rb.AddRelativeForce(Vector3.up * thrust * Time.deltaTime);
            }
        }
      
    }
    private void PlayerShipMovement()
    {
        transform.position += Vector3.right * speedValue * Time.deltaTime;
        if (!shipMovementParticle.isPlaying)
        {
            shipMovementParticle.Play();
        }
    }
    private void KillPlayer()
    {
        speedValue = 0;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        shipMovementParticle.Stop();
        isDead = true;
        crashShipParticle.Play();
        loseText.gameObject.SetActive(true);
        playerShipSprite.SetActive(false);
        playerController.RestartGameDelay();
    }
    private void WinPlayer()
    {
        speedValue = 0;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        shipMovementParticle.Stop();
        playerShipSprite.SetActive(false);
        winText.gameObject.SetActive(true);
        confettiParticle.Play();
        playerController.RestartGameDelay();
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            KillPlayer();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("FinishControl"))
        {
            
            finishParticle.Play();
            isWin = true;
            WinPlayer();
            
        }
    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
