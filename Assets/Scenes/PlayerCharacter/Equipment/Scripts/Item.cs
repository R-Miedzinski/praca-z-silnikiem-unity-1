using UnityEngine;
using System.Collections.Generic;
using System;

public class Item : MonoBehaviour
{
    public int id;
    public string itemName;
    public string description;
    public Dictionary<EItemUsageType, ItemEffect[]> effects = new Dictionary<EItemUsageType, ItemEffect[]>();

   // TODO: Create a proper constructor.
    public Item(int id, string itemName, string description, ItemEffect[] effects)
    {
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        Array.ForEach(effects, new Action<ItemEffect>(effect =>
        {
            if (!this.effects.ContainsKey(effect.type))
            {
                this.effects[effect.type] = new ItemEffect[] { effect };
            }
            else
            {
                this.effects[effect.type] = this.effects[effect.type] + new ItemEffect[] { effect };
            }
        }));
    }

}
