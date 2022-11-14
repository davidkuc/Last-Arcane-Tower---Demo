using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(HPMPControl))]
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class PlayerManager : Singleton<PlayerManager>
{
    public event Action PlayerDeath;

    [Header("Spells")]
    [SerializeField] private int specialSpellCooldown = 10;
    [Space]
    [SerializeField] private Player_SO player_SO;
    [SerializeField] private PlayerSprites_SO playerSprites_SO;

    private SpellController spellController;
    private SpriteRenderer towerSprite;
    private Transform spellSource;
    private Transform wallSpellSpawnPoint;
    private Transform spellSeperationLine;
    private SpellCooldownLogic specialSpellCooldownLogic;

    HPMPControl hPMPControl;
    private float currentHP;
    private float maxHP;
    private bool playerDead;

    public Player_SO Player_SO => player_SO;
    public bool IsPlayerDead => playerDead;
    public bool SpecialSpellModeActive => InputManager.Instance.IsSpecialSpellModeActive;
    public Transform SpellSource => spellSource;
    public Transform WallSpellSpawnPoint => wallSpellSpawnPoint;
    public Transform SpellSeperationLine => spellSeperationLine;
    public bool IsSpecialSpellCooldownActive => specialSpellCooldownLogic.CooldownActive;

    protected override void Awake()
    {
        base.Awake();
        player_SO = ResourceLoader.LoadPlayer_SO();
        playerSprites_SO = ResourceLoader.LoadPlayerSprites_SO();
        spellSource = transform.Find("SpellSource");
        wallSpellSpawnPoint = transform.Find("WallSpellFloorPoint");
        spellSeperationLine = transform.Find("SpellSeperationLine");

        hPMPControl = GetComponent<HPMPControl>();
        towerSprite = transform.Find("TowerSprite").GetComponent<SpriteRenderer>();
        spellController = transform.Find("SpellController").GetComponent<SpellController>();

        towerSprite.sprite = playerSprites_SO.sprites[0];
        maxHP = hPMPControl.HPMP_SO.MaxHP;
        specialSpellCooldownLogic = new SpellCooldownLogic(specialSpellCooldown);

        specialSpellCooldownLogic.CooldownActivated += SetActive_SpecialSpellModeButton;
        specialSpellCooldownLogic.CooldownDeactivated += SetActive_SpecialSpellModeButton;

        PlayerDeath += OnPlayerDeath;
    }

    private void SetActive_SpecialSpellModeButton()
    {
        if (GameManager.Instance.TutorialActive)
            return;

        UI_Manager.Instance.SetActive_SpecialSpellModeButton(IsSpecialSpellCooldownActive);
    }

    private void Start() => OnPlayerLoaded();

    private void OnEnable()
    {
        GameManager.Instance.GestureRecognizedWithSpellName += spellController.OnGestureRecognized;
        PlayerDeath += UI_Manager.Instance.ToggleGameLoseMenu;
        InputManager.Instance.SwipeUp += spellController.OnSwipeUp;
        InputManager.Instance.SwipeDown += spellController.OnSwipeDown;
        InputManager.Instance.ProjectileSwipe += spellController.OnProjectileSwipe;
        InputManager.Instance.SwipeUp += spellController.OnSwipeUp;
    }

    private void OnDisable()
    {
        GameManager.Instance.GestureRecognizedWithSpellName -= spellController.OnGestureRecognized;
        PlayerDeath -= UI_Manager.Instance.ToggleGameLoseMenu;
        InputManager.Instance.SwipeUp -= spellController.OnSwipeUp;
        InputManager.Instance.SwipeDown -= spellController.OnSwipeDown;
        InputManager.Instance.ProjectileSwipe -= spellController.OnProjectileSwipe;
        InputManager.Instance.SwipeUp -= spellController.OnSwipeUp;
    }

    private void Update()
    {
        SpriteChange();
        GodMode(GameManager.Instance.GodMode);
        specialSpellCooldownLogic.CheckCooldownTime();
    }

    public void ProcessSpecialSpellCooldown() => specialSpellCooldownLogic.ProcessCooldown();

    public void Death()
    {
        playerDead = true;
        if (PlayerDeath != null) PlayerDeath();
    }

    private void SpriteChange()
    {
        currentHP = hPMPControl.GetCurrentHP();

        if (currentHP >= 0.7 * maxHP) towerSprite.sprite = playerSprites_SO.sprites[0];
        else if (currentHP >= 0.4 * maxHP) towerSprite.sprite = playerSprites_SO.sprites[1];
        else towerSprite.sprite = playerSprites_SO.sprites[2];
    }

    private void GodMode(bool value) => hPMPControl.GodMode(value);

    private void OnPlayerLoaded()
    {
        playerDead = false;
        spellController.OnPlayerLoaded();
    }

    private void OnPlayerDeath() => spellController.OnPlayerDeath();
}

