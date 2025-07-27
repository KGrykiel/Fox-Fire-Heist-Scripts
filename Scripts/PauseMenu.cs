using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public GameObject StartMenuUI;
    public GameObject gameUI;
    public GameObject typingCanvas; // Reference to the TypingCanvas
    public GameObject endingTextCanvas; // Reference to the FadeCanvas
    public GameObject endingCanvas;
    public Image fadeImage; // Reference to the Image component on the FadeCanvas
    public Sprite customCursorSprite; // Reference to the custom cursor sprite
    public Vector2 cursorHotspot = Vector2.zero; // Hotspot for the custom cursor
    public TypingEffect typingEffect; // Reference to the TypingEffect script
    public TypingEffect typingEffect2; // Reference to the TypingEffect script

    public static bool isPaused = false; // Global pause flag
    public static bool isStart = true;
    private bool isTyping = false;
    private Texture2D customCursorTexture;

    void Start()
    {
        // Hide the pause menu at the start
        pauseMenuUI.SetActive(false);
        StartMenuUI.SetActive(true);
        gameUI.SetActive(false);
        typingCanvas.SetActive(false);
        endingTextCanvas.SetActive(false);
        endingCanvas.SetActive(false);

        // Convert the sprite to a texture
        if (customCursorSprite != null)
        {
            customCursorTexture = SpriteToTexture2D(customCursorSprite);
        }
        Time.timeScale = 0f; // Pause the game
        isStart = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(customCursorTexture, cursorHotspot, CursorMode.Auto); // Set custom cursor

        // Subscribe to the typing complete event
        typingEffect.onTypingComplete.AddListener(OnTypingComplete);
    }

    void Update()
    {
        // Check for the pause input (e.g., Escape key)
        if (Input.GetKeyDown(KeyCode.Escape) && !isStart && !isTyping)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Revert to default cursor
    }

    public void begin()
    {
        print("begin");
        isStart = false;
        StartMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        typingCanvas.SetActive(true);

        // Start the typing effect
        typingEffect.StartTyping();
        isTyping = true;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(customCursorTexture, cursorHotspot, CursorMode.Auto); // Set custom cursor
    }

    public void Quit()
    {
        // Add your quit logic here (e.g., load the main menu, quit the application)
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void OnTypingComplete()
    {
        // Handle the completion of the typing effect
        isTyping = false;
        gameUI.SetActive(true);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Revert to default cursor
        typingCanvas.SetActive(false);
    }

    public void StartNewSequence()
    {
        StartCoroutine(NewSequenceCoroutine());
    }

    private IEnumerator NewSequenceCoroutine()
    {
        yield return new WaitForSeconds(12f); // Wait for a couple of seconds

        endingTextCanvas.SetActive(true);
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        // Fade to black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        isPaused = true;
        typingEffect2.StartTyping();
        isTyping = true;
    }
    public void OnEndTypingComplete()
    {
        // Handle the completion of the typing effect
        isTyping = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Revert to default cursor
        endingTextCanvas.SetActive(false);
        endingCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(customCursorTexture, cursorHotspot, CursorMode.Auto); // Set custom cursor
    }

    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
        {
            Texture2D newText = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);
            newText.SetPixels(sprite.texture.GetPixels());
            newText.Apply();
            return newText;
        }
    }
}
