using System;

public class LevelController : DebuggableBaseClass
{
    public event Action<int> LevelIndexSet;

    private Levels_SO levels_SO;
    private int levelIndex;
    private GameModes gameMode;

    public int LevelIndex => levelIndex;
    public Levels_SO Levels_SO => levels_SO; 
    public GameModes GameMode  => gameMode;

    private void Awake() => levels_SO = ResourceLoader.LoadLevels_SO();

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
        LevelIndexSet?.Invoke(levelIndex);
    }

    public void SetGameMode(GameModes gameMode) => this.gameMode = gameMode;
}
