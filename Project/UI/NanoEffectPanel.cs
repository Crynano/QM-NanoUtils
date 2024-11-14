using System;
using System.Globalization;
using MGSC;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QM_NanoUtils
{
    public class NanoEffectPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private IEffectWithView _attachedEffect;
        private bool _createdTooltip = false;
        
        public void Init(IEffectWithView effect)
        {
            this._attachedEffect = effect;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // InitTooltip();
            // My custom code
            CreateCustomTooltip();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_createdTooltip)
            {
                SingletonMonoBehaviour<TooltipFactory>.Instance.HideTooltip();
            }
            _createdTooltip = false;
        }

        protected virtual void CreateCustomTooltip()
        {
            _createdTooltip = true;
            Console.WriteLine($"CALLING CUSTOM CREATE TOOLTIP AND ITS A SUCCESS");
            Tooltip tooltip = SingletonMonoBehaviour<TooltipFactory>.Instance.BuildEmptyTooltip(wide: false, red: true);
            tooltip.SetCaption1("Default Title", SingletonMonoBehaviour<TooltipFactory>.Instance.FirstLetterColor);
            tooltip.SetCaption2("Default Label");
            tooltip.SetDescription("Default description of the custom tooltip.");
            tooltip.SetCaption1Right(_attachedEffect.ViewValue.ToString(CultureInfo.InvariantCulture));
            SingletonMonoBehaviour<TooltipFactory>.Instance.AddPanelToTooltip().Initialize("Custom Effect");
            tooltip.EndContent();
        }
    }
}