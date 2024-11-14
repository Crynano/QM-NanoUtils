using MGSC;

namespace QM_NanoUtils
{
    public class DevEffectNoView : MGSC.BaseEffect, MGSC.IEffectWithVisualState
    {
        [Save]
        private int _effectValue = 256;
        
        public CreatureVisualState CreatureVisualState { get; } = CreatureVisualState.Burning;

        public DevEffectNoView() : base()
        {
            // Do Nothing?
        }
        
        public DevEffectNoView(int id, int duration)
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