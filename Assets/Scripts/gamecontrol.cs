using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Source of Images, all images are free.
//Dracola, https://www.pngfuel.com/free-png/afima 
//saw, https://svgsilh.com/image/2022676.html 
//Mr.Bean, https://www.seekpng.com/ipng/u2q8i1y3y3a9e6a9_mr-bean-cake-bean-cakes-cute-characters-novelty/
//Man with mask, https://dlpng.com/png/6820244 
//background https://www.freepik.com/free-vector/game-2d-park-landscape_3948169.htm#page=12&query=game+background&position=41


public class gamecontrol : MonoBehaviour
{
    
    [SerializeField]
    private GameObject spawnPoint ;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Transform stompCheck;


    [SerializeField]
    private float stompRadius =0.2f;


    [SerializeField]
    Sprite player2;



    private int NoOfEnemeis = 0;
    private float maxSpeed = 10f;
    private float move;
    private float move2;
    private Rigidbody2D rb;
    private float jump = 70f;
    private bool grounded = false;
    private bool doubleJump = false;
    private bool spacePressed ;
    private int health ;
    private int lives ;

    void Start()

    {
        health = 2;
        lives = 2;
       
        rb = GetComponent<Rigidbody2D>();
        NoOfEnemeis = 0;
  
        int num = (int)(Random.value * 10);

        if (num % 2 == 0)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = player2;
            move = 0;
        }
    }

    // Update is called once per frame
    void Update()

    {
        move = Input.GetAxis("Horizontal");
        move2 = Input.GetAxis("Vertical");
        spacePressed = Input.GetKeyDown("space");
        Debug.Log("health = " + health + "  lives = " + lives); 
    }


    private void checkStomp()
    {
        Collider2D stomped = Physics2D.OverlapCircle
            (
               stompCheck.position,
               stompRadius,
               enemyLayer);
        if(stomped != null)
        {
            Destroy(stomped.gameObject);
            Debug.Log("stomped");
            NoOfEnemeis += 1;
        }
    }

   
    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(
               stompCheck.position,
               stompRadius,
               groundLayer);
        rb.velocity = new Vector2(move * maxSpeed, move2 * maxSpeed);

         checkStomp();
        if (grounded)
        {
             if (spacePressed || doubleJump)
            {
                spacePressed = false;
                rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jump);
            }
        } 
    }


   private void OnCollisionEnter2D(Collision2D collision)

   {

    string collideTag = collision.gameObject.tag;
    if (collideTag == "enemy") // if Mr Bean hits enemies, they will be destroyed and number of killed enemies increased by 1.
    {
            //  this.gameObject.transform.SetPositionAndRotation(spawnPoint.transform.position, Quaternion.identity);
            //rb.velocity = Vector2.zero;
            //grounded = false;
          //  NoOfEnemeis += 1;
           decreaseHealth();
    }


  
    if (collideTag == "hazard")// if Mr Bean hits any saw, he will return to start point which has the same cordinate of
                               //starting position of Mr.Bean
    {
        this.gameObject.transform.SetPositionAndRotation(spawnPoint.transform.position, Quaternion.identity);

    }

    if (collideTag == "floor")
    {
       // grounded = true;
        doubleJump = false;
    }

   }


    private void decreaseHealth()
    {
        health -= 1;
        
        if (lives != 0)
        {
            this.gameObject.transform.SetPositionAndRotation(spawnPoint.transform.position, Quaternion.identity);
            rb.velocity = Vector2.zero;
        }


        if (health == 0)
        {
            lives -= 1;
            health = 2;    
        }

       
        if (lives == 0)
        {
            grounded = false;
            SceneManager.LoadScene("Lose");
            health =0;
            
            
        }

    }
    

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            grounded = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "goal" && NoOfEnemeis == 4)

        {
            SceneManager.LoadScene("WinScene");
        }
    }



}



