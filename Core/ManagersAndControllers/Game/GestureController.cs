using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GestureRecognizer))]
public class GestureController : DebuggableBaseClass
{
    private GestureRecognizer gestureRecognizer;
    private TrailController trailController;

    private bool specialSpellModeActive = false;

    public GestureRecognizer GestureRecognizer => gestureRecognizer;
    public bool IsSpecialSpellModeActive => specialSpellModeActive;
    public TrailController TrailController => trailController; 

    private void Awake() => trailController = transform.Find("TrailController").GetComponent<TrailController>();

    void Start() => gestureRecognizer = GetComponent<GestureRecognizer>();

    public void OnGestureRead(string recognizedSpellName)
    {
        if (recognizedSpellName == Settings.GestureErrorMessage)
            return;
        else
            InputManager.Instance.OnGestureRecognizedWithSpellName(recognizedSpellName);


        TrailController.Clear();
    }

    public void OnEndTouch(Vector2 position, float time, bool isSpecialSpellModeActive)
    {
        if (!isSpecialSpellModeActive)
            return;

        GestureRecognizer.OnEndTouch(position, time);
    }

    public void OnStartTouch(Vector2 position, float time, bool isSpecialSpellModeActive)
    {
        TrailController.ToggleTrail(true);
        if (!IsSpecialSpellModeActive)
            return;

        GestureRecognizer.OnStartTouch(position, time);
    }

    public void ToggleTrail(bool active) => TrailController.ToggleTrail(active);

    public void ToggleSpecialSpellMode()
    {
        TrailController.ToggleTrail(false);
        specialSpellModeActive = !specialSpellModeActive;
        PrintDebugLog($"Special spell mode toggled! Active: {specialSpellModeActive}");
    }

    public void ClearTrail() => TrailController.Clear();
}
