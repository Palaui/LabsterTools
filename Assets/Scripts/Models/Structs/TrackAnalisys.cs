using System.Collections.Generic;
using UnityEngine;

public struct TrackAnalisys
{
    private readonly TrackScriptable track;

    public readonly List<CircuitStep> steps;
    public readonly List<Vector3> stepPositions;

    public readonly Vector3 startPosition;
    public readonly Vector3 endPosition;
    public readonly Vector3 center;
    public readonly float size;

    private bool isComplete;


    public readonly bool IsComplete => isComplete;


    public TrackAnalisys(TrackScriptable track)
    {
        this.track = track;

        steps = new List<CircuitStep>();
        stepPositions = new List<Vector3>();

        startPosition = track.StartPieceModel.position;
        endPosition = track.EndPieceModel.position;
        center = (startPosition + endPosition) / 2;
        size = Vector3.Distance(startPosition, endPosition);

        isComplete = false;
        CalculateSteps();
    }


    private void CalculateSteps()
    {
        List<Vector3> positions = new List<Vector3> { startPosition };
        GetNeighbours(positions, startPosition);

        if (isComplete)
        {
            Vector3 lastDirection = Vector3.zero;
            for (int i = 0; i < stepPositions.Count - 1; i++)
            {
                Vector3 current = stepPositions[i];
                Vector3 next = stepPositions[i + 1];

                Vector3 direction = (next - current).normalized;

                if (i == 0)
                    steps.Add(CircuitStep.Rect);
                else if (direction == lastDirection)
                    steps.Add(CircuitStep.Rect);
                else
                    steps.Add(CircuitStep.Turn);

                lastDirection = direction;
            }
        }
    }

    private void GetNeighbours(List<Vector3> positions, Vector3 position)
    {
        Search(positions, position + Vector3.forward * EditorConstants.GRID_SIZE);
        Search(positions, position + Vector3.back * EditorConstants.GRID_SIZE);
        Search(positions, position + Vector3.left * EditorConstants.GRID_SIZE);
        Search(positions, position + Vector3.right * EditorConstants.GRID_SIZE);
    }

    private void Search(List<Vector3> positions, Vector3 search)
    {
        if (!isComplete && track.ContainsPieceAt(search) && !positions.Contains(search))
        {
            List<Vector3> list = new List<Vector3>(positions) { search };

            if (search == endPosition)
            {
                stepPositions.AddRange(list);
                isComplete = true;
            }
            else
                GetNeighbours(list, search);
        }
    }
}