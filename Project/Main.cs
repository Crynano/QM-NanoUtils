using System;
using System.Collections.Generic;
using MGSC;
using UnityEngine;

namespace QM_NanoUtils
{
    public class Main
    {
        #region Variables

        public static bool IsPluginInstalled { get; } = true;
        private static MGSC.Player Player => MGSC.DungeonGameMode.Instance.Creatures.Player;

        private static List<KeyValuePair<string, BaseEffect>> _registeredEffects =
            new List<KeyValuePair<string, BaseEffect>>();

        #endregion

        #region Hooks

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AddExamples(IModContext context)
        {
            API.RegisterEffect("backpack_medical", new DevEffect(1, 0) { Endless = true });
            API.RegisterEffect("backpack_duggur", new DevEffectNoView(1, 0) { Endless = true });
        }

        // [Hook(ModHookType.DungeonStarted)]
        // public static void LoadUIEffects()
        // {
        //     foreach (var entry in _registeredEffects)
        //     {
        //         AddEffectToUI(entry.Value);
        //     }
        // }
        //
        // [Hook(ModHookType.DungeonFinished)]
        // public static void UnloadUIEffects()
        // {
        //     foreach (var entry in _registeredEffects)
        //     {
        //         RemoveEffectFromUI(entry.Value);
        //     }
        // }

        private static bool _modLoaded = false;

        [Hook(ModHookType.DungeonStarted)]
        public static void HookMod(IModContext context)
        {
            if (_modLoaded) return;
            HookToMercenaryInventory();
            _modLoaded = true;
            // And ALSO apply all status effects to the player? When loading the dungeon for the first time? But what if, just game start? Let me check
            MGSC.SharedUi.EscScreen.ExitToMainMenu += UnhookToMercenaryInventory;
        }

        [Hook(ModHookType.DungeonFinished)]
        public static void UnhookMod(IModContext context)
        {
            // When exiting the game or going to the main menu, this fails completely. Because player is destroyed.
            // Maybe we can hook to player destruction and remove hooks from there.
            // But can't test because there's no status effect persistance.
            if (!_modLoaded) return;
            try
            {
                UnhookToMercenaryInventory();
                _modLoaded = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while unhooking Mod\n{ex}");
            }
            finally
            {
                // You do not want to assume the moad has been unloaded correctly.
                // Gotta find a way to UNHOOK before exiting and quitting the game.
                // Maybe when the player is getting destroyed or else.
                //_modLoaded = false;
                MGSC.SharedUi.EscScreen.ExitToMainMenu -= UnhookToMercenaryInventory;
            }
        }

        private static void HookToMercenaryInventory()
        {
            Console.WriteLine($"Started loading hooks for inventory.");
            Player.Inventory.HelmetSlot.OnItemAdded += AddedPlayerItem;
            Player.Inventory.HelmetSlot.OnItemRemoved += RemovedPlayerItem;
            Player.Inventory.HelmetSlot.OnItemSwitched += SwapPlayerItem;
            Player.Inventory.ArmorSlot.OnItemAdded += AddedPlayerItem;
            Player.Inventory.ArmorSlot.OnItemRemoved += RemovedPlayerItem;
            Player.Inventory.ArmorSlot.OnItemSwitched += SwapPlayerItem;
            Player.Inventory.LeggingsSlot.OnItemAdded += AddedPlayerItem;
            Player.Inventory.LeggingsSlot.OnItemRemoved += RemovedPlayerItem;
            Player.Inventory.LeggingsSlot.OnItemSwitched += SwapPlayerItem;
            Player.Inventory.BootsSlot.OnItemAdded += AddedPlayerItem;
            Player.Inventory.BootsSlot.OnItemRemoved += RemovedPlayerItem;
            Player.Inventory.BootsSlot.OnItemSwitched += SwapPlayerItem;
            Player.Inventory.VestSlot.OnItemAdded += AddedPlayerItem;
            Player.Inventory.VestSlot.OnItemRemoved += RemovedPlayerItem;
            Player.Inventory.VestSlot.OnItemSwitched += SwapPlayerItem;
            Player.Inventory.BackpackSlot.OnItemAdded += AddedPlayerItem;
            Player.Inventory.BackpackSlot.OnItemRemoved += RemovedPlayerItem;
            Player.Inventory.BackpackSlot.OnItemSwitched += SwapPlayerItem;
            Console.WriteLine($"Successfully loaded hooks.");
        }

        private static void UnhookToMercenaryInventory()
        {
            Console.WriteLine($"Started unloading hooks for inventory");
            Player.Inventory.HelmetSlot.OnItemAdded -= AddedPlayerItem;
            Player.Inventory.HelmetSlot.OnItemRemoved -= RemovedPlayerItem;
            Player.Inventory.HelmetSlot.OnItemSwitched -= SwapPlayerItem;
            Player.Inventory.ArmorSlot.OnItemAdded -= AddedPlayerItem;
            Player.Inventory.ArmorSlot.OnItemRemoved -= RemovedPlayerItem;
            Player.Inventory.ArmorSlot.OnItemSwitched -= SwapPlayerItem;
            Player.Inventory.LeggingsSlot.OnItemAdded -= AddedPlayerItem;
            Player.Inventory.LeggingsSlot.OnItemRemoved -= RemovedPlayerItem;
            Player.Inventory.LeggingsSlot.OnItemSwitched -= SwapPlayerItem;
            Player.Inventory.BootsSlot.OnItemAdded -= AddedPlayerItem;
            Player.Inventory.BootsSlot.OnItemRemoved -= RemovedPlayerItem;
            Player.Inventory.BootsSlot.OnItemSwitched -= SwapPlayerItem;
            Player.Inventory.VestSlot.OnItemAdded -= AddedPlayerItem;
            Player.Inventory.VestSlot.OnItemRemoved -= RemovedPlayerItem;
            Player.Inventory.VestSlot.OnItemSwitched -= SwapPlayerItem;
            Player.Inventory.BackpackSlot.OnItemAdded -= AddedPlayerItem;
            Player.Inventory.BackpackSlot.OnItemRemoved -= RemovedPlayerItem;
            Player.Inventory.BackpackSlot.OnItemSwitched -= SwapPlayerItem;
            Console.WriteLine($"Successfully unloaded hooks.");
        }

        private static void AddedPlayerItem(BasePickupItem pickupItem)
        {
            var itemRecord = pickupItem.Record<BasePickupItemRecord>();
            if (itemRecord != null)
            {
                Console.WriteLine($"Player equipped: {itemRecord.Id}!");
                // Call the function!
                ApplyAllObjectEffects(itemRecord.Id);
            }
            else
            {
                Console.WriteLine($"ItemRecord is null! {pickupItem.Id}");
            }
        }

        private static void RemovedPlayerItem(BasePickupItem pickupItem)
        {
            var itemRecord = pickupItem.Record<BasePickupItemRecord>();
            if (itemRecord != null)
            {
                Console.WriteLine($"Player removed: {itemRecord.Id}!");
                // Call the function!
                RemoveAllObjectEffects(itemRecord.Id);
            }
            else
            {
                Console.WriteLine($"ItemRecord is null! {pickupItem.Id}");
            }
        }

        private static void SwapPlayerItem(BasePickupItem oldItem, BasePickupItem newItem)
        {
            Console.WriteLine($"Player swapped old item: {oldItem.Id} to {newItem.Id}");
            RemoveAllObjectEffects(oldItem.Id);
            ApplyAllObjectEffects(newItem.Id);
        }

        private static void ApplyAllObjectEffects(string ID)
        {
            var foundEffects = _registeredEffects.FindAll(x => x.Key.Equals(ID));
            Console.WriteLine($"I found [{foundEffects.Count}] effects with ID [{ID}]");
            foreach (var effect in foundEffects)
            {
                ApplySingleEffect(effect.Value);
            }
        }

        private static void RemoveAllObjectEffects(string ID)
        {
            var foundEffects = _registeredEffects.FindAll(x => x.Key.Equals(ID));
            Console.WriteLine($"I found [{foundEffects.Count}] effects with ID [{ID}]");
            foreach (var effect in foundEffects)
            {
                RemoveSingleEffect(effect.Value);
            }
        }

        #endregion

        #region Effect Handling

        private static void ApplySingleEffect(BaseEffect effect)
        {
            if (effect == null) return;
            Console.WriteLine($"Added {effect.GetType().Name} to Player");
            // And also UI
            AddEffectToUI(effect);
            Player.EffectsController.Add(effect);
        }

        private static void RemoveSingleEffect(BaseEffect effect)
        {
            if (effect == null) return;
            Console.WriteLine($"Removed {effect.GetType().Name} of Player");
            Player.EffectsController.Remove(effect);
            // And we remove it afterwards.
            RemoveEffectFromUI(effect);
        }

        #endregion

        #region UI Handling

        private static void AddEffectToUI(BaseEffect effect)
        {
            if (!(effect is IEffectWithView effectWithView))
            {
                return;
            }

            // Variables
            var effectsView = MGSC.DungeonUI.Instance.Hud.Effects;
            string effectKey = effectsView.GetEffectKey(effect);
            CommonEffectPanel basePanel = effectsView.GetPanel();
            
            // TODO ADD CUSTOM SPRITE IMPORT for now we use WHITE, because NULL crashes!
            basePanel.Initialize(MGSC.DungeonGameMode.Instance.Creatures, effectWithView,
                Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero, 100f));
            
            // Init custom control
            // CustomEffectPanel effectPanel = basePanel.gameObject.AddComponent<CustomEffectPanel>();
            // effectPanel.Clone(basePanel, effectWithView);
            // // Destroy the basePanel?
            // UnityEngine.Object.DestroyImmediate(basePanel);
            
            // Non destroy version
            NanoEffectPanel effectPanel = basePanel.gameObject.AddComponent<NanoEffectPanel>();
            effectPanel.Init(effectWithView);
            
            // Register them
            effectsView._typeToPanels.Add(effectKey, new List<CommonEffectPanel>() { basePanel });
            effectsView._typeToEffects.Add(effectKey, new List<IEffectWithView> { effectWithView });
        }

        private static void RemoveEffectFromUI(BaseEffect effect)
        {
            if (!(effect is IEffectWithView effectWithView))
            {
                return;
            }

            var effectsView = MGSC.DungeonUI.Instance.Hud.Effects;
            string effectKey = effectsView.GetEffectKey(effect);
            effectsView._typeToPanels.Remove(effectKey);
        }

        #endregion

        #region API

        public static class API
        {
            public static void RegisterEffect(string ID, BaseEffect effect)
            {
                AddEffectToRegistry(new KeyValuePair<string, BaseEffect>(ID, effect));
            }

            public static void RegisterEffect(KeyValuePair<string, BaseEffect> entry)
            {
                // Send a key value or any other type
                AddEffectToRegistry(entry);
            }

            private static void AddEffectToRegistry(KeyValuePair<string, BaseEffect> entry)
            {
                // This will allow duplicate entries to be registered
                // Because an item may have multiple ones active
                Main._registeredEffects.Add(entry);
            }

            public static void UnregisterEffect(string ID, BaseEffect effect)
            {
                RemoveEffectFromRegistry(new KeyValuePair<string, BaseEffect>(ID, effect));
            }

            public static void UnregisterEffect(KeyValuePair<string, BaseEffect> entry)
            {
                RemoveEffectFromRegistry(entry);
            }

            private static void RemoveEffectFromRegistry(KeyValuePair<string, BaseEffect> entry)
            {
                // Only remove the effect that exactly matches.
                // Prone to error because of using new BaseEffects everytime
                _registeredEffects.RemoveAll(x =>
                    x.Key == entry.Key &&
                    x.Value.GetType() == entry.Value.GetType());
            }
        }

        #endregion
    }
}