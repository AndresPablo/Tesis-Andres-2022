import cv2
import time
import numpy as np
from pythonosc.udp_client import SimpleUDPClient

def resource_path(relative_path):
    """ Get absolute path to resource, works for dev and for PyInstaller """
    try:
        # PyInstaller creates a temp folder and stores path in _MEIPASS
        base_path = sys._MEIPASS
    except Exception:
        base_path = os.path.abspath(".")

    return os.path.join(base_path, relative_path)

# Cargar el modelo
net = cv2.dnn.readNetFromCaffe('MobileNetSSD_deploy.prototxt', 'MobileNetSSD_deploy.caffemodel')

# Configurar la cámara
cap = cv2.VideoCapture(1)

# Verificar si la cámara está conectada correctamente
if not cap.isOpened():
    raise Exception("Error: No se pudo acceder a la cámara.")
else:
    print("Cámara conectada correctamente.")

# Definir el cliente OSC
client = SimpleUDPClient("127.0.0.1", 7001)

# Cargar la configuración desde un archivo
try:
    with open("config.txt", "r") as f:
        lines = f.readlines()
        confidence_threshold = float(lines[0].strip())
        fps = float(lines[1].strip())
except FileNotFoundError:
    confidence_threshold = 0.5  # Valor predeterminado
    fps = 6.0  # Valor predeterminado

# Variable para controlar la visualización
show_video = True

while True:
    # Leer la imagen de la cámara
    ret, img = cap.read()
    if not ret:
        break

    # Invertir la imagen horizontalmente (espejo)
    img = cv2.flip(img, 1)

    # Obtener el tamaño de la imagen
    height, width = img.shape[:2]

    # Dibujar una línea negra vertical en el medio de la pantalla
    cv2.line(img, (width // 2, 0), (width // 2, height), (0, 0, 0), 2)

    # Preparar la imagen para el modelo
    blob = cv2.dnn.blobFromImage(cv2.resize(img, (300, 300)), 0.007843, (300, 300), 127.5)
    net.setInput(blob)

    # Realizar la detección
    detections = net.forward()

    # Inicializar contadores
    izq = 0
    der = 0

    # Filtrar detecciones
    for i in range(detections.shape[2]):
        confidence = detections[0, 0, i, 2]
        if confidence > confidence_threshold:
            idx = int(detections[0, 0, i, 1])
            if idx == 15:  # 15 es el índice para "persona"
                box = detections[0, 0, i, 3:7] * np.array([width, height, width, height])
                (startX, startY, endX, endY) = box.astype("int")

                # Calcular el centro del rectángulo
                centerX = (startX + endX) // 2

                # Contar si está en el lado izquierdo o derecho
                if centerX < width // 2:
                    izq += 1
                    cv2.rectangle(img, (startX, startY), (endX, endY), (255, 0, 0), 2)  # Azul
                else:
                    der += 1
                    cv2.rectangle(img, (startX, startY), (endX, endY), (0, 255, 0), 2)  # Verde

    # Mostrar el número de detecciones en la parte superior izquierda
    if show_video:
        cv2.putText(img, f"Izquierda: {izq}", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)
        cv2.putText(img, f"Derecha: {der}", (width - 200, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

    # Crear un panel en la parte inferior
    panel_height = 100
    panel = np.zeros((panel_height, width, 3), dtype=np.uint8)
    panel[:] = (50, 50, 50)  # Color del panel

    # Escribir información en el panel
    cv2.putText(panel, f"Umbral: {confidence_threshold:.2f}", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (255, 255, 255), 2)
    cv2.putText(panel, f"FPS: {fps:.1f}", (10, 60), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (255, 255, 255), 2)
    cv2.putText(panel, "Atajos: u (+), d (-) | v (no mostrar) | q (cerrar)", (width // 6, 90), cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2)

    # Combinar la imagen y el panel
    output = np.vstack((img, panel))

    # Enviar los conteos por OSC
    client.send_message("/", [izq, der])

    # Ajustar el umbral de confianza y FPS con teclas
    key = cv2.waitKey(1) & 0xFF
    if key == ord('u'):  # Aumentar umbral
        confidence_threshold += 0.05
    elif key == ord('d'):  # Disminuir umbral
        confidence_threshold -= 0.05
    elif key == ord('f'):  # Aumentar FPS
        fps += 1
    elif key == ord('s'):  # Disminuir FPS
        fps = max(1, fps - 1)  # Mantener FPS mínimo en 1
    elif key == ord('v'):  # Alternar visualización
        show_video = not show_video

    # Asegurarse de que el umbral esté dentro de un rango razonable
    confidence_threshold = max(0.0, min(1.0, confidence_threshold))

    # Mostrar la imagen
    if show_video:
        cv2.imshow('Demokratos - VotoCam', output)

    # Limitar la tasa de fotogramas
    time.sleep(1 / fps)

    # Salir si se presiona 'q'
    if key == ord('q'):
        break

# Guardar la configuración en un archivo
with open("config.txt", "w") as f:
    f.write(f"{confidence_threshold:.2f}\n")
    f.write(f"{fps:.1f}\n")

# Liberar la cámara y cerrar ventanas
cap.release()
cv2.destroyAllWindows()
