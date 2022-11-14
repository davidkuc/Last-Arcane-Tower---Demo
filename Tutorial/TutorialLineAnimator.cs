using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

[MovedFrom("TutorialLineAnimator")]
public class TutorialLineAnimator : DebuggableBaseClass
{
    enum TutorialAnimationTypes
    {
        SwipeAnimationLoop,
        ButtonAnimationLoop,
        spellSeperationLineLoop
    }

    [SerializeField] private TutorialAnimationTypes tutorialAnimationType;
    [SerializeField] private float animationDuration = 5f;
    [SerializeField] private float scalingDuration = 5f;
    [Space]
    [Header("Easing curves")]
    [SerializeField, FormerlySerializedAs("startAnimationCurve")] AnimationCurve startAnimationCurve;
    [SerializeField, FormerlySerializedAs("endAnimationCurve")] AnimationCurve endAnimationCurve;
    [SerializeField, FormerlySerializedAs("scaleCurve")] AnimationCurve scaleCurve;

    private LineRenderer lineRenderer;
    private Vector3[] linePoints;
    private int pointsCount;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Store a copy of lineRenderer's points in linePoints array
        pointsCount = lineRenderer.positionCount;
        linePoints = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            linePoints[i] = lineRenderer.GetPosition(i);
        }

        switch (tutorialAnimationType)
        {
            case TutorialAnimationTypes.SwipeAnimationLoop:
                StartCoroutine(AnimateSwipeAnimationLoop());
                break;
            case TutorialAnimationTypes.ButtonAnimationLoop:
                break;
            default:
                break;
        }
    }

    private IEnumerator AnimateSwipeAnimationLoop()
    {
        float segmentDuration = animationDuration / pointsCount;

        while (true)
        {

            for (int i = 0; i < pointsCount - 1; i++)
            {
                lineRenderer.enabled = true;

                float startTime = Time.time;

                Vector3 startPosition = linePoints[i];
                Vector3 endPosition = linePoints[i + 1];

                Vector3 pos = startPosition;

                while (pos != endPosition)
                {
                    float t = (Time.time - startTime) / segmentDuration;
                    var easedTime = startAnimationCurve.Evaluate(t);
                    pos = Vector3.Lerp(startPosition, endPosition, easedTime);

                    // animate all other points except point at index i
                    for (int j = i + 1; j < pointsCount; j++)
                        lineRenderer.SetPosition(j, pos);

                    PrintDebugLog("first loop");
                    yield return null;
                }

                startTime = Time.time;

                while (lineRenderer.GetPosition(0) != endPosition)
                {
                    float t = (Time.time - startTime) / segmentDuration;
                    var easedTime = endAnimationCurve.Evaluate(t);
                    pos = Vector3.Lerp(startPosition, endPosition, easedTime);

                    for (int j = 0; j < pointsCount - 1; j++)
                        lineRenderer.SetPosition(j, pos);

                    PrintDebugLog("second loop");
                    yield return null;
                }

                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.enabled = false;

                yield return new WaitForSecondsRealtime(0.3f);
            }
        }
    }
}
