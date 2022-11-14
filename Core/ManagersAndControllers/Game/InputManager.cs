using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SwipeDetection))]
public class InputManager : Singleton<InputManager>, IPlayerLoader
{
    public event Action<Vector2, float, bool> StartTouch;
    public event Action<Vector2, float, bool> EndTouch;

    public event Action<Vector2> SwipeDown;
    public event Action<Vector2> SwipeUp;
    public event Action<Vector2> ProjectileSwipe;
    public event Action TouchHold;

    public event Action<SpellTypes, Vector2> SpellThrow;

    private GestureController gestureController;
    private PlayerActions playerActions;
    private PlayerManager player;
    private SwipeDetection swipeDetection;

    private float startTouchTime;
    private bool isLeftMouseButtonBeingHold;

    public bool IsSpecialSpellModeActive => gestureController.IsSpecialSpellModeActive;
    public Vector2 SwipeDirection => swipeDetection.GetSwipeDirection();
    public bool IsPlayerDeadOrNotLoaded
    {
        get
        {
            if (player == null || player.IsPlayerDead)
                return true;
            else
                return false;
        }
    }

    private void Update()
    {
#if UNITY_WEBGL
        if (Input.GetMouseButton(0) && isLeftMouseButtonBeingHold != true)
        {
            var asd = new InputAction.CallbackContext();
            PrintDebugLog($"Start touch time: {asd.time}");
            OnStartTouch(new InputAction.CallbackContext() { });

            isLeftMouseButtonBeingHold = true;
        }

        if (Input.GetMouseButtonUp(0) && isLeftMouseButtonBeingHold == true)
        {
            var asd = new InputAction.CallbackContext();
            PrintDebugLog($"Endtouch time: {asd.time}");
            OnEndTouch(new InputAction.CallbackContext() { });


            isLeftMouseButtonBeingHold = false;
        }
    }
#endif


    protected override void Awake()
    {
        base.Awake();

        gestureController = transform.Find("GestureController").GetComponent<GestureController>();
        swipeDetection = GetComponent<SwipeDetection>();

#if UNITY_ANDROID
        playerActions = new PlayerActions();
        playerActions.Touch.TouchPress.started += ctx => OnStartTouch(ctx);
        playerActions.Touch.TouchPress.canceled += ctx => OnEndTouch(ctx);
        playerActions.Touch.TouchHold.performed += ctx => OnTouchHold(ctx);
#endif

        StartTouch += gestureController.OnStartTouch;
        EndTouch += gestureController.OnEndTouch;

        GameManager.Instance.GameSceneLoaded += ActivateTrail;
        GameManager.Instance.GameSceneUnloaded += DeactivateTrail;
        GameManager.Instance.GestureRecognizedWithSpellName += ToggleSpecialSpellMode;
    }


    private void DeactivateTrail() => gestureController.ToggleTrail(false);

    private void ActivateTrail() => gestureController.ToggleTrail(true);

#if UNITY_ANDROID
    private void OnEnable() => playerActions.Enable();

    private void OnDisable() => playerActions.Disable();
#endif

    public void ToggleSpecialSpellMode(string recognizedSpellName) => gestureController.ToggleSpecialSpellMode();

    public void OnGestureRecognizedWithSpellName(string recognizedSpellName)
    {
        GameManager.Instance.InvokeGestureRecognizedWithSpellName(recognizedSpellName);
        SpellThrow?.Invoke(SpellTypes.Special, Vector2.zero);
    }

    public Vector2 PrimaryPosition()
    {
        Vector2 primaryPosition;

#if UNITY_WEBGL
        primaryPosition = ScreenToWorld(GameManager.Instance.MainCamera, Input.mousePosition);
#else
        primaryPosition = ScreenToWorld(GameManager.Instance.MainCamera, Touchscreen.current.primaryTouch.position.ReadValue());
#endif
        return primaryPosition;
    }

    public void LoadPlayer() => player = PlayerManager.Instance;

    public void OnTouchHold(InputAction.CallbackContext context)
    {
        if (UI_Manager.Instance.Is_UI_Active || gestureController.IsSpecialSpellModeActive || IsPlayerDeadOrNotLoaded)
            return;

        var position = ScreenToWorld(GameManager.Instance.MainCamera, Touchscreen.current.primaryTouch.position.ReadValue());

        TouchHold?.Invoke();
    }

    public void OnEndTouch(InputAction.CallbackContext context)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (TouchIsOverUI())
            return;

        Vector3 worldPosition;

#if UNITY_WEBGL
        worldPosition = ScreenToWorld(GameManager.Instance.MainCamera, Input.mousePosition);
#else
        worldPosition = ScreenToWorld(GameManager.Instance.MainCamera, Touchscreen.current.primaryTouch.position.ReadValue());
#endif

        EndTouch?.Invoke(worldPosition, (float)context.time, IsSpecialSpellModeActive);
    }

    public void OnStartTouch(InputAction.CallbackContext context)
    {
        //if (UI_Manager.Instance.Is_UI_Active)
        //gestureController.ClearTrail();

        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (TouchIsOverUI())
            return;

        startTouchTime = (float)context.time;

        StartCoroutine(SkipFirstTouchPosition());
    }

    public void OnSwipeUp(Vector2 position)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsSpecialSpellModeActive || IsPlayerDeadOrNotLoaded)
            return;

        SwipeUp?.Invoke(position);
        SpellThrow?.Invoke(SpellTypes.Wall, position);
    }

    public void OnSwipeDown(Vector2 position)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsSpecialSpellModeActive || IsPlayerDeadOrNotLoaded)
            return;

        SwipeDown?.Invoke(position);
        SpellThrow?.Invoke(SpellTypes.Skydrop, position);
    }

    public void OnProjectileSwipe(Vector2 position)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsSpecialSpellModeActive || IsPlayerDeadOrNotLoaded)
            return;

        ProjectileSwipe?.Invoke(position);
        SpellThrow?.Invoke(SpellTypes.Projectile, position);
    }

    public void ToggleGamePause() => gestureController.ToggleTrail(!gestureController.TrailController.IsTrailActive);

    private bool TouchIsOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

#if UNITY_WEBGL
        eventDataCurrentPosition.position = Input.mousePosition;
#else
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        UI_Manager.Instance.GameSceneUIGraphicRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private IEnumerator SkipFirstTouchPosition()
    {
        yield return null;

        Vector2 worldPosition;

#if UNITY_WEBGL
        worldPosition = ScreenToWorld(GameManager.Instance.MainCamera, /*Input.touches[0].position*/Input.mousePosition);
#else
        worldPosition = ScreenToWorld(GameManager.Instance.MainCamera, Touchscreen.current.primaryTouch.position.ReadValue());
#endif
        StartTouch?.Invoke(worldPosition, startTouchTime, IsSpecialSpellModeActive);
    }

    public static Vector2 ScreenToWorld(Camera camera, Vector3 position) => camera.ScreenToWorldPoint(position);
}
