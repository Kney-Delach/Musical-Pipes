using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
	public Text nameText;
	public Text dialogueText;

    public CanvasGroup dialogueGroup; 

	private Queue<string> sentences;

    public bool isActive = false; 
    public bool deactivated = false; 
    public bool spawnObject = false;
    private static DialogueManager instance; 
    public static DialogueManager Instance { get { return instance ;} }
    private void Awake()
    {
         if (instance != null)
                Destroy(gameObject);
        else
            instance = this; 
    }

    private void Update()
    {
        if(isActive && Input.GetKeyDown(KeyCode.Return))
        {
            deactivated = true; 
            EndDialogue();
        }
    }
    private void OnDestroy()
    {
        if(instance == this)
            instance = null;
    }
	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
	}

	public void StartDialogue (Dialogue dialogue)
	{
        isActive = true;
        dialogueGroup.alpha = 1;

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
        spawnObject = true; 
        isActive = false; 
        //deactivated = false; 
        dialogueGroup.alpha = 0;
	}
}
