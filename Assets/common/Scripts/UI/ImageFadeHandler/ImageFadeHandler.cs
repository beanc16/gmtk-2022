using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Beanc16.Common.UI
{
    public class ImageFadeHandler : MonoBehaviour
    {
        [SerializeField] private List<ImageFadeContainer> imagesToFade;



        private void Update()
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                imagesToFade[i].Update();
            }
        }



        public void FadeInAll()
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                this.FadeIn(i);
            }
        }

        public void FadeOutAll()
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                this.FadeOut(i);
            }
        }

        public void FadeStopAll()
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                this.FadeStop(i);
            }
        }

        public void ShowAll()
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                this.Show(i);
            }
        }

        public void HideAll()
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                this.Hide(i);
            }
        }

        public void SetOpacityAll(float alpha)
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                this.SetOpacity(i, alpha);
            }
        }



        public void FadeInAllBut(int index)
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                if (i == index) continue;
                this.FadeIn(i);
            }
        }

        public void FadeOutAllBut(int index)
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                if (i == index) continue;
                this.FadeOut(i);
            }
        }

        public void FadeStopAllBut(int index)
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                if (i == index) continue;
                this.FadeStop(i);
            }
        }

        public void ShowAllBut(int index)
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                if (i == index) continue;
                this.Show(i);
            }
        }

        public void HideAllBut(int index)
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                if (i == index) continue;
                this.Hide(i);
            }
        }

        public void SetOpacityAllBut(int index, float alpha)
        {
            for (int i = 0; i < imagesToFade.Count; i++)
            {
                if (i == index) continue;
                this.SetOpacity(i, alpha);
            }
        }



        public void FadeIn(int index)
        {
            imagesToFade[index].FadeIn();
        }

        public void FadeOut(int index)
        {
            imagesToFade[index].FadeOut();
        }

        public void FadeStop(int index)
        {
            imagesToFade[index].FadeStop();
        }



        public void Show(int index)
        {
            imagesToFade[index].Show();
        }

        public void Hide(int index)
        {
            imagesToFade[index].Hide();
        }

        public void SetOpacity(int index, float alpha)
        {
            imagesToFade[index].SetOpacity(alpha);
        }
    }



    [System.Serializable]
    public class ImageFadeContainer
    {
        [SerializeField] private Image imageToFade;
        [SerializeField, Range(0f, 5f)] private float fadeSpeed = 1f;
        [SerializeField] private ImageFadeState state = ImageFadeState.NONE;

        [HideInInspector]
        public float FadeMultiplier { get => this.fadeSpeed * Time.deltaTime; }
        private float CurOpacity { get => this.imageToFade.color.a; }



        public void Update()
        {
            if (this.state != ImageFadeState.NONE)
            {
                float newOpacity = GetNewOpacity();
                this.SetOpacity(newOpacity);
            }
        }

        private float GetNewOpacity()
        {
            float newOpacity = this.CurOpacity;

            if (state == ImageFadeState.FADE_IN)
            {
                newOpacity += this.FadeMultiplier;
            }
            else if (state == ImageFadeState.FADE_OUT)
            {
                newOpacity -= this.FadeMultiplier;
            }

            newOpacity = BindNewOpacity(newOpacity);

            return newOpacity;
        }

        private float BindNewOpacity(float newOpacity)
        {
            // Don't go out of bounds of the min/max values
            if (newOpacity > 1f)
            {
                newOpacity = 1f;
                this.state = ImageFadeState.NONE;
            }
            else if (newOpacity < 0f)
            {
                newOpacity = 0f;
                this.state = ImageFadeState.NONE;
            }

            return newOpacity;
        }



        public void FadeIn()
        {
            this.state = ImageFadeState.FADE_IN;
        }

        public void FadeOut()
        {
            this.state = ImageFadeState.FADE_OUT;
        }

        public void FadeStop()
        {
            this.state = ImageFadeState.NONE;
        }



        public void Show()
        {
            this.SetOpacity(1f);
        }

        public void Hide()
        {
            this.SetOpacity(0f);
        }

        public void SetOpacity(float alpha)
        {
            imageToFade.color = new Color(
                imageToFade.color.r,
                imageToFade.color.g,
                imageToFade.color.b,
                alpha
            );
        }
    }



    public enum ImageFadeState
    {
        NONE,
        FADE_IN,
        FADE_OUT
    }
}
