using SeaSharkMC.Game.Inventory;

namespace SeaSharkMC.Game;

public class PlayerInventory
{
    // refer to https://wiki.vg/images/1/13/Inventory-slots.png
    public Slot[] slots = new Slot[46];

    public PlayerInventory()
    {
        for (int i = 0; i < 46; i++)
        {
            slots[i] = new Slot();
        }
    }
}