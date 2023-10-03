namespace SeaSharkMC.Game;

public struct PlayerAbilities
{
    public PlayerAbilityFlags flags;
    public float flyingSpeed;
    public float fovModifer;

    public bool invulnerable
    {
        get => flags.HasFlag(PlayerAbilityFlags.INVULNERABLE);
        set => flags = value ? flags | PlayerAbilityFlags.INVULNERABLE : flags & ~PlayerAbilityFlags.INVULNERABLE;
    }

    public bool flying
    {
        get => flags.HasFlag(PlayerAbilityFlags.FLYING);
        set => flags = value ? flags | PlayerAbilityFlags.FLYING : flags & ~PlayerAbilityFlags.FLYING;
    }

    public bool allowFlight
    {
        get => flags.HasFlag(PlayerAbilityFlags.ALLOW_FLIGHT);
        set => flags = value ? flags | PlayerAbilityFlags.ALLOW_FLIGHT : flags & ~PlayerAbilityFlags.ALLOW_FLIGHT;
    }

    public bool instantBreak
    {
        get => flags.HasFlag(PlayerAbilityFlags.CREATIVE_MODE);
        set => flags = value ? flags | PlayerAbilityFlags.CREATIVE_MODE : flags & ~PlayerAbilityFlags.CREATIVE_MODE;
    }

    public PlayerAbilities(float flyingSpeed = 0.05f, float fovModifer = 0.1f, bool invulnerable = false,
        bool flying = false, bool allowFlight = false, bool instantBreak = false)
    {
        this.flyingSpeed = flyingSpeed;
        this.fovModifer = fovModifer;
        this.invulnerable = invulnerable;
        this.flying = flying;
        this.allowFlight = allowFlight;
        this.instantBreak = instantBreak;
    }
}