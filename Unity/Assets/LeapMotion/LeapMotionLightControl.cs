using UnityEngine;
using Leap;  // Aseg�rate de usar el namespace correcto de Leap Motion

public class LeapMotionLightControl : MonoBehaviour
{
    private Controller leapController;

    public Light[] controlledLights;  // Arreglo para las luces que deseas controlar
    public Renderer[] controlledRenderers;  // Arreglo para los renderers cuyos materiales cambiar�n

    private Material[] materialInstances;  // Instancias de materiales para cada luz

    public float lightIntensityFactor = 1.0f;  // Factor de control de intensidad
    public float colorChangeSpeed = 2.0f;      // Velocidad con la que cambia el color
    public float curtainDelay = 0.1f;          // Delay entre las luces de la "cortina"

    private Color[][] colorRanges = new Color[][]
    {
        new Color[] { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue },  // Rango c�lido a fr�o
        new Color[] { Color.magenta, new Color(0.5f, 0, 0.5f), Color.green, Color.yellow, Color.white },  // Rango con colores saturados
        new Color[] { Color.green, Color.blue, Color.magenta, Color.red, new Color(0.5f, 0.5f, 0) },  // Rango contrastado
    };

    private int colorRangeIndex = 0;  // Controlar qu� rango de colores estamos usando
    private float[] lightTimings;     // Tiempo de inicio de cada luz en la animaci�n de "cortina"

    private Color[] currentFingerColors = new Color[5];  // Almacenamos el color actual de cada dedo para transici�n

    void Start()
    {
        leapController = new Controller();  // Iniciar el controlador de Leap Motion

        // Inicializar los materiales para cada luz
        materialInstances = new Material[controlledRenderers.Length];
        for (int i = 0; i < controlledRenderers.Length; i++)
        {
            materialInstances[i] = new Material(controlledRenderers[i].sharedMaterial);  // Usar sharedMaterial para no crear una nueva instancia cada vez
            controlledRenderers[i].material = materialInstances[i];  // Asignamos la instancia del material a cada objeto
        }

        // Inicializar los tiempos de la "cortina"
        lightTimings = new float[controlledLights.Length];
        for (int i = 0; i < controlledLights.Length; i++)
        {
            lightTimings[i] = curtainDelay * i;  // Establecemos un peque�o delay entre cada luz
        }

        // Inicializamos el color de los dedos a blanco o cualquier valor predeterminado
        for (int i = 0; i < 5; i++)
        {
            currentFingerColors[i] = Color.white;
        }
    }

    void Update()
    {
        Frame frame = leapController.Frame(); // Obt�n el frame actual

        // Si la mano est� presente
        if (frame.Hands.Count > 0)
        {
            Hand hand = frame.Hands[0];  // Tomamos la primera mano (puedes agregar l�gica para usar ambas manos)

            // Control de la rotaci�n de la mu�eca para cambiar colores
            float handRoll = hand.Rotation.eulerAngles.z;  // El �ngulo de roll (rotaci�n de la mu�eca)

            // Normalizamos el valor del �ngulo de la rotaci�n a un rango de 0 a 1
            // Donde 0.0 es rojo y 1.0 tambi�n es rojo, pasando por todo el espectro crom�tico
            float hue = Mathf.InverseLerp(0, 360, handRoll);  // Normalizamos el �ngulo entre 0 y 1

            // Control de la apertura de la mano
            float handOpeness = hand.GrabStrength;  // Apertura de la mano (0 a 1)
            float curtainTransition = Mathf.Lerp(0, 1, handOpeness);  // El valor de la "cortina"

            // Iterar sobre las luces y controlar su animaci�n e interacci�n
            for (int i = 0; i < controlledLights.Length; i++)
            {
                // Cambio de color basado en la rotaci�n de la mu�eca (no depende de la "cortina")
                Color targetColor = Color.HSVToRGB(hue, 1.0f, 1.0f);  // Color generado a partir de la rotaci�n

                // Animaci�n de "cortina": Controlar la transici�n de colores de izquierda a derecha
                if (Time.time >= lightTimings[i])
                {
                    float positionFactor = Mathf.Clamp01(curtainTransition - (curtainDelay * i));
                    controlledLights[i].color = Color.Lerp(
                        colorRanges[colorRangeIndex][i],
                        targetColor,
                        positionFactor
                    );
                }

                // Cambiar la intensidad de la luz en funci�n de la posici�n de la palma
                controlledLights[i].intensity = Mathf.Clamp(hand.PalmPosition.y * lightIntensityFactor + i * 10, 160, 280);

                // Modificar la emisi�n del material
                if (materialInstances[i] != null)
                {
                    materialInstances[i].SetColor("_EmissiveColor", targetColor);  // Modificar el color emisivo del material
                    materialInstances[i].SetFloat("_EmissiveIntensity", 20000.0f);  // Aumentar la intensidad emisiva
                }
            }

            // Mapeo de dedos para control de color de cada luz
            Finger[] fingers = hand.fingers; // Accedemos al arreglo de dedos de la mano

            for (int i = 0; i < fingers.Length; i++)  // Iteramos sobre los dedos
            {
                Finger finger = fingers[i];  // Obtener el dedo correspondiente
                if (finger.IsExtended)  // Verificar si el dedo est� extendido
                {
                    Color targetFingerColor = GetFingerColor(i);  // Determinamos qu� color asignar seg�n el dedo

                    // Hacemos la transici�n suave entre el color actual y el nuevo color del dedo
                    currentFingerColors[i] = Color.Lerp(currentFingerColors[i], targetFingerColor, colorChangeSpeed * Time.deltaTime);

                    // Aplicar el color interpolado al dedo y a la luz correspondiente
                    controlledLights[i].color = currentFingerColors[i];
                    if (materialInstances[i] != null)
                    {
                        materialInstances[i].SetColor("_EmissiveColor", currentFingerColors[i]);  // Modificamos el color emisivo
                    }
                }
            }
        }
        else
        {
            // Si no hay manos, desactiva las luces o ponlas en un valor por defecto
            foreach (var light in controlledLights)
            {
                light.intensity = (light.intensity >0)? light.intensity-10 :0;
            }
        }
    }

    // Funci�n para mapear cada dedo a un color espec�fico
    Color GetFingerColor(int fingerIndex)
    {
        switch (fingerIndex)
        {
            case 0: return Color.red;  // Pulgar
            case 1: return Color.green;  // �ndice
            case 2: return Color.blue;  // Medio
            case 3: return Color.yellow;  // Anular
            case 4: return Color.cyan;  // Me�ique
            default: return Color.white;
        }
    }
}
