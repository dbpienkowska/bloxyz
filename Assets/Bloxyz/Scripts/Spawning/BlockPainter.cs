using UnityEngine;
using UnityEngine.Assertions;

namespace Bloxyz
{
    public class BlockPainter : MonoBehaviour
    {
        public ColorPalette palette;
        public Coloring coloring = Coloring.Sequence;

        public enum Coloring { Random, Sequence, First, Last }

        private int _index = 0;

        public void Paint(Block block)
        {
            Assert.IsNotNull(palette);
            Assert.IsTrue(palette.colors.Length > 0);

            switch(coloring)
            {
                case Coloring.Random:
                    _PaintCubes(block, palette.colors[Random.Range(0, palette.colors.Length)]);
                    break;
                case Coloring.Sequence:
                    _PaintCubes(block, palette.colors[_index]);
                    _index = (_index + 1) % palette.colors.Length;
                    break;
                case Coloring.First:
                    _PaintCubes(block, palette.colors[0]);
                    break;
                case Coloring.Last:
                    _PaintCubes(block, palette.colors[palette.colors.Length - 1]);
                    break;
            }
        }

        public void _PaintCubes(Block block, Color color)
        {
            foreach(CubeSlot slot in block.slots)
                slot.cube.color = color;
        }
    }
}

