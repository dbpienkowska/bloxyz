using UnityEngine;

namespace Bloxyz
{
    public class Track : MonoBehaviour
    {
        public int x => Mathf.RoundToInt(transform.position.x);
        public int z => Mathf.RoundToInt(transform.position.z);

        public float yDrop { get; private set; }

        private MeshRenderer _renderer;

        public void SetY(int y)
        {
            transform.ChangeLocalPosition(y: y - yDrop);
        }

        public void MultiplyColor(Color color)
        {
            _renderer.material.color *= color;
        }

        public void Hide()
        {
            _renderer.enabled = false;
        }

        public void Show()
        {
            _renderer.enabled = true;
        }

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();

            yDrop = (1f - transform.localScale.y) / 2;
            transform.ChangeLocalPosition(y: -yDrop);
        }

    }
}

