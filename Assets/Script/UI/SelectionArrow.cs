using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound; // The sound we play when we move the arrow UP || DOWN
    [SerializeField] private AudioClip interactSound; // sound upon selection
    private RectTransform rect;

    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Move Selection Arrow UP || DOWN
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }

        // Interacting with options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
        {
            SoundManager.instance.PlaySound(changeSound);
        }


        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if(currentPosition > options.Length-1)
        {
            currentPosition = 0;
        }

        // assigning Y position of current iption to arrow ( Moving UP || DOWN )
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);

        // Accessing Button Component of Options (OnClick)
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
