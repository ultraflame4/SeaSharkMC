using System;

namespace SeaSharkMC.Game;
[Flags]
public enum PlayerAbilityFlags : byte
{
    INVULNERABLE = 0x01,
    FLYING = 0x02,
    ALLOW_FLIGHT= 0x04,
    CREATIVE_MODE = 0x08
}