using UnityEngine;

public static class Settings
{
    public const string EnemyLayerName = "Enemy";
    public const string BaseLayerName = "Base";
    public const string SpellLayerName = "Spells";
    public const string GroundLayerName = "Ground";

    public const string ResourcesFolder = @".\Assets\Resources";
    public const string GestureTemplatesFolderName = "Gesture Templates";
    public const string GestureErrorMessage = "Gesture not recognized";

    public const float OrcSpawnPosition_Y = -6.52f;
    public const float CyclopSpawnPosition_Y = -1.32f;
    public const float GargSpawnPosition_Y = -1.80f;

    public const float TutorialEnemyMovementSpeed = 0.7f;

    public const string GameSceneName = "GameScene";
    public const string GameScene_Dev_Name = "GameScene_Dev";
    public const string TutorialSceneName = "TutorialScene";
}
