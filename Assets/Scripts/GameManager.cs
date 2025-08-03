using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 0;
    public Slider healthBar;
    public GameObject turtleCrawling;
    public GameObject turtleWalking;
    public GameObject cinemachineCameraPrefab;
    public GameObject powerUpUIPrefab;
    public PowerUpUI powerUpUI;
    public Image fillImage;
    public GameObject caveEntranceIce;
    public GameObject caveEntranceMagma;
    public int numLives = 0;

    [SerializeField]
    public LevelSpawnPoints[] levelSpawnPoints;
    public float fadeDuration = 1f;
    public float startMenuDisableDelay = 2f;
    public CanvasGroup fadeCanvas;
    public bool gameOver;
    public GameObject skipText;

    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float turtleHealth = 10f;

    public GameObject finalCamFollowTargetPrefab;
    public Transform finalCameraStartPoint;
    public Transform finalCameraEndPoint;
    public Transform finalCameraStaticPoint;
    public float finalCamMoveSpeed = 2f;
    public GameObject leaderboard;
    public GameObject canvas;
    public bool canDestroy = false;

    private float runOutDistance = 4f; // distance in # of tiles the turtle runs out of cave on its own
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject turtleToSpawn;
    void Start()
    {
        canvas.SetActive(false);
        fillImage = healthBar.fillRect.GetComponent<Image>();
        Sound bgm = AudioManager.Instance.Play("Level1_BGM");
        if (!bgm.source.isPlaying)
        {
            bgm.source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pauseGame()
    {
        Time.timeScale = 0f; // pause the game
    }

    public void resumeGame()
    {
        Time.timeScale = 1f; // resume the game
    }

    public void initiateNextTurtleLife()
    {
        // get the spawn points for the current level and starts the turtle instantiation coroutine
        Transform turtleSpawn = levelSpawnPoints[currentLevel].turtleSpawnPoint;
        Transform camSpawn = levelSpawnPoints[currentLevel].camSpawnPoint;
        StartCoroutine(instantiateTurtle(turtleSpawn, camSpawn));
    }
    public IEnumerator instantiateTurtle(Transform turtleSpawnPoint, Transform camSpawnPoint)
    {
        // instantiate the turtle and camera at the specified spawn points and get references to their components
        Debug.Log("instantiateTurtle called");
        /*
        if (moveSpeed > 4f)
        {
            turtleToSpawn = turtleWalking;  // if the move speed is greater than 4, use the walking turtle prefab
        } 
        else
        {
            turtleToSpawn = turtleCrawling; // if the move speed is less than 4, use the crawling turtle prefab
        }
        */
        turtleToSpawn = turtleCrawling;
        GameObject turtle = Instantiate(turtleToSpawn, turtleSpawnPoint.position, Quaternion.identity);
        GameObject cameraObject = Instantiate(cinemachineCameraPrefab.gameObject, camSpawnPoint.position, Quaternion.identity);
        Turtle_Pickup turtlePickup = turtle.GetComponent<Turtle_Pickup>();
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        CinemachineCamera camera = cameraObject.GetComponent<CinemachineCamera>();
        Animator anim = turtle.GetComponent<Animator>();

        // set the camera's target to the turtle and disable player controls for a short time
        camera.Target.TrackingTarget = turtle.transform;
        turtleController.controlsEnabled = false; //disable player controls

        // set the turtle's camera and game manager references
        turtleController.camera = cameraObject;
        turtleController.gameManager = this;
        turtlePickup.powerUpUI = powerUpUIPrefab;
        
        // set the player reference in the powerUpUI script
        powerUpUI.setPlayer(turtle);
        // wait for a short time to allow the camera to focus on the turtle
        yield return new WaitForSeconds(2f);

        
        powerUpUIPrefab.SetActive(true);
        turtleController.healthBar = healthBar;
        if (healthBar != null) // fill health bar bc it looks nice
        {
            healthBar.maxValue = turtleHealth;
            healthBar.value = turtleHealth;
        }
        while (powerUpUIPrefab.activeSelf )
        {
            yield return null; // wait until the powerUpUI is turned off
        }

        // set the turtle's properties
        turtleController.moveSpeed = moveSpeed;
        turtleController.jumpForce = jumpForce;
        turtleController.lifetime = turtleHealth;
        

        if (healthBar != null) // fill health bar again bc it might have been updated
        {
            healthBar.maxValue = turtleHealth;
            healthBar.value = turtleHealth;
        }

        // numLives must be increased AFTER the ui is turned on and off so the powerups reset to 0 count if the turtle was on its 0ith life
        numLives++;


        // make the turtle run out of the cave a set distance (this is SUPER jank but it works). It is intended behavior that the player can prematurely end
        // the turtle's run out by pressing a movement key
        float time = runOutDistance / moveSpeed;
        bool breaked = false;
        turtle.transform.localScale = new Vector3(-1, 1, 1); // Facing right
        anim.SetBool("forceWalk", true);
        while (time > 0)
        {
            float currentVelX = moveSpeed;
            turtleController.rb.linearVelocity = new Vector2(currentVelX, 0f);
            time -= Time.deltaTime;
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                turtleController.controlsEnabled = true; // enable player controls
                anim.SetBool("forceWalk", false);
                turtleController.moveInput = Input.GetAxisRaw("Horizontal");
                breaked = true;
                break;
            }
            yield return null;
        }
        if (!breaked)
        {
            anim.SetBool("forceWalk", false);
            turtleController.rb.linearVelocity = new Vector2(0f, 0f);
            turtleController.controlsEnabled = true;
        }
    }

    public void nextLevel(){
        currentLevel++;
        StartCoroutine(LoadNextLevel());
    }

    public IEnumerator LoadNextLevel()
    {
        canvas.SetActive(true);
        if(currentLevel == 1){
            AudioManager.Instance.fadeOutHelper("Level1_BGM", 0);
            StartCoroutine(AudioManager.Instance.playWithFadeIn("Level2_BGM"));
            yield return StartCoroutine(FadeCanvas(0f, 2f));
            StartCoroutine(AudioManager.Instance.playNarration("Magma Cave Narration"));
            fillImage.color = new Color(0.6f, 0.8f, 1f, 1f);
            caveEntranceIce.SetActive(true);
            yield return StartCoroutine(FadeCanvas(2f, 0f));
        }else if(currentLevel == 2){
            AudioManager.Instance.fadeOutHelper("Level2_BGM", 0);
            StartCoroutine(AudioManager.Instance.playWithFadeIn("Level3_BGM"));
            StartCoroutine(AudioManager.Instance.playNarration("Frozen Cave Narration"));
            yield return StartCoroutine(FadeCanvas(0f, 2f));
            fillImage.color = new Color(1f, 0f, 0f, 1f);
            caveEntranceMagma.SetActive(true);
            yield return StartCoroutine(FadeCanvas(2f, 0f));
        }
        canvas.SetActive(false);
        initiateNextTurtleLife();
    }

    IEnumerator FadeCanvas(float from, float to)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, time / fadeDuration);
            fadeCanvas.alpha = alpha;
            yield return null;
        }
        fadeCanvas.alpha = to;
        yield break;
    }

    public IEnumerator EndLevelSequence(GameObject turtle, bool left)
    {
        skipText.SetActive(true);
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        Animator anim = turtle.GetComponent<Animator>();
        
        turtleController.controlsEnabled = false;
        
        if(left){
            turtle.transform.localScale = new Vector3(1, 1, 1);
        }else{
            turtle.transform.localScale = new Vector3(-1, 1, 1);
        }
        anim.SetBool("forceWalk", true);

        float walkTime = 0.5f;
        float timer = 0f;

        while (timer < walkTime)
        {
            if(left){
                turtleController.rb.linearVelocity = new Vector2(-moveSpeed, 0f);
            }else{
                turtleController.rb.linearVelocity = new Vector2(moveSpeed, 0f);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        turtleController.rb.linearVelocity = Vector2.zero;
        anim.SetBool("forceWalk", false);
        StartCoroutine(FadeSpriteOut(turtle.GetComponent<SpriteRenderer>(), 0.5f));
        Destroy(turtle);
        nextLevel();
    }

    public IEnumerator FadeSpriteOut(SpriteRenderer sprite, float duration)
    {
        Color originalColor = sprite.color;
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // fully transparent
    }

    public IEnumerator FinalSequence(GameObject turtle, bool left)
    {
        canvas.SetActive(true);
        skipText.SetActive(true);
        TurtleController turtleController = turtle.GetComponent<TurtleController>();
        Animator anim = turtle.GetComponent<Animator>();

        turtleController.controlsEnabled = false;
        turtleController.rb.linearVelocity = Vector2.zero;

        turtle.transform.localScale = left ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        anim.SetBool("forceWalk", true);

        float walkTime = 0.5f;
        float timer = 0f;

        while (timer < walkTime)
        {
            turtleController.rb.linearVelocity = left ? new Vector2(-moveSpeed, 0f) : new Vector2(moveSpeed, 0f);
            timer += Time.deltaTime;
            yield return null;
        }

        turtleController.rb.linearVelocity = Vector2.zero;
        anim.SetBool("forceWalk", false);

        yield return StartCoroutine(FadeSpriteOut(turtle.GetComponent<SpriteRenderer>(), 0.5f));
        Destroy(turtle);

        GameObject camObj = turtleController.camera;
        CinemachineCamera vcam = camObj.GetComponent<CinemachineCamera>();

        GameObject followTarget = Instantiate(finalCamFollowTargetPrefab, finalCameraStartPoint.position, Quaternion.identity);
        yield return StartCoroutine(FadeCanvas(0f, 1f));
        vcam.Target.TrackingTarget = followTarget.transform;
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(FadeCanvas(1f, 0f));

        Vector3 startPos = finalCameraStartPoint.position;
        Vector3 endPos = finalCameraEndPoint.position;
        float moveTime = 20f;
        float elapsed = 0f;

        while (elapsed < moveTime)
        {
            if (Input.GetKey(KeyCode.E)){
                break;
            };
            followTarget.transform.position = Vector3.Lerp(startPos, endPos, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPos = finalCameraStaticPoint.position;
        float finalMoveTime = 0.1f;
        float finalElapsed = 0f;

        yield return StartCoroutine(FadeCanvas(0f, 1f));
        skipText.SetActive(false);
        followTarget.transform.position = finalPos;
        yield return new WaitForSecondsRealtime(1f);
        leaderboard.SetActive(true);
        canvas.SetActive(false);
        yield return StartCoroutine(FadeCanvas(1f, 0f));
        gameOver = true;
    }

}
