using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    public CameraController cameraController;
    public GameObject playerSprite;
    public GameObject player;
    public GameObject playerOnShip;
    public TextMeshProUGUI loseText;
    private bool isDead;
    public float speedValue = 10.4f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public Transform Sprite;
    public float restartDelay = 1f;
    [SerializeField] ParticleSystem crashParticle, movementParticle, portalParticle;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (!isDead)
        {
            PlayerMovement();

            if (IsGrounded())
            {
                Vector3 Rotation = Sprite.rotation.eulerAngles;
                Rotation.z = Mathf.Round(Rotation.z / 90) * 90;
                Sprite.rotation = Quaternion.Euler(Rotation);


                if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
                {
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * 26.6581f, ForceMode2D.Impulse);

                }
            }
            else
            {
                Sprite.Rotate(Vector3.back * 1);
            }

        }
        
    }
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            KillPlayer();
        }
        if (collision.gameObject.CompareTag("EntrancePortal"))
        {
            portalParticle.Play();
        }

        if (collision.gameObject.CompareTag("PortalSwap"))
        {
            player.SetActive(false);
            cameraController.SwitchToNewTarget();
            playerOnShip.SetActive(true);
        }
    }
    private void PlayerMovement()
    {
        transform.position += Vector3.right * speedValue * Time.deltaTime;
        if (!movementParticle.isPlaying)
        {
            movementParticle.Play();
        }
    }
    private void KillPlayer()
    {
        movementParticle.Stop();
        isDead = true;
        crashParticle.Play();
        loseText.gameObject.SetActive(true);
        playerSprite.SetActive(false);
        RestartGameDelay();
    }

    

    public void RestartGameDelay()
    {
        Invoke("RestartGame", restartDelay);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
