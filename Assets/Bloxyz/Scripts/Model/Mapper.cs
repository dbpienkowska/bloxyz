namespace Bloxyz
{
    public class Mapper
    {
        public static int platformSize = 2;

        public static int MapX(int x, int z, int rotation)
        {
            if(rotation < 0)
                rotation = rotation + 360;

            int result = 0;

            switch(rotation)
            {
                case 0:
                    result = x;
                    break;
                case 90:
                    result = platformSize - 1 - z;
                    break;
                case 180:
                    result = platformSize - 1 - x;
                    break;
                case 270:
                    result = z;
                    break;
            }

            return result;
        }

        public static int MapZ(int x, int z, int rotation)
        {
            if(rotation < 0)
                rotation = rotation + 360;

            int result = 0;

            switch(rotation)
            {
                case 0:
                    result = z;
                    break;
                case 90:
                    result = x;
                    break;
                case 180:
                    result = platformSize - 1 - z;
                    break;
                case 270:
                    result = platformSize - 1 - x;
                    break;
            }

            return result;
        }
    }
}
