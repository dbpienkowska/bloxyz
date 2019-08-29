using UnityEngine;

namespace Bloxyz
{
    public class BlockTransformer : MonoBehaviour, IPlatformUser
    {
        public Platform platform { private get; set; }
        public Positioning positioning = Positioning.Random;
        public Rotating rotating = Rotating.Random;

        public enum Positioning { Random, Start, End }
        public enum Rotating { Random, None, OneTurn, OneCounterTurn, HalfTurn }

        private Vector2Int _minTranslation;
        private Vector2Int _maxTranslation;

        public void Transform(Block block)
        {
            switch(rotating)
            {
                case Rotating.Random:
                    _SetRotation(block, Random.Range(0, 3) * 90); break;
                case Rotating.None:
                    break;
                case Rotating.OneTurn:
                    _SetRotation(block, 90); break;
                case Rotating.OneCounterTurn:
                    _SetRotation(block, -90); break;
                case Rotating.HalfTurn:
                    _SetRotation(block, 180); break;
            }

            _UpdateTranslationBounds(block);

            switch(positioning)
            {
                case Positioning.Random:
                    _SetRandomPosition(block); break;
                case Positioning.Start:
                    _SetStartPosition(block); break;
                case Positioning.End:
                    _SetEndPosition(block); break;
            }
        }

        private void _SetRotation(Block block, int rotation)
        {
            block.transform.eulerAngles = Vector3.zero;
            block.transform.Rotate(Vector3.up * rotation);
        }

        private void _SetRandomPosition(Block block)
        {
            int randomX = Random.Range(_minTranslation.x, _maxTranslation.x + 1);
            int randomZ = Random.Range(_minTranslation.y, _maxTranslation.y + 1);

            block.transform.ChangeLocalPosition(x: randomX, z: randomZ);
        }

        private void _SetStartPosition(Block block)
        {
            block.transform.ChangeLocalPosition(x: _minTranslation.x, z: _minTranslation.y);
        }

        private void _SetEndPosition(Block block)
        {
            block.transform.ChangeLocalPosition(x: _maxTranslation.x, z: _maxTranslation.y);
        }

        private void _UpdateTranslationBounds(Block block)
        {
            int minMaxStartMultiplier = platform.size + 1;
            _minTranslation = Vector2Int.one * minMaxStartMultiplier;
            _maxTranslation = Vector2Int.one * -minMaxStartMultiplier;

            foreach(CubeSlot slot in block.bottomSlots)
            {
                if(slot.x < _minTranslation.x)
                    _minTranslation.x = slot.x;
                if(slot.x > _maxTranslation.x)
                    _maxTranslation.x = slot.x;
                if(slot.z < _minTranslation.y)
                    _minTranslation.y = slot.z;
                if(slot.z > _maxTranslation.y)
                    _maxTranslation.y = slot.z;
            }

            _minTranslation.x = 0 - _minTranslation.x;
            _minTranslation.y = 0 - _minTranslation.y;

            int platformMaxXZ = platform.size - 1;
            _maxTranslation.x = platformMaxXZ - _maxTranslation.x;
            _maxTranslation.y = platformMaxXZ - _maxTranslation.y;
        }
    }
}

