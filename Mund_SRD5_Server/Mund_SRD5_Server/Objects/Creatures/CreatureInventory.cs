using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Objects
{
    public partial class Creature
    {
        public Dictionary<InventorySlot, List<ItemType>> ItemSlotAllowance = new Dictionary<InventorySlot, List<ItemType>>()
        {
            { InventorySlot.Belt, new List<ItemType>() { ItemType.Belt } },
            { InventorySlot.Chest, new List<ItemType>() { ItemType.Clothing } },
            { InventorySlot.LeftHand, new List<ItemType>() { ItemType.LightWeapon, ItemType.Shield } },
            { InventorySlot.LeftRing, new List<ItemType>() { ItemType.Ring } },
            { InventorySlot.Neck, new List<ItemType>() { ItemType.Necklace } },
            { InventorySlot.RightHand, new List<ItemType>() { ItemType.LightWeapon, ItemType.HeavyOrAmmoWeapon, ItemType.LightWeapon } },
            { InventorySlot.RightRing, new List<ItemType>() { ItemType.Ring } },
        };

        public bool EquipItem(int InventorySlot, string ItemIdentifier)
        {
            if (String.IsNullOrWhiteSpace(ItemIdentifier))
            {
                if(Equipment.ContainsKey(InventorySlot))
                {
                    Inventory.Add(Equipment[InventorySlot]);
                    Equipment.Remove(InventorySlot);
                    return true;
                }
            }
            else
            {
                InventoryItem itemToRemove = null;
                foreach (InventoryItem it in Inventory)
                {
                    if (it.Identifier == ItemIdentifier)
                    {
                        if (ItemSlotAllowance[(InventorySlot)InventorySlot].Contains(it.ItType))
                        {
                            if (Equipment.ContainsKey(InventorySlot))
                            {
                                if (Equipment[InventorySlot] != null)
                                {
                                    Inventory.Add(Equipment[InventorySlot]);
                                }
                                itemToRemove = it;
                                Equipment[InventorySlot] = it;
                                break;
                            }
                            else
                            {
                                itemToRemove = it;
                                Equipment.Add(InventorySlot, it);
                                break;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if(itemToRemove != null)
                {
                    Inventory.Remove(itemToRemove);
                    return true;
                }
            }
            return false;
        }
    }
}
