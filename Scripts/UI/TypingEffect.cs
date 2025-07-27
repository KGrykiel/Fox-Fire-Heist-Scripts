using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI typingText; // Reference to the Text component
    public float typingSpeed = 0.05f; // Speed of typing
    private string fullText = ""; // The full text to display
    public int textIndex = 0; // Index of the current paragraph

    private bool isTyping = false;
    private string startText = " The world is powered by steam and the skies are filled with brass airships. Technology and culture are at its peak, but everything is about to change. \n\n A wealthy magnate named Ignatius Fox is currently running tests of his newest invention, a miracle power source nicknamed \"Fox-Fire\" that could revolutionise the world. His idea of a revolution however seems to favour only the highest bidder. \n\n You are currently in a box smuggled into the cargo hold of his flagship with one goal in mind: take the Fox-Fire for yourself and give it to those that need it most. \n\n Press [LMB] to turn on the light."; // The full text to display
    private string endText = " You grab the orb and make a daring escape. You rush back to the main room, shatter a window and jump out before anyone stationed on the airship can stop you. It's a long way down but your parachute is tried and tested. \n\n The Fox-Fire is now in your hands, oozing with energy. You don't know what terrific impact it will have on civilization, but there is one thing you do: It's better off with you. \n\n Press [LMB] to continue."; // The full text to display

    // Define the event
    public UnityEvent onTypingComplete = new UnityEvent();

    void Start()
    {
        typingText.text = ""; // Clear the text at the start

    }

    public void StartTyping()
    {
        if (textIndex == 0)
        {
            fullText = startText;
            print(fullText);
        }
        else if (textIndex == 1)
        {
            fullText = endText;
        }
        if (!isTyping)
        {
            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        foreach (char letter in fullText.ToCharArray())
        {
            typingText.text += letter;
            if (letter == '.' || letter == ',' || letter == ':')
            {
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        isTyping = false;
    }

    void Update()
    {
        if (isTyping && Input.anyKeyDown)
        {
            StopAllCoroutines();
            typingText.text = fullText;
            isTyping = false;
        }
        else if (!isTyping && Input.GetKeyDown(KeyCode.Mouse0))
        {
            onTypingComplete.Invoke(); // Invoke the event if typing is interrupted
        }
    }
}
