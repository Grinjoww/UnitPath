using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class GameDB : MonoBehaviour
{
    // Esta clase define cómo es la tabla de Jugador
    public class Jugador
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Dinero { get; set; }
    }

    void Start()
    {
        // 1. Definir la ruta donde se guardará el archivo .db
        string rutaDB = Application.streamingAssetsPath + "/unipath.db";

        // Si no existe la carpeta StreamingAssets, la creamos (solo para evitar errores)
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        // 2. Crear la conexión
        var connection = new SQLiteConnection(rutaDB, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // 3. Crear la tabla (si no existe)
        connection.CreateTable<Jugador>();

        // 4. Guardar un dato de prueba
        var nuevoJugador = new Jugador
        {
            Nombre = "Jaime",
            Dinero = 100
        };
        connection.Insert(nuevoJugador);

        Debug.Log("¡Base de Datos Creada! Guardada en: " + rutaDB);
    }
}
