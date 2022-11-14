public class DifficultyToStars
{
    public int stars { get { return _stars; } }
    private int _stars;

    public DifficultyToStars(Difficulty difficulty)
    {
        if (difficulty == Difficulty.normal) _stars = 1;
        else if (difficulty == Difficulty.hard) _stars = 2;
        else if (difficulty == Difficulty.crazy) _stars = 3;
    }
}
