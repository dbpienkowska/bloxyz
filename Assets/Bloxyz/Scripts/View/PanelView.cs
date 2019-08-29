using UnityEngine;

namespace Bloxyz
{
    public class PanelView : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        public void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        public void SetInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
            _canvasGroup.blocksRaycasts = interactable;
        }

        public void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;
        }

        private void Awake()
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
}

