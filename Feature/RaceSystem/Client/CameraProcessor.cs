using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client
{
    public class CameraProcessor
    {
        private static float CamSpeed = 2.33f;
        private static float Precision = 2.33f;
        
        public float offsetRotX = 0.0f;
        public float offsetRotY = 0.0f;
        public float offsetRotZ = 0.0f;
        
        public Vector3 ProcessNewPosition(Vector3 pos)
        {
            float newX = pos.X;
            float newY = pos.Y;
            float newZ = pos.Z;
            
            if (API.IsInputDisabled(0))
            {
                // Key "W"
                if (API.IsDisabledControlPressed(1, 32))
                {
                    Vector3 mult = CalculateMult();

                    newX -= 0.1f * CamSpeed * mult.X;
                    newY += 0.1f * CamSpeed * mult.Y;
                    newZ += 0.1f * CamSpeed * mult.Z;
                }
                
                // Key "S"
                if (API.IsDisabledControlPressed(1, 33))
                {
                    Vector3 mult = CalculateMult();

                    newX += 0.1f * CamSpeed * mult.X;
                    newY -= 0.1f * CamSpeed * mult.Y;
                    newZ -= 0.1f * CamSpeed * mult.Z;
                }
                
                // Key "A"
                if (API.IsDisabledControlPressed(1, 34))
                {
                    Vector3 mult = CalculateMult(true);

                    newX -= 0.1f * CamSpeed * mult.X;
                    newY += 0.1f * CamSpeed * mult.Y;
                    newZ += 0.1f * CamSpeed * mult.Z;
                }
                
                // Key "D"
                if (API.IsDisabledControlPressed(1, 35))
                {
                    Vector3 mult = CalculateMult(true);

                    newX += 0.1f * CamSpeed * mult.X;
                    newY -= 0.1f * CamSpeed * mult.Y;
                    newZ -= 0.1f * CamSpeed * mult.Z;
                }

                offsetRotX -= API.GetDisabledControlNormal(1, 2) * Precision * 8.0f;
                offsetRotZ -= API.GetDisabledControlNormal(1, 1) * Precision * 8.0f;
            }

            if (offsetRotX > 90.0f)
                offsetRotX = 90.0f;
            else if (offsetRotX < -90.0f)
                offsetRotX = -90.0f;

            if (offsetRotY > 90.0f)
                offsetRotY = 90.0f;
            else if (offsetRotY < -90.0f)
                offsetRotY = -90.0f;

            if (offsetRotZ > 360.0f)
                offsetRotZ = offsetRotZ - 360.0f;
            else if (offsetRotZ < -360.0f)
                offsetRotZ = offsetRotZ + 360.0f;

            return new Vector3(newX, newY, newZ);
        }

        private Vector3 CalculateMult(bool flag = false)
        {
            float multX = API.Sin(offsetRotZ + (flag ? 90.0f : 0.0f));
            float multY = API.Cos(offsetRotZ + (flag ? 90.0f : 0.0f));
            float multZ = API.Sin(flag ? offsetRotY : offsetRotX);
            
            return new Vector3(multX, multY, multZ);
        }
    }
}