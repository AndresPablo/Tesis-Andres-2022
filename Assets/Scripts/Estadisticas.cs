public static class Estadisticas
{
    // Variables de estadísticas globales
    public static int Puntos { get; set; }
    public static int Muertes { get; set; }
    public static float Tiempo_Jugado { get; set; }
    public static int Victorias_Izquierda { get; set; }
    public static int Victorias_Derecha { get; set; }

    // Método para reiniciar estadísticas
    public static void ResetStats()
    {
        Puntos = 0;
        Muertes = 0;
        Tiempo_Jugado = 0f;
        Victorias_Izquierda = 0;
        Victorias_Derecha = 0;
    }
}