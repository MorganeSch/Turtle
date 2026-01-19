using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InstructionEvent : UnityEvent<string, List<string>> { }

public class TurtleBase : MonoBehaviour
{
    struct State
    {
        public bool penDown;
        public Color color;
        public BetterLineRenderer2D lineRenderer;
    }

    enum InstructionKind
    {
        Turn,
        Advance,
        Color,
        PenUp,
        PenDown,
    }

    struct Instruction
    {
        public InstructionKind instructionKind;
        public float amount;
        public Color color;

        public static Instruction Turn(float degrees)
        {
            return new Instruction()
            {
                instructionKind = InstructionKind.Turn,
                amount = degrees
            };
        }

        public static Instruction Advance(float distance)
        {
            return new Instruction()
            {
                instructionKind = InstructionKind.Advance,
                amount = distance
            };
        }

        public static Instruction Color(Color color)
        {
            return new Instruction()
            {
                instructionKind = InstructionKind.Color,
                color = color,
            };
        }

        public static Instruction PenUp()
        {
            return new Instruction()
            {
                instructionKind = InstructionKind.PenUp,
            };
        }

        public static Instruction PenDown()
        {
            return new Instruction()
            {
                instructionKind = InstructionKind.PenDown,
            };
        }

        public override string ToString()
        {
            switch (this.instructionKind)
            {
                case InstructionKind.Turn:
                    return $"Turn {this.amount} degrees";
                case InstructionKind.Advance:
                    return $"Advance {this.amount} units";
                case InstructionKind.Color:
                    return $"Change color to #{this.color.ToHexString()}";
                case InstructionKind.PenUp:
                    return "Pen Up";
                case InstructionKind.PenDown:
                    return "Pen Down";
                default:
                    return "ERROR";
            }
        }

    }

    List<BetterLineRenderer2D> lineRenderers;
    public Material material;
    Queue<Instruction> instructions;
    State state;
    bool executing = false;

    public float stepTime = 0.5f;
    float currentStepTime;
    [Range(0, 1)]
    public float stepTimeMultiplier = 0.95f;
    public float lineWidth = 0.2f;


    public UnityEvent onProgramStarted;
    public InstructionEvent onInstructionPopped;
    public UnityEvent onProgramFinished;

    // Start is called before the first frame update
    void Awake()
    {
        instructions = new Queue<Instruction>();
        lineRenderers = new List<BetterLineRenderer2D>();
        // material = new Material(Shader.Find("Particles/Standard Unlit"));
        // material.color = Color.white;
        currentStepTime = stepTime;
        state = new State()
        {
            penDown = false,
            color = Color.white,
        };
    }

    BetterLineRenderer2D StartLine(Color color)
    {
        state.penDown = true;
        state.color = color;
        GameObject line = new GameObject($"Line #{lineRenderers.Count}");
        line.transform.parent = this.transform;
        line.transform.localPosition = Vector3.zero;
        line.transform.localRotation = Quaternion.identity;

        BetterLineRenderer2D lineRenderer = line.AddComponent<BetterLineRenderer2D>();
        lineRenderer.baseMaterial = material;
        lineRenderer.color = color;
        lineRenderer.width = lineWidth;
        lineRenderer.useWorldSpace = true;
        lineRenderer.points.Add(transform.position);
        lineRenderers.Add(lineRenderer);
        state.lineRenderer = lineRenderer;
        return lineRenderer;
    }

    public void Turn(float degrees)
    {

        instructions.Enqueue(Instruction.Turn(degrees));
        if (!executing)
        {
            StartCoroutine(Execute());
        }
    }

    public void Advance(float distance)
    {

        instructions.Enqueue(Instruction.Advance(distance));
        if (!executing)
        {
            StartCoroutine(Execute());
        }
    }

    public void ChangeColor(Color color)
    {

        instructions.Enqueue(Instruction.Color(color));
        if (!executing)
        {
            StartCoroutine(Execute());
        }
    }

    public void PenUp()
    {

        instructions.Enqueue(Instruction.PenUp());
        if (!executing)
        {
            StartCoroutine(Execute());
        }
    }

    public void PenDown()
    {

        instructions.Enqueue(Instruction.PenDown());
        if (!executing)
        {
            StartCoroutine(Execute());
        }
    }

    void EndLine()
    {
        state.penDown = false;
    }

    IEnumerator Execute()
    {
        if (executing)
        {
            yield return null;
        }
        else
        {
            executing = true;
            onProgramStarted.Invoke();
            currentStepTime = stepTime;
            yield return new WaitForEndOfFrame();
            while (instructions.Count > 0)
            {
                Instruction instruction = instructions.Dequeue();
                onInstructionPopped.Invoke($"{instruction}", instructions.Select(instruction => $"{instruction}").ToList());
                switch (instruction.instructionKind)
                {
                    case InstructionKind.Turn:
                        yield return ExecuteTurn(instruction.amount);
                        break;
                    case InstructionKind.Advance:
                        yield return ExecuteAdvance(instruction.amount);
                        break;
                    case InstructionKind.Color:
                        yield return ExecuteColor(instruction.color);
                        break;
                    case InstructionKind.PenUp:
                        yield return ExecutePen(false);
                        break;
                    case InstructionKind.PenDown:
                        yield return ExecutePen(true);
                        break;
                }
                currentStepTime *= stepTimeMultiplier;
            }
            executing = false;
            onProgramFinished.Invoke();
        }
    }

    IEnumerator ExecuteTurn(float degrees)
    {
        float startTime = Time.time;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.AngleAxis(-degrees, Vector3.forward) * startRotation;
        while (startTime + currentStepTime > Time.time)
        {
            float t = (Time.time - startTime) / currentStepTime;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = endRotation;
    }

    IEnumerator ExecuteAdvance(float distance)
    {
        float startTime = Time.time;
        if (state.penDown)
        {
            state.lineRenderer.points.Add(transform.position);
        }
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + (transform.up * distance);
        while (startTime + currentStepTime > Time.time)
        {
            float t = (Time.time - startTime) / currentStepTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            if (state.penDown)
            {
                state.lineRenderer.points[state.lineRenderer.points.Count - 1] = transform.position;
            }
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPosition;
        if (state.penDown)
        {
            state.lineRenderer.points[state.lineRenderer.points.Count - 1] = transform.position;
        }
    }

    IEnumerator ExecuteColor(Color color)
    {
        if (state.penDown)
        {
            StartLine(color);
        }
        else
        {
            state.color = color;
        }
        yield return new WaitForSeconds(currentStepTime);
    }

    IEnumerator ExecutePen(bool down)
    {
        if (down)
        {
            StartLine(state.color);
        }
        else
        {
            EndLine();
        }
        yield return new WaitForSeconds(currentStepTime);
    }
}
