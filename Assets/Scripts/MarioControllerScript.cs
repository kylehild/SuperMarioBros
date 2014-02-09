using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float	speed;
	private float 	runSpeed = 10f;
	private float	walkSpeed = 5f;
	private float	pipeSpeed = 2f;
	private float	growTimer = 0.5f;
	private float	fireTimer = 0.5f;
	private float	starTimer = 11.25f;

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

	public Animator			anim;
	public BoxCollider2D 	headCollider;
	public BoxCollider2D 	footCollider;
	public BoxCollider2D	triggerCollider;
	public CircleCollider2D rightFootCollider;
	public CircleCollider2D	leftFootCollider;
	public GameObject		mainCamera;
	public static Vector3	startPos = new Vector3 (3f, 1.5f, 0);

	public static bool		goingUp = false;
	public static float		numCoins = 0f;
	public static float		score = 0f;
	public static float		state = 0f;
	public static float		lives = 3f;
	public static float		timeLeft = 400f;
	public static string	lastLevel;
	private float 			marioTimeScale = 23f;

	public AudioClip		gameMusic;
	public AudioClip		coinSound;
	public AudioClip		deathSound;
	public AudioClip		smallJumpSound;
	public AudioClip		bigJumpSound;
	public AudioClip		growSound;
	public AudioClip		hurrySound;
	public AudioClip		starSound;

	public RuntimeAnimatorController	smallController;
	public RuntimeAnimatorController	largeController;
	public RuntimeAnimatorController	fireController;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator> ();
		UpdateAnimator ();
		speed = walkSpeed;
		gameObject.transform.position = startPos;
	}

	void Update()
	{ 	
		if(anim.GetBool("Win")){
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
			rigidbody2D.AddForce(new Vector2(0f, deathForce));
			deathTime = -1f;
			return;
		}
		else if(deathTime == -1f){
			if(transform.position.y < -200f) {
				timeLeft = 400f;
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
					audio.PlayOneShot(growSound);
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
			else {
				mainCamera.audio.Stop ();
				audio.PlayOneShot(starSound);
				//anim.SetBool("Fire", true);
				stateChange = false;
				//rigidbody2D.velocity = new Vector2(0,0);
				//UpdateAnimator();
				invincible = true;
				Invoke("DoneInvincible", starTimer);
			}
		}

		if(shrinking){
			bool blink = gameObject.GetComponent<SpriteRenderer>().enabled;
			gameObject.GetComponent<SpriteRenderer>().enabled = !blink;
		}

		if(invincible){
			bool blink = gameObject.GetComponent<SpriteRenderer>().enabled;
			gameObject.GetComponent<SpriteRenderer>().enabled = !blink;
		}

		// Jump stuff
		if((Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.X))
		   && grounded){
			if(state == 0)
				audio.PlayOneShot(smallJumpSound);
			else
				audio.PlayOneShot(bigJumpSound);
			anim.SetBool ("Jump", true);
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

		//Crouch stuff
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			anim.SetBool ("Crouch", true);
		else
			anim.SetBool ("Crouch", false);

		//walk and run stuff
		float xAxisValue = Input.GetAxis("Horizontal");
		Vector2 vel = rigidbody2D.velocity;

		if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Comma)){
			speed = runSpeed;
		}
		else{
			speed = walkSpeed;
		}

		if(!anim.GetBool("Slide")){
			vel.x = xAxisValue * speed;
			anim.SetFloat ("Speed", Mathf.Abs(vel.x));
		}
		else if(rigidbody2D.velocity.x == 0)
			anim.SetBool ("Slide", false);
		else{
			if(rigidbody2D.velocity.x < -0.05f) 
				vel.x = rigidbody2D.velocity.x + 0.05f;
			else if(rigidbody2D.velocity.x > 0.05f) 
				vel.x = rigidbody2D.velocity.x - 0.05f;
			else 
				vel.x = 0;
			anim.SetFloat ("Speed", Mathf.Abs(vel.x));
		}

		//Pipe Stuff
		if(inPipe && (anim.GetBool ("Crouch") || goingDown)) {
			marioTimeScale = 500f;
			goingDown = true;
			vel.x = 0f;
			vel.y = -pipeSpeed;
		}
		else if(inPipe){
			marioTimeScale = 500f;
			vel.x = pipeSpeed;
			vel.y = 0f;
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
			Debug.Log("Hit head");
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
			if(collider == collider.gameObject.GetComponent<GoombaController>().bodyCollider && 
			   !collider.gameObject.GetComponent<GoombaController>().anim.GetBool("Squished") &&
			   !invincible){

				if(state == 0) anim.SetBool("Death", true);
				else changeState(0);
			}
		}
	}

	void DoneInvincible(){
		invincible = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		state -= 3f;
		UpdateAnimator();
		mainCamera.audio.PlayOneShot(gameMusic);
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
			headCollider.center = new Vector2(0f, 0.95f);
			headCollider.size = new Vector2(0.1f, 0.1f);
			triggerCollider.center = new Vector2(0f, 0.55f);
			triggerCollider.size = new Vector2(0.75f, 0.9f);
		}
		else if(state == 1){
			anim.runtimeAnimatorController = largeController;
			headCollider.center = new Vector2(0f, 1.95f);
			headCollider.size = new Vector2(0.1f, 0.1f);
			triggerCollider.center = new Vector2(0f, 1.05f);
			triggerCollider.size = new Vector2(1f, 1.9f);
		}
		else{
			anim.runtimeAnimatorController = fireController;
			headCollider.center = new Vector2(0f, 1.95f);
			headCollider.size = new Vector2(0.1f, 0.1f);
			triggerCollider.center = new Vector2(0f, 1.05f);
			triggerCollider.size = new Vector2(1f, 1.9f);
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
		else if(newState == state+3f ||
		        state == 3 && newState == 4 ||
		        state == 4 && newState == 5)
		{
			state = newState;
			stateChange = true;
		}
	}
	
	public float getState(){
		return state;
	}
	
	public void addLife(){
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

	public void SpeedMusic(){
		mainCamera.GetComponent<AudioSource> ().pitch = 1.15f;
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