using MGSC;
using UnityEngine;

namespace QM_NanoUtils
{
    public class DevEffect : BaseEffect, IEffectWithView, IEffectWithVisualState
    {
        public float ViewValue => 1;
        public EffectViewShowValueMode ShowValueMode { get; } = EffectViewShowValueMode.ShowMax;
        public EffectViewShowValueFormat ShowValueFormat { get; } = EffectViewShowValueFormat.Raw;
        public bool Show { get; } = true;
        public bool IsRedView { get; } = false;
        public bool BlinkOnChange { get; } = true;
        
        public CreatureVisualState CreatureVisualState { get; } = CreatureVisualState.Burning;

        public DevEffect()
        {
            Debug.Log($"Default ctr for DevEffect");
        }
        
        public DevEffect(int id, int duration)
        {
            this.ID = id;
            this.Duration = duration;
        }

        public override void ProcessActionPoint()
        {
            base.ProcessActionPoint();
        }

        public override void OnAdded()
        {
            base.OnAdded();
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
        }
    }
}