using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UIPanel
{
    public class UI_LogoPanel: PanelBase
    {
        [SerializeField]
        private Image USCLogo;
        
        [SerializeField]
        private Image RollingBladeLogo;
        
        [SerializeField]
        private Text RollingBladeTitle;
        
        public override void Show()
        {
            // OnUnPause();
            base.Show();
            StartCoroutine(FadeInAndOut(3));
        }

        IEnumerator FadeInAndOut(float time)
        {
            RollingBladeLogo.DOFade(1f, 2).SetEase(Ease.OutCubic);
            RollingBladeTitle.DOFade(1f, 2).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(time);
            RollingBladeLogo.DOFade(0f, 2).SetEase(Ease.OutCubic);
            RollingBladeTitle.DOFade(0f, 2).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(1);
            USCLogo.DOFade(1f, 2).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(time);
            USCLogo.DOFade(0f, 2).SetEase(Ease.OutCubic);
        }


        // public override void Hide(Action callBack)
        // {
        //     base.Hide(callBack);
        // }
        
        protected override void OnPause()
        {
        }

        protected override void OnUnPause()
        {
        }
    }
}