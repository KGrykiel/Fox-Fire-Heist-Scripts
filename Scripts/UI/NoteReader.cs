using UnityEngine;
using TMPro;

public class NoteReader : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 3f; // Range within which the player can interact with the note
    public KeyCode interactionKey = KeyCode.Mouse0; // Key to press for interaction
    public GameObject noteCanvas; // Reference to the canvas that displays the note
    public TextMeshProUGUI noteText; // Reference to the TextMeshProUGUI component on the canvas

    private bool isNoteDisplayed = false; // Flag to track if the note is currently displayed
    private Transform currentNoteTransform; // Reference to the currently displayed note's transform

    private void Start()
    {
        noteCanvas.SetActive(false);
    }

    private void Update()
    {
        if (PauseMenu.isPaused || PauseMenu.isStart)
        {
            return;
        }
        // Check if the player presses the interaction key
        if (Input.GetKeyDown(interactionKey))
        {
            OnButtonEnter();
        }

        // Hide the canvas when the player presses the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }

        // Check if the player moves beyond the interaction range
        if (isNoteDisplayed && currentNoteTransform != null)
        {
            float distance = Vector3.Distance(playerCamera.transform.position, currentNoteTransform.position);
            if (distance > interactionRange)
            {
                CloseNote();
            }
        }
    }

    private void OnButtonEnter()
    {
        // Hide the canvas if it is already displayed
        if (isNoteDisplayed)
        {
            CloseNote();
            return;
        }

        // Check if the player is looking at a note
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            // Check if the object has a Note component
            Note note = hit.collider.GetComponent<Note>();
            if (note != null)
            {
                print("Reading note: " + note.text);
                // Display the note text on the canvas
                noteText.text = note.text;
                noteCanvas.SetActive(true);
                isNoteDisplayed = true;
                currentNoteTransform = note.transform;
            }
        }
    }

    private void CloseNote()
    {
        noteCanvas.SetActive(false);
        isNoteDisplayed = false;
        currentNoteTransform = null;
    }
}
