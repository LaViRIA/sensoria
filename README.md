# Sensoria

> **Interactive installation combining gestural control of light and sound**  
> Leap Motion · Max/MSP · Pure Data · Ableton Live · Arduino Nano · Unity (digital twin)

---
## Overview
*Sensoria* convierte gestos de las manos, captados por un Leap Motion, en eventos **MIDI** (sonido) y **OSC** (luz). El procesamiento ocurre en Max/MSP; Pure Data recibe los mensajes OSC y, a través del extern **PDuino**, envía salidas PWM a tres Arduino Nano que alimentan tiras **LED COB 24 V**. La latencia gesto → sonido se mantiene en **≈ 24 ms ± 3 ms**.

---
## Repository structure
| Path | Descripción |
|------|-------------|
| `max/` | Parches Max/MSP  
├── `Ultraleap.amxd` — flujo Leap Motion → MIDI/OSC  
└── `osc_router.maxpat` — envío OSC a Pure Data |
| `pd/`  | Patches Pure Data  
├── `Pduino_RGB.pd` — conversión OSC → PWM vía PDuino  
└── `hsv2rgb.pd` — utilidades de color |
| `unity/` | Proyecto **Unity 2022.3 URP** (digital twin) |
| `ableton/` | Sesión Ableton Live 12 `sensoria.als` |
| `docs/` | Diagramas, esquemáticos y paper PMW‑2025 |

> **Nota**: no se incluye firmware .ino; los Arduino Nano corren **StandardFirmata** cargado desde el IDE de Arduino.

---
## Hardware
| Componente | Cant. | Detalles |
|------------|------|----------|
| Leap Motion Controller | 1 | Gemini 5.2 SDK |
| Arduino Nano | 3 | Firmware *StandardFirmata* @ 24 kHz PWM |
| MOSFET IRLZ44N | 9 | 3 por canal RGB |
| LED COB strip 24 V (GRB) | 5 × 1 m | Distribuidas en la sala |
| PSU 24 V / 5 A | 1 | Fuente única |

---
## Software prerequisites
* **Max/MSP 8.6**  
  *Packages:* `aka.leapmotion`, `aka.osc`  
* **Pure Data 0.54** + externs **PDuino 0.6** y **mrpeach**  
* **Ableton Live 12 Suite** (Operator, Scale, Arpeggiator, EQ Eight)  
* **Unity 2022.3 URP** + `WebSocketSharp` (digital twin)  
* **Arduino IDE 2.x** (para cargar *StandardFirmata*)

---
## Quick start
```bash
# Clonar el repositorio
$ git clone https://github.com/<user>/sensoria.git
$ cd sensoria
```
1. **Cargar firmware Firmata**  
   Abre el IDE de Arduino → Ejemplos → Firmata → *StandardFirmata* → sube a cada Nano.
2. **Iniciar Pure Data**  
   Abre `pd/Pduino_RGB.pd` → clic en *Connect* → verifica pines PWM 3‑5‑6.  
3. **Lanzar Max/MSP**  
   Abre `max/Ultraleap.amxd` → activa *Leap* y salidas OSC/MIDI.  
4. **Abrir Ableton**  
   Carga `ableton/sensoria_session.als`; ajusta buffer a 256 samples.  
5. **Interactuar**  
   Coloca las manos sobre el Leap Motion → observa sonido y luces.

---
## Gesture → MIDI mapping
| Gesto (Leap) | Mensaje MIDI | Rango | Destino |
|--------------|-------------|-------|---------|
| X mano izq. | **Note On** | 0‑127 | Operator (Celesta) |
| Velocidad mano izq. | **Velocity** | 0‑127 | Dinámica |
| Y mano der. | **CC 2** | 0‑127 | Delay (Send A) |
| X mano der. | **CC 3** | 0‑127 | Reverb (Send B) |

---
## OSC routing (Max → Pure Data)
```text
/ManolzqX <f>
/ManolzqY <f>
/ManolzqZ <f>
/ManoderX <f>
/ManoderY <f>
/ManoderZ <f>
```
El patch `Pduino_RGB.pd` empaqueta estos valores HSV y los mapea a PWM.

---

## Credits
Proyecto de tesis — Licenciatura en Artes Digitales, UG.  Es
