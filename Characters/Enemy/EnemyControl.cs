using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : BaseEnemyControl
{
    [NonSerialized] public int playerStartingVector = 1;

    [SerializeField] private EnemyBehaviour_SO enemyBehaviour;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask baseLayer;

    [Header("Default base")]
    private EnemyHPMPControl enemyHPMPControl;
    private Rigidbody2D rigidbody2D;
    private EnemySFXManager enemySFX;
    private Slider HPSlider;
    private Animator enemyAnimator;

    private float nextRegTime = 1f;

    private string attackAnimation = "Attack";
    private string walkAnimation = "Walk";
    private string death = "Death";

    private bool isAtackAnimationEnd = true;
    private bool isAtackingNow = false;
    private bool isWalking = true;
    private bool isRegenerating = false;

    private DifficultyScaling enemy;

    public bool IsMovingEnemy { get => isMovingEnemy; set => isMovingEnemy = value; }

    void Start()
    {
        enemySFX = GetComponentInChildren(typeof(EnemySFXManager)) as EnemySFXManager;
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        enemyHPMPControl = GetComponent<EnemyHPMPControl>();
        HPSlider = GetComponentInChildren(typeof(Slider)) as Slider;
        isAtackingNow = false;
        isWalking = true;

        HPSlider.transform.localScale = new Vector2(HPSlider.transform.localScale.x * playerStartingVector, HPSlider.transform.localScale.y);

        enemy = new DifficultyScaling(GameManager.Instance.StageDifficulty, enemyBehaviour);

        StartCoroutine(Run());
        StartCoroutine(RegenerationHP());
        StartCoroutine(Atack());
    }
    IEnumerator Atack()
    {
        while (true)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, baseLayer);

            if (hitEnemies.Length > 0)
            {
                ChangeToAtackAnimation();
                isAtackingNow = true;
                isWalking = false;

                yield return new WaitUntil(() => isAtackAnimationEnd);
                yield return new WaitForSeconds(0.1f);
                isAtackAnimationEnd = false;

                foreach (var enemies in hitEnemies)
                {
                    enemies.GetComponent<IDamagable>().TakeDamage(enemy.damage);
                }

                yield return new WaitForSeconds(enemy.timeBetweenAtack);
            }
            else isAtackingNow = false;
            yield return null;
        }
    }

    IEnumerator RegenerationHP()
    {
        if (!isRegenerating)
        {
            isRegenerating = true;
            while (true)
            {
                if (enemyHPMPControl.currentHP < enemy.maxHP)
                {
                    enemyHPMPControl.currentHP += enemy.regHP;
                    if (enemyHPMPControl.currentHP > enemy.maxHP) enemyHPMPControl.currentHP = enemy.maxHP;
                }
                yield return new WaitForSeconds(nextRegTime);
            }
        }
        isRegenerating = false;
    }

    IEnumerator Run()
    {
        while (IsMovingEnemy)
        {
            if (!isAtackingNow)
            {

                ChangeToWalkAnimation();

                Vector2 EnemyVelocity = new Vector2(overrideMovementSpeed ? newMovementSpeed : enemy.speed * playerStartingVector,
                    rigidbody2D.velocity.y);

                rigidbody2D.velocity = EnemyVelocity;

            }
            yield return null;
        }
    }

    public void SetDirection()
    {
        if (playerStartingVector < 0)
        {
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
        }
    }

    void ChangeToAtackAnimation()
    {
        enemyAnimator.Play(attackAnimation, -1, 0);
        enemySFX.atackAudioPlay();
    }

    void ChangeToWalkAnimation()
    {
        if (!isWalking) enemyAnimator.Play(walkAnimation);
        isWalking = true;
    }

    public void StopAllCoroutinesAfterDeath()
    {
        StopAllCoroutines();
        enemySFX.DisableHurtSFXAfterDeath();
        enemySFX.deathAudioPlay();
        enemyAnimator.Play(death);
    }

}

