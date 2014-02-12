using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float	speed = 0f;
	public float	baseSpeed = 5f;
	public float	fireballCount = 0f;
	public Vector2	vel;
	private float 	maxRunSpeed = 10f;
	private float	maxWalkSpeed = 5f;
	private float	acc = 20f;
	private float	dec = 40f;
	private float	decNatural = 15f;
	private float	pipeSpeed = 2f;
	private float	growTimer = 0.5f;
	private float	fireTimer = 0.5f;
	private float	starTimer = 12f;
	private float	fireballTimer = 0f;

	public float	jumpForce = 500f;
	private float	jumpTime = 0f;
	private float 	heldVelocity = 6.3f;
	public bool		grounded = false;

	private float 	deathTime = 0f;
	public float 	deathForce = 1000f;

	private bool 			facingRight = true;
	public bool				inPipe = false;
	public bool 			goingDown = false;
	private bool 			win = false;
	private bool 			dead = false;
	private bool			stateChange = false;
	private bool			firstChange = false;
	private bool			shrinking = false;
	private bool			invincible = false;
	private bool			hitStar = false;

	public Animator			anim;
	public BoxCollider2D 	headCollider;
	public BoxCollider2D 	footCollider;
	public BoxCollider2D	triggerCollider;
	public BoxCollider2D	middleCollider;
	public CircleCollider2D rightFootCollider;
	public CircleCollider2D	leftFootCollider;
	public GameObject		mainCamera;
	public GameObject		fireball;
	public static Vector3	startPos = new Vector3 (3f, 1.5f, 0);

	public static bool		goingUp = false;
	public static float		numCoins = 0f;
	public static float		score = 0f;
	public static float		state = 0f;
	public static float		lives = 3f;
	public static float		timeLeft = 400f;
	public static string	lastLevel;
	private float 			marioTimeScale = 23f;

	public AudioClip		coinSound;
	public AudioClip		deathSound;
	public AudioClip		smallJumpSound;
	public AudioClip		bigJumpSound;
	public AudioClip		growSound;
	public AudioClip		shrinkSound;
	public AudioClip		hurrySound;
	public AudioClip		starSound;
	public AudioClip		gameMusic;
	public AudioClip		oneupSound;
	public AudioClip		fireballSound;

	public RuntimeAnimatorController	smallController;
	public RuntimeAnimatorController	largeController;
	public RuntimeAnimatorController	fireController;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator> ();
		UpdateAnimator ();
		gameObject.transform.position = startPos;
	}

	void Update()
	{ 	
		if(anim.GetBool("Win")){
			if(!win)
				rigidbody2D.velocity = new Vector2(0f, 0.5f);
			win = true;
		}

		//Make timescale not increase in guis
		if(Application.loadedLevelName == "LivesScreen" || 
		   Application.loadedLevelName == "GameOverScreen" || win){
			return;
		}
		else if(Application.loadedLevelName == "StartScreen"){
			timeLeft = 400f;
		}
		else{
			if (marioTimeScale > 0)
				marioTimeScale--;
			else{
				marioTimeScale = 23f;
				timeLeft--;
			}
		}

		if (timeLeft == 0 && !win)
			anim.SetBool ("Death", true);
		else if(timeLeft == 100 && !win){
			audio.Stop();
			mainCamera.audio.mute = true;
			audio.PlayOneShot(hurrySound);
			Invoke("SpeedMusic", 3f);
		}

		//death stuff
		if(anim.GetBool("Death") && !dead){
			marioTimeScale = 500f;
			mainCamera.audio.Stop ();
			audio.PlayOneShot(deathSound);
			Vector2 newVel = new Vector2(0f, 0f);
			rigidbody2D.velocity = newVel;
			dead = true;
			deathTime = 15f;
			return;
		}

		if(deathTime > 0){
			deathTime--;
			return;
		}
		else if(deathTime == 0 && dead){
			lastLevel = Application.loadedLevelName;
			lives--;
			Destroy(footCollider);
			Destroy(rightFootCollider);
			Destroy(leftFootCollider);
			Destroy(triggerCollider);
			Destroy(headCollider);
			Destroy(middleCollider);
			rigidbody2D.AddForce(new Vector2(0f, deathForce));
			deathTime = -1f;
			return;
		}
		else if(deathTime == -1f){
			if(transform.position.y < -200f) {
				timeLeft = 400f;
				state = 0;
				if(lives > 0)
					Application.LoadLevel("LivesScreen");
				else
					Application.LoadLevel("GameOverScreen");
			}
			return;
		}

		//state change
		if(stateChange){
			if(state == 0){
				if(firstChange){
					audio.PlayOneShot(shrinkSound);
					firstChange = false;
					rigidbody2D.velocity = new Vector2(0,0);
					Invoke("UpdateAnimator", growTimer);
					shrinking = true;
					gameObject.layer = LayerMask.NameToLayer("Item");
					Invoke("DoneShrinking", 3f);
					anim.SetBool("Shrink", true);
				}
				bool blink = gameObject.GetComponent<SpriteRenderer>().enabled;
				gameObject.GetComponent<SpriteRenderer>().enabled = !blink;
				return;
			}
			else if(state == 1){
				if(firstChange){
					audio.PlayOneShot(growSound);
					anim.SetBool("Grow", true);
					firstChange = false;
					rigidbody2D.velocity = new Vector2(0,0);
					Invoke("UpdateAnimator", growTimer);
				}
				return;
			}
			else if(state == 2){
				if(firstChange){
					audio.PlayOneShot(growSound);
					anim.SetBool("Fire", true);
					firstChange = false;
					rigidbody2D.velocity = new Vector2(0,0);
					Invoke("UpdateAnimator", fireTimer);
				}
				return;
			}
		}

		if(hitStar){
			mainCamera.audio.Stop();
			audio.PlayOneShot(starSound);
			invincible = true;
			hitStar = false;
			if(state == 0)
				triggerCollider.size = new Vector2(1.25f, 0.8f);
			else
				triggerCollider.size = new Vector2(1.75f, 1.8f);
			Invoke("DoneInvincible", starTimer);
		}

		if(shrinking || invincible){
			bool blink = gameObject.GetComponent<SpriteRenderer>().enabled;
			gameObject.GetComponent<SpriteRenderer>().enabled = !blink;
		}

		//Crouch stuff
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
			if(state > 0){
				headCollider.center = new Vector2(0f, 0.925f);
				triggerCollider.center = new Vector2(0f, 0.5f);
				triggerCollider.size = new Vector2(0.75f, 0.8f);
				middleCollider.center = new Vector2(0f, 0.5f);
				middleCollider.size = new Vector2(0.1f, 0.5f);
			}
			anim.SetBool ("Crouch", true);
		}
		else{
			if(state > 0){
				headCollider.center = new Vector2(0f, 1.925f);
				triggerCollider.center = new Vector2(0f, 1.0f);
				triggerCollider.size = new Vector2(1f, 1.8f);
				middleCollider.center = new Vector2(0f, 1f);
				middleCollider.size = new Vector2(0.1f, 1.5f);
			}
			anim.SetBool ("Crouch", false);
		}

		// Jump stuff
		if((Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.X))
		   && grounded){
			if(state == 0)
				audio.PlayOneShot(smallJumpSound);
			else
				audio.PlayOneShot(bigJumpSound);
			if(!anim.GetBool("Crouch")) anim.SetBool ("Jump", true);
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			jumpTime = 15f;
			if(anim.GetFloat("Speed") > 5f) heldVelocity += 1f;
			grounded = false;
		}

		if((Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.X))
		   && jumpTime != 0){
			jumpTime--;
			//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y+heldVelocity);
			rigidbody2D.AddForce(new Vector2(0f, heldVelocity*jumpTime));
		}
		else{
			jumpTime = 0;
			heldVelocity = 6.3f;
		}

		//Fire bullets
		if(fireballTimer != 0f) fireballTimer--;
		else if(state == 2f && fireballCount < 2f &&
		       (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Comma))){

			Debug.Log("Fire");
			Vector3 pos = transform.position;
			pos.y += 1f;
			if(facingRight){
				pos.x += 0.51f;
				fireball.GetComponent<FireBallScript>().left = false;
			}
			else{
				pos.x -= 0.51f;
				fireball.GetComponent<FireBallScript>().left = true;
			}
			fireballCount++;
			fireballTimer = 5f;
			audio.PlayOneShot(fireballSound);
			Instantiate(fireball, pos, Quaternion.identity);
		}

		//walk and run stuff
		float xAxisValue = Input.GetAxis("Horizontal");
		xAxisValue = Mathf.Round(xAxisValue * 100f) / 100f;
		vel = rigidbody2D.velocity;
		speed = 0f;

		if(Mathf.Abs(vel.x) < maxWalkSpeed){
			speed = xAxisValue * acc * Time.deltaTime;
			if(vel.x == 0f && xAxisValue > 0f) speed += 1f;
			else if(vel.x == 0f && xAxisValue < 0f) speed += -1f;
		}
		else if(!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.Comma)){
			if(vel.x > 0) speed = -dec * Time.deltaTime;
			else speed = dec * Time.deltaTime;
		}
		
		if((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Comma))
		   && Mathf.Abs(vel.x) < maxRunSpeed && grounded){
			speed = xAxisValue * acc * Time.deltaTime;
			if(vel.x == 0f && xAxisValue > 0f) speed += 1f;
			else if(vel.x == 0f && xAxisValue < 0f) speed += -1f;
		}
		
		if(Mathf.Abs(xAxisValue) == 0f && vel.x != 0){
			if(Mathf.Abs(vel.x) < maxWalkSpeed){
				if(vel.x > 0) speed = -decNatural * Time.deltaTime;
				else speed = decNatural * Time.deltaTime;
			}
			else{
				if(vel.x > 0) speed = -dec * Time.deltaTime;
				else speed = dec * Time.deltaTime;
			}
		}
		
		if(vel.x * xAxisValue < 0){
			if(vel.x > 0) speed += -dec * Time.deltaTime;
			else speed += dec * Time.deltaTime;
		}
		
		if(!grounded && Mathf.Abs(vel.x) < baseSpeed){
			if(xAxisValue > 0) speed = acc * Time.deltaTime;
			else if(xAxisValue < 0) speed = -acc * Time.deltaTime;
		}
		
		vel.x += speed;
		if(Mathf.Abs(vel.x) < 0.15f) vel.x = 0f;
		anim.SetFloat("Speed", Mathf.Abs(vel.x));

		if(facingRight && vel.x >= 0 || !facingRight && vel.x <= 0)
			anim.SetBool("Slide", false);

		//Pipe Stuff
		if(inPipe && (anim.GetBool ("Crouch") || goingDown)) {
			marioTimeScale = 500f;
			anim.SetBool("Crouch", true);
			goingDown = true;
			vel.x = 0f;
			vel.y = -pipeSpeed;
		}
		else if(inPipe){
			marioTimeScale = 500f;
			anim.SetFloat("Speed", 1f);
			vel.x = pipeSpeed;
			vel.y = 0f;
		}
		else if(anim.GetBool("Crouch") && grounded){
			if(rigidbody2D.velocity.x != 0f){
				float val;
				if(vel.x > 0) val = rigidbody2D.velocity.x + (-dec * Time.deltaTime);
				else val = rigidbody2D.velocity.x + (dec * Time.deltaTime);
				if(val < 0.15f) val = 0f;
				rigidbody2D.velocity = new Vector2(val, rigidbody2D.velocity.y);
			}
			return;
		}

		if(goingUp){
			marioTimeScale = 50f;
			vel.x = 0f;
			vel.y = pipeSpeed*2;
		}
		
		rigidbody2D.velocity = vel;

		if (xAxisValue > 0 && !facingRight){
			if(anim.GetFloat("Speed") > 0) anim.SetBool("Slide", true);
			Flip ();
		}
		else if(xAxisValue < 0 && facingRight){
			if(anim.GetFloat("Speed") > 0) anim.SetBool("Slide", true);
			Flip ();
		}
	}

	public void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnCollisionEnter2D(Collision2D collision){

		if(collision.contacts[0].otherCollider == headCollider)
			rigidbody2D.AddForce(new Vector2(0f, -300f));
		else if(collision.contacts[0].otherCollider == footCollider ||
		        collision.contacts[0].otherCollider == rightFootCollider ||
		        collision.contacts[0].otherCollider == leftFootCollider){
			if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")
			   && grounded == false){
				grounded = true;
				anim.SetBool("Jump", false);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.layer == LayerMask.NameToLayer("Enemies")){
			if(collider.gameObject.name == "Goomba"){
				if(invincible) collider.gameObject.GetComponent<GoombaController>().KillGoomba();
				else if(collider == collider.gameObject.GetComponent<GoombaController>().bodyCollider && 
				   !collider.gameObject.GetComponent<GoombaController>().squished){

					if(transform.position.y > collider.gameObject.transform.position.y+0.9f){}
					else if(state == 0) anim.SetBool("Death", true);
					else changeState(0);
				}
			}
			else if(collider.gameObject.name == "GreenKoopa"){
				if(invincible) collider.gameObject.GetComponent<KoopaController>().KillKoopa();
				else if(collider == collider.gameObject.GetComponent<KoopaController>().bodyCollider){
					
					if(transform.position.y > collider.gameObject.transform.position.y+0.9f){}
					else if(collider.gameObject.GetComponent<KoopaController>().squished &&
					        !collider.gameObject.GetComponent<KoopaController>().hit){
						
						collider.gameObject.GetComponent<KoopaController>().audio.PlayOneShot(
							collider.gameObject.GetComponent<KoopaController>().bumped);
						
						if(transform.position.x-collider.gameObject.transform.position.x < 0f)
							collider.gameObject.GetComponent<KoopaController>().speed *= -1;
						collider.gameObject.GetComponent<KoopaController>().anim.SetBool("Hit", true);
						collider.gameObject.GetComponent<KoopaController>().hit = true;
					}
					else if(state == 0) anim.SetBool("Death", true);
					else changeState(0);
				}
			}
		}
	}
	
	void OnTriggerStay2D(Collider2D collider){
		if(collider.gameObject.tag == "Block"){
			if(collider.gameObject.name == "HiddenBlock" &&
			   !collider.gameObject.GetComponent<HiddenBlocks>().hit) return;

			if(transform.position.x-collider.gameObject.transform.position.x > 0f){
				float pos = transform.position.x-collider.gameObject.transform.position.x;
				float force;
				if(state > 0)
					force = 400f * ((0.9f-pos)/0.9f);
				else
					force = 200f * ((0.9f-pos)/0.9f);
				rigidbody2D.AddForce(new Vector2(force, 0f));
			}
			else if(transform.position.x-collider.gameObject.transform.position.x < 0f){
				float pos = transform.position.x-collider.gameObject.transform.position.x;
				float force;
				if(state > 0)
					force = 400f * ((0.9f+pos)/0.9f);
				else
					force = 200f * ((0.9f+pos)/0.9f);
				rigidbody2D.AddForce(new Vector2(-force, 0f));
			}
		}
	}


	void DoneInvincible(){
		invincible = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		mainCamera.audio.PlayOneShot(gameMusic);
		if(state == 0)
			triggerCollider.size = new Vector2(0.75f, 0.8f);
		else
			triggerCollider.size = new Vector2(1f, 1.8f);
	}

	void DoneShrinking(){
		shrinking = false;
		gameObject.layer = LayerMask.NameToLayer("Player");
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		stateChange = false;
	}

	public void UpdateAnimator(){
		if(state == 0){
			anim.runtimeAnimatorController = smallController;
			headCollider.center = new Vector2(0f, 0.925f);
			triggerCollider.center = new Vector2(0f, 0.5f);
			triggerCollider.size = new Vector2(0.75f, 0.8f);
			middleCollider.center = new Vector2(0f, 0.5f);
			middleCollider.size = new Vector2(0.1f, 0.5f);
		}
		else if(state == 1){
			anim.runtimeAnimatorController = largeController;
			headCollider.center = new Vector2(0f, 1.925f);
			triggerCollider.center = new Vector2(0f, 1.0f);
			triggerCollider.size = new Vector2(1f, 1.8f);
			middleCollider.center = new Vector2(0f, 1f);
			middleCollider.size = new Vector2(0.1f, 1.5f);
		}
		else{
			anim.runtimeAnimatorController = fireController;
			headCollider.center = new Vector2(0f, 1.925f);
			triggerCollider.center = new Vector2(0f, 1.0f);
			triggerCollider.size = new Vector2(1f, 1.8f);
			middleCollider.center = new Vector2(0f, 1f);
			middleCollider.size = new Vector2(0.1f, 1.5f);
		}
		
		stateChange = false;
	}

	public void changeState(float newState){
		if((state == 0f && newState == 1f) ||
		   (state == 1f && newState == 2f) ||
		   (state == 1f && newState == 0f) ||
		   (state == 2f && newState == 0f))
		{
			state = newState;
			stateChange = true;
			firstChange = true;
			score += 1000;
		}
		else if(state == 0f && newState == 2f)
		{
			state = 1f;
			stateChange = true;
			score += 1000;
		}
		else if(state == 2f && newState == 2f)
		{
			score += 1000;
		}
	}
	
	public float getState(){
		return state;
	}
	
	public void addLife(){
		audio.PlayOneShot(oneupSound);
		lives++;
		score += 1000;
	}

	public float getLives(){
		return lives;
	}

	public void setMarioStart(Vector3 newPos){
		startPos = newPos;
	}

	public void setUp(bool set){
		goingUp = set;
	}

	public void addCoin(){
		audio.PlayOneShot(coinSound);
		numCoins++;
		if(numCoins == 100f){
			numCoins = 0;
			addLife();		
		}
		score += 200;
	}

	public float getCoins(){
		return numCoins;
	}

	public float getScore(){
		return score;
	}

	public void addScore(float amount){
		score += amount;
	}

	public float getTime(){
		return timeLeft;
	}

	public void subtractTime(){
		audio.time = .75f;
		audio.Play ();
		timeLeft--;
		score += 50f;
	}

	public string getLastLevel(){
		return lastLevel;
	}

	public void setLastLevel(string newLast){
		lastLevel = newLast;
	}

	public void HitStar(){
		hitStar = true;
	}

	public bool Invincible(){
		return invincible;
	}

	public void SubtractFireball(){
		fireballCount--;
	}

	public void SpeedMusic(){
		audio.Stop();
		mainCamera.audio.mute = false;
		mainCamera.audio.pitch = 1.15f;
	}

	public void initVariables(){
		timeLeft = 400f;
		numCoins = 0f;
		score = 0f;
		startPos = new Vector3 (3f, 1.5f, 0);
		lives = 3f;
		state = 0f;
	}
}