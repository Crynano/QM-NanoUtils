using System;
using System.Globalization;
using MGSC;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QM_NanoUtils
{
    public class CustomEffectPanel : CommonEffectPanel
    {
        private IEffectWithView attachedEffect;

        public void Clone(CommonEffectPanel basePanel, IEffectWithView attachedEffect)
        {
            this._bg = basePanel._bg;
            this._bgCover = basePanel._bgCover;
            this._icon = basePanel._icon;
            this._whitefade = basePanel._whitefade;
            this._whitefadeCurve = basePanel._whitefadeCurve;
            //this._count  = basePanel._count;
            this._coverRedColor = basePanel._coverRedColor;
            this._coverGreenColor = basePanel._coverGreenColor;
            this._redBorder = basePanel._redBorder;
            this._greenBorder  = basePanel._greenBorder;
            this._yellowBorder = basePanel._yellowBorder;
            this._effectWithViews.Clear();
            this._effectWithViews.AddRange(basePanel._effectWithViews);
            this._creatures = basePanel._creatures;
            this._originalBgSprite = basePanel._originalBgSprite;
            this._timeSinceShow = basePanel._timeSinceShow;
            this._cachedValue = basePanel._cachedValue;
            this._createdTooltip = basePanel._createdTooltip;
            //
            this.attachedEffect = attachedEffect;
        }
        
        public new void OnPointerEnter(PointerEventData eventData)
        {
            // InitTooltip();
            // My custom code
            CreateCustomTooltip();
            _bg.sprite = _yellowBorder;
        }
        
        public new void OnPointerExit(PointerEventData eventData)
        {
            if (_createdTooltip)
            {
                SingletonMonoBehaviour<TooltipFactory>.Instance.HideTooltip();
            }
            _createdTooltip = false;
            _bg.sprite = _originalBgSprite;
        }

        protected virtual void CreateCustomTooltip()
        {
            _createdTooltip = true;
            Console.WriteLine($"CALLING CUSTOM CREATE TOOLTIP AND ITS A SUCCESS");
            Tooltip tooltip = SingletonMonoBehaviour<TooltipFactory>.Instance.BuildEmptyTooltip(wide: false, red: true);
            tooltip.SetCaption1("Default Title", SingletonMonoBehaviour<TooltipFactory>.Instance.FirstLetterColor);
            tooltip.SetCaption2("Default Label");
            tooltip.SetDescription("Default description of the custom tooltip.");
            tooltip.SetCaption1Right(attachedEffect.ViewValue.ToString(CultureInfo.InvariantCulture));
            SingletonMonoBehaviour<TooltipFactory>.Instance.AddPanelToTooltip().Initialize("Burning Bro");
            tooltip.EndContent();
        }
    }
}