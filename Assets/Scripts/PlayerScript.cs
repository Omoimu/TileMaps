using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    
    // Ground Checks
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;

    Vector3 characterScale;
    float characterScaleX;

    Animator anim; //animator

    //Score stuff//
    private int scoreValue = 0;
    private int playerLives = 3;
    public Text score;
    public Text lives;
    public Text results;
   // public Text hozText;
   // public Text jumpText;
    private bool facingRight = true;
    float hozMovement = 0;
    float vertMovement = 0;
    //audio stuff
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); //animator

        score.text = scoreValue.ToString();
        lives.text = playerLives.ToString();
        results.text = "";
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        hozMovement = Input.GetAxis("Horizontal");
        vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        /*
                if (hozMovement > 0 && facingRight == true)
                {
                    Debug.Log("Facing Right");
                    hozText.text = "Facing Right";
                }

                if (hozMovement < 0 && facingRight == false)
                {
                    Debug.Log("Facing Left");
                    hozText.text = "Facing Left";
                }

                if (vertMovement > 0 && isOnGround == false)
                {
                    Debug.Log("Jumping");
                    jumpText.text = "Jumping";
                }
                else if (vertMovement == 0 && isOnGround == true)
                {
                    Debug.Log("Not Jumping");
                    jumpText.text = "Not Jumping";
                }
        */
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue == 4)
            {
                transform.position = new Vector2(90.0f, -3.75f);
                playerLives = 3;
                lives.text = playerLives.ToString();

            }
            if (scoreValue == 8)
            {
                results.text = "You win! Game created by Dominique Mobley";
                audioSource.PlayOneShot(clip, volume);
            }
        }
        if (collision.collider.tag == "Enemy")
        {
            playerLives -= 1;
            lives.text = playerLives.ToString();
            Destroy(collision.collider.gameObject);
            if (playerLives == 0)
            {
                results.text = "You Lose!";

                this.gameObject.SetActive(false);
            }
        }

        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (hozMovement == 0) { anim.SetInteger("State", 0); }
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("State", 2);
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
        if (hozMovement != 0)
        {

                anim.SetInteger("State", 1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
    }


}

