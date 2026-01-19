using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurtleLogger : MonoBehaviour
{
    public TextMeshProUGUI currentInstructionPrefab;
    public TextMeshProUGUI nextInstructionPrefab;

    TextMeshProUGUI currentInstruction;
    List<TextMeshProUGUI> nextInstructions;

    public void OnTurtleProgramStarted()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        currentInstruction = null;
        nextInstructions = new List<TextMeshProUGUI>();
    }

    public void OnTurtleInstruction(string current, List<string> next)
    {
        if (currentInstruction == null)
        {
            currentInstruction = Instantiate(currentInstructionPrefab, this.transform);
            currentInstruction.transform.SetSiblingIndex(0);
        }
        while (Mathf.Min(next.Count, 30) > nextInstructions.Count)
        {
            nextInstructions.Add(Instantiate(nextInstructionPrefab, this.transform));
        }
        currentInstruction.text = current;
        for (int i = 0; i < nextInstructions.Count; i++)
        {
            if (i < next.Count)
            {
                nextInstructions[i].text = next[i];
            }
            else
            {
                nextInstructions[i].text = "";
            }
            nextInstructions[i].transform.SetSiblingIndex(i + 1);
        }
    }

    public void OnTurtleProgramEnded()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        currentInstruction = null;
        nextInstructions = new List<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Awake()
    {
        nextInstructions = new List<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
