using UnityEngine;
using Leap;

public class LeapMotionLightControl_PDStyle : MonoBehaviour
{
    private Controller leapController;

    public Light[] leftHandLights;  // luces 1, 3, 5
    public Light[] rightHandLights; // luces 2, 4

    public float maxSaturationHeight = 8.0f;  // valor para dividir Y y obtener saturación
    public float hueMultiplier = 12.0f; // como 12 notas por "metro" de X

    void Start()
    {

        leapController = new Controller();

    }

    void Update()
    {
        Frame frame = leapController.Frame();

        if (frame.Hands.Count > 0)
        {
            foreach (Hand hand in frame.Hands)
            {
                bool isLeft = hand.IsLeft;
                Vector3 palmPosition = hand.PalmPosition;

                float xPos = Mathf.Abs(palmPosition.x);
                float yPos = palmPosition.y;

                float hue = (xPos * hueMultiplier) % 1f;
                float saturation = Mathf.Clamp01(yPos / maxSaturationHeight);

                Color color = Color.HSVToRGB(hue, saturation, 1f);

                Light[] lightsToControl = isLeft ? leftHandLights : rightHandLights;
                foreach (Light light in lightsToControl)
                {
                    light.color = color;
                }
            }
        }
        else
        {
            // Atenuar suavemente si no hay manos
            foreach (var light in leftHandLights)
                light.intensity = Mathf.Max(0, light.intensity - Time.deltaTime * 10);
            foreach (var light in rightHandLights)
                light.intensity = Mathf.Max(0, light.intensity - Time.deltaTime * 10);
        }
    }
}
