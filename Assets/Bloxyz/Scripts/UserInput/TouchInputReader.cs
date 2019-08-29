using UnityEngine;

namespace Bloxyz
{
    public class TouchInputReader : MonoBehaviour, IInputReader
    {
        public ICommand startGameCommand { get; set; }
        public ICommand pauseResumeCommand { get; set; }
        public ICommand rotateLeftCommand { get; set; }
        public ICommand rotateRightCommand { get; set; }
        public ICommand quickLandCommand { get; set; }

        public float swipeMinDistance = 12f;
        public float tapMaxDistance = 0.1f;

        private Touch _touch;
        private float _startX;
        private float _startY;
        //private float _halfScreenWidth = Screen.width / 2;
        //private float _halfScreenHeight = Screen.height / 2;

        private void Update()
        {
            if(Input.touchCount == 1)
            {
                _touch = Input.GetTouch(0);

                if(_touch.phase == TouchPhase.Began)
                {
                    _startX = _touch.position.x;
                    _startY = _touch.position.y;
                }
                if(_touch.phase == TouchPhase.Ended)
                {
                    float xDiff = _touch.position.x - _startX;
                    float yDiff = _touch.position.y - _startY;

                    if(Mathf.Abs(xDiff) < tapMaxDistance && Mathf.Abs(yDiff) < tapMaxDistance)
                    {
                        if(_touch.tapCount == 1)
                            startGameCommand.Execute();
                        else if(_touch.tapCount == 2)
                            pauseResumeCommand.Execute();
                    }
                    else if(Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
                    {
                        if(xDiff < -swipeMinDistance)
                            rotateRightCommand.Execute();
                        else if(xDiff > swipeMinDistance)
                            rotateLeftCommand.Execute();
                    }
                    else if(Mathf.Abs(yDiff) > Mathf.Abs(xDiff))
                    {
                        if(yDiff < -swipeMinDistance)
                            quickLandCommand.Execute();
                    }
                }
            }
        }
    }
}

