using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSystem : DebuggableBaseClass
{
    [SerializeField] private MonstersList_SO enemyTypeList;
    [SerializeField] private LevelData_SO levelWavesSO;
    [SerializeField] private ScenesSettings_SO scenesSettingSO;
    [SerializeField] private GameObject wavesProgressObj;

    public event Action OnSpawnChangeFillValueEvent;
    public event Action OnEndSpawningChangeEvent;
    public event Action<int> GameWon;

    private GameManager gameManager;
    private WavesProgressSlider wavesProgressBar;

    private float nextSpawnTime = 0;
    private Vector3 LeftSpawn = new Vector3(19f, 0, 0);
    private Vector3 RightSpawn = new Vector3(-19f, 0, 0);

    private bool isGameOver = false;
    private bool isGameWin = false;

    private int index = 0;
    private int levelIndex = 0;
    private Coroutine spawningCoroutine;

    private void Awake()
    {
        wavesProgressBar = wavesProgressObj.GetComponent<WavesProgressSlider>();
        levelWavesSO = scenesSettingSO.levelData_SO;
        for (int j = 0; j < levelWavesSO.wavesMonsters.Length; j++)
        {
            levelWavesSO.wavesMonsters[j].isSpawningNow = false;
        }
        isGameOver = false;
    }

    public void SetLevelIndex(int levelIndex) => this.levelIndex = levelIndex;

    public float GetTimeToSpawnNextWaves(int waveIndex, bool max)
    {
        if (max) waveIndex = levelWavesSO.wavesMonsters.Length;

        int waveCounter = 0;

        float finalTime = 0;

        foreach (var waves in levelWavesSO.wavesMonsters)
        {
            if (waveCounter > waveIndex) return finalTime;
            float time = 0;
            foreach (var wave in waves.waveMonsters)
            {
                float tempTime = wave.amountOfMonsters * wave.timeBetweenNewMonster;
                if (tempTime > time) time = tempTime;
            }
            finalTime += time;
            waveCounter++;
        }
        return finalTime;
    }

    public int GetAmountOfWaves() => levelWavesSO.wavesMonsters.Length - 1;

    // Find your monster script with enum
    public GameObject GetMonsterType(MonsterTypes enemyType)
    {
        foreach (var enemyIndex in enemyTypeList.monsterList)
        {
            if (enemyIndex.monsterEnum == enemyType) return enemyIndex.monster;
        }

        return null;
    }

    public void OnClickSpawnWave()
    {
        //if (GameManager.Instance.GameMode == GameModes.SandboxMode)
        //{
        //    StartCoroutine(WaveLogic(index));
        //}
        //else
        if (levelWavesSO.wavesMonsters[index].isSpawningNow == false)
        {
            GetSpawnTime();
            levelWavesSO.wavesMonsters[index].isSpawningNow = true;
            StartCoroutine(WaveLogic(index));
        }
    }

    public void EndTheGame()
    {
        isGameOver = true;
        StopAllCoroutines();
        DestroyAllMonters();
    }

    public void StartTheGame()
    {
        //Reload all elements
        isGameOver = false;
        nextSpawnTime = 0;
        index = 0;
        levelWavesSO = scenesSettingSO.levelData_SO;
        for (int j = 0; j < levelWavesSO.wavesMonsters.Length; j++)
        {
            levelWavesSO.wavesMonsters[j].isSpawningNow = false;
        }
    }

    public int GetStarsAmountInthisLevel()
    {
        PrintDebugLog($"{nameof(WaveSystem)} - Number of stars in this level: {new DifficultyToStars(GameManager.Instance.StageDifficulty).stars} ");
        return new DifficultyToStars(GameManager.Instance.StageDifficulty).stars;
    }

    IEnumerator WaveLogic(int wavesIndex)
    {
        if (index < levelWavesSO.wavesMonsters.Length - 1) index++;
        bool isSpawningNow = false;
        int currentWaveIndex = 0;

        if (GameManager.Instance.GameMode == GameModes.SandboxMode)
        {
            while (true)
            {
                WaveMonster waveMonster = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[currentWaveIndex];
                //if (levelWavesSO.wavesMonsters[wavesIndex].isSpawningNow)
                //{
                    if (!isSpawningNow)
                    {
                        spawningCoroutine = StartCoroutine(SpawnWave(currentWaveIndex, wavesIndex));
                        isSpawningNow = true;
                    }
         
                    if (wavesIndex < levelWavesSO.wavesMonsters.Length - 1 &&
                        currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1)
                    {
                        wavesIndex++;
                        currentWaveIndex = 0;
                    }

                    // change list current wave index value
                    if (currentWaveIndex < levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1 && isSpawningNow)
                    {
                        isSpawningNow = false;
                        currentWaveIndex++;
                    }
                    else if (currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1 &&
                        wavesIndex == levelWavesSO.wavesMonsters.Length - 1)
                    {              
                        wavesIndex = 0;
                        currentWaveIndex = 0;
                        isSpawningNow = false;
                        for (int j = 0; j < levelWavesSO.wavesMonsters.Length; j++)
                        {
                            levelWavesSO.wavesMonsters[j].isSpawningNow = false;
                        }                      
                    }
                    yield return new WaitForSeconds(waveMonster.timeBetweenNewWave);
                //}
                yield return null;
            }
            //WaveMonster waveMonster;
            //while (true)
            //{
            //    //WaveMonster waveMonster = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[currentWaveIndex];

            //    //if (!isSpawningNow)
            //    //{
            //    //    spawningCoroutine = StartCoroutine(SpawnWave(currentWaveIndex, wavesIndex));
            //    //    isSpawningNow = true;
            //    //}

            //    spawningCoroutine = StartCoroutine(SpawnWave(currentWaveIndex, wavesIndex));
            //    //currentWaveIndex++;


            //    // change list waves index value
            //    //if (wavesIndex < levelWavesSO.wavesMonsters.Length - 1 &&
            //    //    currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1)
            //    //{
            //    //    break;
            //    //}


            //    if (currentWaveIndex < levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1)
            //    {
            //        //isSpawningNow = false;
            //        currentWaveIndex++;
            //    }

            //    if (currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1 &&
            //            wavesIndex == levelWavesSO.wavesMonsters.Length - 1)
            //    {
            //        // All enemies are spawned
            //        wavesIndex = 0;
            //        currentWaveIndex = 0;
            //    }
            //    yield return new WaitForSecondsRealtime(3f);
            //}
        }
        else
        {
            while (true)
            {
                WaveMonster waveMonster = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[currentWaveIndex];
                if (levelWavesSO.wavesMonsters[wavesIndex].isSpawningNow)
                {

                    if (!isSpawningNow)
                    {
                        spawningCoroutine = StartCoroutine(SpawnWave(currentWaveIndex, wavesIndex));
                        isSpawningNow = true;
                    }

                    // change list waves index value
                    if (wavesIndex < levelWavesSO.wavesMonsters.Length - 1 &&
                        currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1)
                    {
                        break;
                    }

                    // change list current wave index value
                    if (currentWaveIndex < levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1 && isSpawningNow)
                    {
                        isSpawningNow = false;
                        currentWaveIndex++;
                    }
                    else if (currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1 &&
                        wavesIndex == levelWavesSO.wavesMonsters.Length - 1)
                    {
                        // All enemies are spawned
                        StarSpriteEasingScale();
                        StartCoroutine(WinTheGame());
                        break;
                    }
                    yield return new WaitForSeconds(waveMonster.timeBetweenNewWave);
                }
                yield return null;
            }
        }

    }

    IEnumerator WinTheGame()
    {
        DifficultyToStars DTS = new DifficultyToStars(GameManager.Instance.StageDifficulty);
        while (true)
        {
            if (transform.childCount == 0)
            {
                yield return new WaitWhile(() => wavesProgressBar.waveSlider.value < 1);
                yield return new WaitForSeconds(2f);

                GameManager.Instance.PlayerProgressDataController.Player_SO.ChangeStarsAmount(levelIndex, DTS.stars);
                GameManager.Instance.GameWin();
                GameWon?.Invoke(DTS.stars);

                break;
            }
            yield return null;
        }
    }

    IEnumerator SpawnWave(int thisWaveIndex, int wavesIndex)
    {
        WaveMonster waveMonster = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[thisWaveIndex];
        float spawningTime = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[thisWaveIndex].timeBetweenNewMonster;
        int i = 0;
        while (i < waveMonster.amountOfMonsters)
        {
            Spawn(waveMonster);
            yield return new WaitForSeconds(spawningTime);
            i++;
        }
    }

    void GetSpawnTime() => OnSpawnChangeFillValueEvent?.Invoke();

    void StarSpriteEasingScale() => OnEndSpawningChangeEvent?.Invoke();

    public void DestroyAllMonters()
    {
        StopAllCoroutines();
        foreach (Transform child in transform) GameObject.Destroy(child.gameObject);
    }

    void Spawn(WaveMonster monster)
    {
        if (GetMonsterType(monster.monster) == null) return;
        GameObject spawnEnemyRight = null;
        GameObject spawnEnemyLeft = null;

        if (monster.waveLocationSpawn == WaveLocationSpawns.Right)
        {
            spawnEnemyRight = Instantiate(GetMonsterType(monster.monster), transform) as GameObject;
        }
        else if (monster.waveLocationSpawn == WaveLocationSpawns.Left)
        {
            spawnEnemyLeft = Instantiate(GetMonsterType(monster.monster), transform) as GameObject;
        }
        else if (monster.waveLocationSpawn == WaveLocationSpawns.Both)
        {
            spawnEnemyLeft = Instantiate(GetMonsterType(monster.monster), transform) as GameObject;
            spawnEnemyRight = Instantiate(GetMonsterType(monster.monster), transform) as GameObject;
        }
        else
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                spawnEnemyRight = Instantiate(GetMonsterType(monster.monster), transform) as GameObject;
            }
            else if (rand == 1)
            {
                spawnEnemyLeft = Instantiate(GetMonsterType(monster.monster), transform) as GameObject;
            }
            else return;
        }

        if (spawnEnemyLeft != null)
        {
            spawnEnemyLeft.transform.parent = transform;
            spawnEnemyLeft.transform.localPosition = LeftSpawn + MonsterStartingPosition(monster); ;
            spawnEnemyLeft.transform.localScale = GetMonsterType(monster.monster).transform.localScale;
            if (spawnEnemyLeft.GetComponent<EnemyControl>() != null)
            {
                spawnEnemyLeft.GetComponent<EnemyControl>().playerStartingVector = -1;
                spawnEnemyLeft.GetComponent<EnemyControl>().SetDirection();
            }
            else if (spawnEnemyLeft.GetComponent<FlyEnemyControl>() != null)
            {
                spawnEnemyLeft.GetComponent<FlyEnemyControl>().playerStartingVector = -1;
                spawnEnemyLeft.GetComponent<FlyEnemyControl>().SetDirection();
            }
        }

        if (spawnEnemyRight != null)
        {
            spawnEnemyRight.transform.parent = transform;
            spawnEnemyRight.transform.localPosition = RightSpawn + MonsterStartingPosition(monster);
            spawnEnemyRight.transform.localScale = GetMonsterType(monster.monster).transform.localScale;
            if (spawnEnemyRight.GetComponent<EnemyControl>() != null)
            {
                spawnEnemyRight.GetComponent<EnemyControl>().playerStartingVector = 1;
                spawnEnemyRight.GetComponent<EnemyControl>().SetDirection();
            }
            else if (spawnEnemyRight.GetComponent<FlyEnemyControl>() != null)
            {
                spawnEnemyRight.GetComponent<FlyEnemyControl>().playerStartingVector = 1;
                spawnEnemyRight.GetComponent<FlyEnemyControl>().SetDirection();
            }
        }
    }

    // Monster starting y position spawn
    private Vector3 MonsterStartingPosition(WaveMonster monster)
    {
        if (monster.monster == MonsterTypes.Orc) return new Vector3(0, -2.5f, 0);
        if (monster.monster == MonsterTypes.Cyclop) return new Vector3(0, 3f, 0);
        if (monster.monster == MonsterTypes.Garg) return new Vector3(0, 1, 0);
        return new Vector3(0, 0, 0);
    }
}