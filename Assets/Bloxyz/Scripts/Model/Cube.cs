using UnityEngine;

namespace Bloxyz
{
    public class Cube : MonoBehaviour
    {
        public int x => Mathf.RoundToInt(transform.position.x);
        public int y => Mathf.RoundToInt(transform.position.y);
        public int z => Mathf.RoundToInt(transform.position.z);
        public int localX => Mathf.RoundToInt(transform.localPosition.x);
        public int localY => Mathf.RoundToInt(transform.localPosition.y);
        public int localZ => Mathf.RoundToInt(transform.localPosition.z);

        public Color color
        {
            get => _material.color;
            set => _material.color = value;
        }

        private Material _material;

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
        }
    }
}

