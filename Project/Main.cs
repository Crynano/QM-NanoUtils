using System;
using MGSC;

namespace QM_NanoUtils
{
    public class Main
    {
        private static MGSC.Player Player => MGSC.DungeonGameMode.Instance.Creatures.Player;

        #region Hooks

        [Hook(ModHookType.DungeonStarted)]
        public static void HookToMercenaryInventory(IModContext context)
        {
            Console.WriteLine($"Loaded HookToMercenaryInventory");
            Player.Inventory.BackpackSlot.OnItemAdded += AddEffectToPlayer;
            Player.Inventory.BackpackSlot.OnItemRemoved += RemoveEffectFromPlayer;
        }
        
        [Hook(ModHookType.DungeonFinished)]
        public static void UnhookToMercenaryInventory(IModContext context)
        {
            Console.WriteLine($"Unloaded HookToMercenaryInventory");
            Player.Inventory.BackpackSlot.OnItemAdded -= AddEffectToPlayer;
            Player.Inventory.BackpackSlot.OnItemRemoved -= RemoveEffectFromPlayer;
        }

        #endregion
        
        private static void AddEffectToPlayer(BasePickupItem pickupItem)
        {
            var bpRecord = pickupItem.Record<BackpackRecord>();
            if (bpRecord != null) Console.WriteLine($"I have equipped the {bpRecord.Id}!");
            var hpRegen = new HealthRegenEffect(-1, 10);
            Console.WriteLine($"Applying the {hpRegen.ID} health regen!");
            Player.EffectsController.Add(hpRegen);
        }

        private static void RemoveEffectFromPlayer(BasePickupItem pickupItem)
        {
            var bpRecord = pickupItem.Record<BackpackRecord>();
            if (bpRecord != null) Console.WriteLine($"I have unequipped the {bpRecord.Id}!");
            Player.EffectsController.Remove(0); // Maybe it will not work...
        }

        
    }
}