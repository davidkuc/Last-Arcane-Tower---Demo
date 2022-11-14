using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHPMPControl : MonoBehaviour
{
    public static Action<int, Rigidbody2D, SpellTypes, SpellEffects> OnDOT_Damage;
    public static Action<int, Rigidbody2D, SpellTypes, SpellEffects>  OnProjectileDamage;
    public static Action<Vector2, Rigidbody2D, SpellTypes, SpellEffects> OnKnockback;
    public static Action<int, Rigidbody2D, SpellTypes, SpellEffects> OnAOE_Damage;

    [SerializeField] public EnemyBehaviour_SO enemyBehaviour;
    private Animator enemyAnimator;
    private GoldAwardSystem goldSystem;
    private EnemyControl enemyControl;
    private FlyEnemyControl flyEnemyControl;

    private Coroutine DOT_Effect;
    private Rigidbody2D enemyRB;

    private EnemySFXManager enemySFX;
    private Slider HPSlider;

    private string hurt = "Hurt";

    [NonSerialized] public float currentHP;
    private bool isDying = false;

    DifficultyScaling enemy;

    public bool IsDOT_Active { get; private set; } = false;

    private void Awake()
    {
        enemySFX = GetComponentInChildren(typeof(EnemySFXManager)) as EnemySFXManager;
        enemy = new DifficultyScaling(GameManager.Instance.StageDifficulty, enemyBehaviour);
        currentHP = enemy.maxHP;
    }

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        goldSystem = GetComponent<GoldAwardSystem>();
        enemyControl = GetComponent<EnemyControl>();
        flyEnemyControl = GetComponent<FlyEnemyControl>();

        HPSlider = GetComponentInChildren(typeof(Slider)) as Slider;
        HPSliderValueChange();
    }

    private void HPSliderValueChange()
    {
        if (currentHP > 0)
            HPSlider.value = currentHP / enemy.maxHP;
        else HPSlider.value = 0;
    }

    public float GetCurrentHP() => currentHP;

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        HPSliderValueChange();

        if (currentHP <= 0)
        {
            if (!isDying)
            {
                Dying();
                isDying = true;
            }
        }
        else if (damage > 0.3 * enemy.maxHP)
        {
            ChangeToHurtAnimation();
        }
    }

    public void TakeAOE_Damage(int damage, SpellEffects spellEffect, SpellTypes spellType)
    {
        TakeDamage(damage);
        if (OnAOE_Damage != null) OnAOE_Damage(damage, this.enemyRB, spellType, spellEffect);
    }

    public void TakeKnockback(Vector2 force, SpellTypes spellType, SpellEffects spellEffect)
    {
        enemyRB.AddForce(force);
        if (OnKnockback != null) OnKnockback(force, this.enemyRB, spellType, spellEffect);
    }

    public void TakeProjectileDamage(int damage, SpellTypes spellType, SpellEffects spellEffect)
    {
        TakeDamage(damage);
        if (OnProjectileDamage != null) OnProjectileDamage(damage, this.enemyRB, spellType, spellEffect);
    }

    public void TakeDOT_Damage(float duration, float interval, int damagePerInterval, SpellTypes spellType, SpellEffects spellEffect)
    {
        if (IsDOT_Active) StopCoroutine(DOT_Effect);
        IsDOT_Active = true;
        DOT_Effect = StartCoroutine(DOT_Damage(duration, interval, damagePerInterval, spellType, spellEffect));
    }

    private IEnumerator DOT_Damage(float duration, float interval, int damagePerInterval, SpellTypes spellType, SpellEffects spellEffect)
    {
        int counter = 0;
        while (counter < duration)
        {
            yield return new WaitForSecondsRealtime(interval);
            TakeDamage(damagePerInterval);
            if (OnDOT_Damage != null) OnDOT_Damage(damagePerInterval, this.enemyRB, spellType, spellEffect);
            counter++;
        }
        IsDOT_Active = false;
    }

    private void Dying()
    {
        if (enemyControl != null)
            enemyControl.StopAllCoroutinesAfterDeath();
        else if (flyEnemyControl != null)
            flyEnemyControl.StopAllCoroutinesAfterDeath();
    }

    public void ChangeToHurtAnimation()
    {
        enemyAnimator.Play(hurt, -1, 0);
        enemySFX.hitAudioPlay();
    }

    void DestroyGameObject()
    {
        goldSystem.RewardPlayerWithGold();

        StartCoroutine(DestroyGameObjectAfterEndSFX());
    }

    IEnumerator DestroyGameObjectAfterEndSFX()
    {

        while (enemySFX.IsDeathAudioPlaying())
        {
            yield return null;
        }

        GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().OnMuteSFXChange -= enemySFX.MuteAllAudio;

        Destroy(gameObject);
    }
}

