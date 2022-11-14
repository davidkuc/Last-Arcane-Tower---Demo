
using System;
using UnityEngine;

[DefaultExecutionOrder(2)]
[RequireComponent(typeof(InputManager))]
public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minimumDistance = 0.2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0, 1)] private float directionThreshold = 0.9f;

    private SwipeDetectionLogic swipeDetectionLogic;
    private InputManager inputManager;

    public Vector2 GetSwipeDirection() => swipeDetectionLogic.GetSwipeDirection();

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        swipeDetectionLogic = new SwipeDetectionLogic(directionThreshold, maximumTime, minimumDistance,
            inputManager.OnSwipeDown, inputManager.OnSwipeUp, inputManager.OnProjectileSwipe);
    }

    private void OnEnable()
    {
        inputManager.StartTouch += SwipeStart;
        inputManager.EndTouch   += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.StartTouch -= SwipeStart;
        inputManager.EndTouch -= SwipeEnd;
    }

    private void SwipeEnd(Vector2 position, float time, bool isSpecialSpellModeActive)
    {
        if (isSpecialSpellModeActive)
            return;

        swipeDetectionLogic.SwipeEnd(position, time);
    }

    private void SwipeStart(Vector2 position, float time, bool isSpecialSpellModeActive)
    {
        if (isSpecialSpellModeActive)
            return;

        swipeDetectionLogic.SwipeStart(position, time);
    }
}
