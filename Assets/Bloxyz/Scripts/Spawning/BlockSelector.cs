using UnityEngine;

namespace Bloxyz
{
    public class BlockSelector : MonoBehaviour
    {
        public enum SelectionCriteria { Random, First, Last };

        public Block[] blockPrefabs;
        public SelectionCriteria criteria = SelectionCriteria.Random;

        public Block Select()
        {
            Block block = null;

            switch(criteria)
            {
                case SelectionCriteria.First:
                    block = SelectFirst(); break;
                case SelectionCriteria.Last:
                    block = SelectLast(); break;
                case SelectionCriteria.Random:
                    block = SelectRandom(); break;
            }

            return block;
        }

        private Block SelectRandom() => blockPrefabs[Random.Range(0, blockPrefabs.Length)];

        private Block SelectFirst() => blockPrefabs[0];

        private Block SelectLast() => blockPrefabs[blockPrefabs.Length - 1];
    }
}

