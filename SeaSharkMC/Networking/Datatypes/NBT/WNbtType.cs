using System;
using fNbt;

namespace SeaSharkMC.Networking.Datatypes;

/// <summary>
/// All subclasses of this class provide an easy way to convert nbt to class instances for easy usage and vice versa.
/// </summary>
/// <typeparam name="T"></typeparam>
public class WNbtType<T> where T : NbtTag
{
    protected T nbtData;


    public WNbtType(T nbtData)
    {
        this.nbtData = nbtData;
    }

    public virtual T GetNbt()
    {
        throw new NotImplementedException("GetNbt() is not implemented for this class.");
    }
}