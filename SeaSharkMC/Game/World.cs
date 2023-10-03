namespace SeaSharkMC.Game;

public class World
{
    public readonly string name;
    public readonly long hashed_seed = 0;
    public readonly bool is_debug = false;
    public readonly bool is_flat = false;
    
    public GameDifficulty difficulty = GameDifficulty.PEACEFUL;
    public bool difficultyLocked = true;
    
    public World(string name) { this.name = name; }
}