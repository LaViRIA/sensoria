# Sensoria (English)

> **Interactive installation combining gestural control of light and sound**
> Leap Motion · Max/MSP · Pure Data · Ableton Live · Arduino Nano · Unity (digital twin)

---

## Overview

*Sensoria* turns hand gestures, captured by a Leap Motion, into **MIDI** (sound) and **OSC** (light) events. Max/MSP (inside Ableton Live as a Max for Live device) generates the messages; Pure Data receives **OSC** and, via the **PDuino** external, sends **PWM** to three Arduino Nano boards that drive **24 V COB LED strips**. Gesture → sound latency is maintained at **≈ 24 ms ± 3 ms**.

---

## Repository structure

| Path                                                                | Content                                                                      | Notes                                          |
| ------------------------------------------------------------------- | ---------------------------------------------------------------------------- | ---------------------------------------------- |
| `Ableton_Sensoria/`                                                 |                                                                              |                                                |
| ├── `Sensoria Project/` — Ableton set (`Sensoria.als`)              |                                                                              |                                                |
| └── `ULTRALEAP.amxd` — Max for Live device (Leap Motion → MIDI/OSC) |                                                                              |                                                |
| `PureData/`                                                         | `Pduino_RGB.pd` — OSC → PWM conversion via **PDuino**                        | Requires **mrpeach** and **pduino** externals. |
| `Unity/`                                                            | Unity 2022.3 URP project folder (`Assets/`, `Packages/`, `ProjectSettings/`) | Digital twin; independent of live flow.        |

> **Note**: Arduino Nano runs **StandardFirmata**; no `.ino` firmware included.

---

## Hardware

| Component                | Qty     | Details                                 |
| ------------------------ | ------- | --------------------------------------- |
| Leap Motion Controller   | 1       | Gemini 5.2 SDK                          |
| Arduino Nano             | 3       | *StandardFirmata* firmware @ 24 kHz PWM |
| IRFZ44N MOSFET           | 9       | 3 per RGB channel                       |
| LED COB strip 24 V (GRB) | 5 × 1 m | Installed in the room                   |
| PSU 24 V / 5 A           | 1       | Single supply                           |

---

## Software prerequisites

* **Ableton Live 12 Suite** (Operator, Scale, Arpeggiator, EQ Eight) with **Max for Live**
  *External device:* `ULTRALEAP.amxd` — if it doesn’t open, install `aka.leapmotion` ≥ 1.1.0 and `aka.osc` from Package Manager.
* **Pure Data 0.54** + externals **PDuino 0.6** and **mrpeach**
* **Unity 2022.3 URP**` (digital twin)
* **Arduino IDE 2.x** (to upload *StandardFirmata*)

---

## Quick start

```bash
# Clone the repository
$ git clone https://github.com/LaViRIA/sensoria.git	
$ cd sensoria
```

1. **Upload StandardFirmata** to each Arduino Nano: Arduino IDE → Examples → Firmata → *StandardFirmata*.
2. **Run Pure Data**: open `PureData/Pduino_RGB.pd` → click *Connect* → check PWM pins 3-5-6.
3. **Open Ableton Live**: double-click `Ableton_Sensoria/Sensoria Project/Sensoria.als`
   Make sure the track with **ULTRALEAP.amxd** is armed and Ableton detects your virtual MIDI port.
4. **Interact**: place your hands over Leap Motion → monitor sound and LED strips response.

---

## Gesture → MIDI mapping

| Gesture (Leap)  | MIDI message | Range | Destination        |
| --------------- | ------------ | ----- | ------------------ |
| Left hand X     | **Note On**  | 0-127 | Operator (Celesta) |
| Left hand speed | **Velocity** | 0-127 | Dynamics           |
| Right hand Y    | **CC 2**     | 0-127 | Delay (Send A)     |
| Right hand X    | **CC 3**     | 0-127 | Reverb (Send B)    |

---

## OSC routing (ULTRALEAP.amxd → Pure Data)

```
/ManolzqX <f>
/ManolzqY <f>
/ManolzqZ <f>
/ManoderX <f>
/ManoderY <f>
/ManoderZ <f>
```

The IP and port in the `udpsend` object must be set according to the machine running Pure Data.




---

# Sensoria (Español)

> **Instalación interactiva combinando control gestual de luz y sonido**
> Leap Motion · Max/MSP · Pure Data · Ableton Live · Arduino Nano · Unity (gemelo digital)

---

## Descripción general

*Sensoria* convierte gestos de las manos, captados por un Leap Motion, en eventos **MIDI** (sonido) y **OSC** (luz). Max/MSP (dentro de Ableton Live como dispositivo Max for Live) genera los mensajes; Pure Data recibe **OSC** y, mediante el extern **PDuino**, envía **PWM** a tres Arduino Nano que alimentan tiras **LED COB 24 V**. La latencia gesto → sonido se mantiene en **≈ 24 ms ± 3 ms**.

---

## Estructura del repositorio

| Ruta                                                                     | Contenido                                                                         | Notas                                                    |
| ------------------------------------------------------------------------ | --------------------------------------------------------------------------------- | -------------------------------------------------------- |
| `Ableton_Sensoria/`                                                      |                                                                                   |                                                          |
| ├── `Sensoria Project/` — set de Ableton (`Sensoria.als`)                |                                                                                   |                                                          |
| └── `ULTRALEAP.amxd` — dispositivo Max for Live (Leap Motion → MIDI/OSC) |                                                                                   |                                                          |
| `PureData/`                                                              | `Pduino_RGB.pd` — conversión OSC → PWM vía **PDuino**                             | Requiere externs **mrpeach** y **pduino**.               |
| `Unity/`                                                                 | Carpeta de proyecto Unity 2022.3 URP (`Assets/`, `Packages/`, `ProjectSettings/`) | Actúa como gemelo digital; no depende del flujo en vivo. |

> **Nota**: los Arduino Nano corren **StandardFirmata**; no se incluye firmware `.ino`.

---

## Hardware

| Componente               | Cant.   | Detalles                                |
| ------------------------ | ------- | --------------------------------------- |
| Leap Motion Controller   | 1       | Gemini 5.2 SDK                          |
| Arduino Nano             | 3       | Firmware *StandardFirmata* @ 24 kHz PWM |
| MOSFET IRFZ44N           | 9       | 3 por canal RGB                         |
| LED COB strip 24 V (GRB) | 5 × 1 m | Instaladas en la sala                   |
| PSU 24 V / 5 A           | 1       | Fuente única                            |

---

## Requisitos de software

* **Ableton Live 12 Suite** (Operator, Scale, Arpeggiator, EQ Eight) con **Max for Live** integrado
  *Dispositivo externo:* `ULTRALEAP.amxd` si no abre, instala `aka.leapmotion` ≥ 1.1.0 y `aka.osc` desde Package Manager.
* **Pure Data 0.54** + externs **PDuino 0.6** y **mrpeach**
* **Unity 2022.3 URP** (gemelo digital)
* **Arduino IDE 2.x** (para cargar *StandardFirmata*)

---

## Ejecución rápida

```bash
# Clonar el repositorio
$ git clone https://github.com/LaViRIA/sensoria.git	
$ cd sensoria
```

1. **Cargar StandardFirmata** en cada Arduino Nano: IDE Arduino → Ejemplos → Firmata → *StandardFirmata*.
2. **Ejecutar Pure Data**: abre `PureData/Pduino_RGB.pd` → clic en *Connect* → verifica pines PWM 3-5-6.
3. **Abrir Ableton Live**: doble-clic en `Ableton_Sensoria/Sensoria Project/Sensoria.als`
   Asegúrate de que la pista con **ULTRALEAP.amxd** esté armada y que Ableton reconozca tu puerto MIDI virtual.
4. **Interactuar**: coloca las manos sobre el Leap Motion → monitorea sonido y reacción de las tiras LED.

---

## Mapeo gesto → MIDI

| Gesto (Leap)        | Mensaje MIDI | Rango | Destino            |
| ------------------- | ------------ | ----- | ------------------ |
| X mano izq.         | **Note On**  | 0-127 | Operator (Celesta) |
| Velocidad mano izq. | **Velocity** | 0-127 | Dinámica           |
| Y mano der.         | **CC 2**     | 0-127 | Delay (Send A)     |
| X mano der.         | **CC 3**     | 0-127 | Reverb (Send B)    |

---

## Ruteo OSC (ULTRALEAP.amxd → Pure Data)

```
/ManolzqX <f>
/ManolzqY <f>
/ManolzqZ <f>
/ManoderX <f>
/ManoderY <f>
/ManoderZ <f>
```

La IP y el puerto del objeto `udpsend` deben ajustarse según la máquina que ejecute Pure Data.

---

