using System.Collections;
using System.Collections.Generic;
using fNbt;

namespace SeaSharkMC.Networking.Datatypes.NBT;

public class WNbtList<T, D> : List<T> where T : WNbtType<D> where D : NbtTag
{
    protected string tagName;

    public WNbtList(string tagName) : base()
    {
        this.tagName = tagName;
    }

    public NbtList GetNbt()
    {
        NbtList list = new NbtList(tagName);
        foreach (WNbtType<D> item in this)
        {
            list.Add(item.GetNbt());
        }
        return list;
    }
}