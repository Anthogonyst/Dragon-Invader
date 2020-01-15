using UnityEngine;
using System.Collections;

public class rPlayerController : MonoBehaviour {

	public float speed = 200f;
	public float jumpForce = 310f;
    private float xSpeed, ySpeed;
    public float acceleration, deceleration, maxSpeed;
	private bool isGrounded = false;
	public Transform groundChek;
	public LayerMask groundLayer, blockLayer;
	private float groundCheckRadius = .18f;
	private bool isFacingRight, idle, jump, jumpagain, doublejump;
    private new AudioSource audio;
    public AudioClip enemySound, collectibleSound, jump1, jump2;
	private Animator anim;
	public float jumpDelay;
	private float nextJumpTime;

	// Use this for initialization
	void Start () {
        //initialize your stuffs here.
        maxSpeed = 4;
        xSpeed = 0;
		ySpeed = 0;
        acceleration = 10;
        deceleration = 20;
        anim = GetComponent<Animator> ();
		isFacingRight = true;
		idle = true;
        jumpagain = false;
        doublejump = false;
		anim.SetBool ("isIdle", true);
        audio = GetComponent<AudioSource>();
		jumpDelay = .3f;
		nextJumpTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
            jump = true;
		}
		if (Input.GetButtonDown("Fire1")) {
			audio.PlayOneShot(collectibleSound, 1f);
		}
        // Double jumping
        if (!isGrounded && Input.GetButtonDown("Jump"))
        {
            jumpagain = true;
        }
    }

    void FixedUpdate() 
	{
		isGrounded = Physics2D.OverlapCircle (groundChek.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle (groundChek.position, groundCheckRadius, blockLayer);
		anim.SetBool ("isGrounded", isGrounded);
        if (jump)
        {
            if (isGrounded)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
				JumpSoundEffect();
            }
            else
            {
                jump = false;
            }
        }
        //
        //      Double jumping!
        //
        if (jumpagain)
        {
            if (!isGrounded && !doublejump)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
                doublejump = true;
				audio.PlayOneShot(jump2);
            }
            else
            {
                jumpagain = false;
            }
        }
        if (isGrounded)
        {
            doublejump = false;
        }
        //
        //
        //
		float moveX = Input.GetAxis ("Horizontal");
        if ((Input.GetKey(KeyCode.A)) && (xSpeed > -maxSpeed))
            xSpeed = xSpeed - acceleration * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.D)) && (xSpeed < maxSpeed))
            xSpeed = xSpeed + acceleration * Time.deltaTime;
        else
        {
            if (xSpeed > deceleration * Time.deltaTime)
                xSpeed = xSpeed - deceleration * Time.deltaTime;
            else if (xSpeed < -deceleration * Time.deltaTime)
                xSpeed = xSpeed + deceleration * Time.deltaTime;
            else
                xSpeed = 0;
        }        
        Vector2 moving = new Vector2(xSpeed, 0);        
		
		if((moveX > 0.0f && !isFacingRight) || (moveX < 0.0f && isFacingRight)) {
			Flip();
		}		
		
		float moveY = Input.GetAxis ("Vertical");
        if ((Input.GetKey(KeyCode.S)) && (ySpeed > -maxSpeed))
            ySpeed = ySpeed - acceleration * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.W)) && (ySpeed < maxSpeed))
            ySpeed = ySpeed + acceleration * Time.deltaTime;
        else
        {
            if (ySpeed > deceleration * Time.deltaTime)
                ySpeed = ySpeed - deceleration * Time.deltaTime;
            else if (ySpeed < -deceleration * Time.deltaTime)
                ySpeed = ySpeed + deceleration * Time.deltaTime;
            else
                ySpeed = 0;
        }        
        Vector2 moving2 = new Vector2(0, ySpeed);        
		if (moveX != 0f || moveY != 0f) {  
			idle = false;
		} else {
			idle = true;
		}
		anim.SetBool ("isIdle", idle);
		if((moveY > 0.0f && !isFacingRight) || (moveY < 0.0f && isFacingRight)) {
			Flip();
		}		
		
		GetComponent<Rigidbody2D>().velocity = moving + moving2;


	}

	void Flip() {
		isFacingRight = !isFacingRight;
		Vector3 playerScale = this.transform.localScale;
		//playerScale.x *= -1;
		this.transform.localScale = playerScale;
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coin")
        {
            audio.PlayOneShot(collectibleSound);
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "mushroom")
        {
            audio.PlayOneShot(enemySound);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Red")
        {
            
        }
    }

	void JumpSoundEffect() {
		if (Time.time >= nextJumpTime) {
			audio.PlayOneShot(jump1);
			nextJumpTime = Time.time + jumpDelay;
		}
	}

}
