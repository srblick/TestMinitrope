using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestSuite
{
    private BoardManager boardManager;
    public GameObject tilePrefab;
    private Tile tile;

    [SetUp]
    public void Setup()
    {
        // Inicializar un objeto BoardManager.
        GameObject boardManagerObj = new GameObject();
        boardManager = boardManagerObj.AddComponent<BoardManager>();

        // Cargar el prefab de tile desde la carpeta Resources
        tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
        
        // Asegúrate de que el prefab se haya cargado correctamente
        Assert.IsNotNull(tilePrefab, "El prefab 'Tile' no se pudo cargar desde Resources.");

        // Asignar el prefab a boardManager
        boardManager.tilePrefab = tilePrefab;

        //GameObject tilePrefab = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Tile"));
        //tilePrefab = tilePrefabOj.GetComponent<>;
        // Inicializar un objeto BoardManager.
        //GameObject boardManagerObj = new GameObject();
           // Object.Instantiate(Resources.Load<GameObject>("Prefabs/Tile"));
        //boardManager = boardManagerObj.AddComponent<BoardManager>();
        //boardManager.tilePrefab = tilePrefab;

    }

    [UnityTest]
    public IEnumerator TestMatchThree()
    {
        
        //tilePrefab = new GameObject();  // Crear un prefab vacío solo para la prueba
        //boardManager.tilePrefab = tilePrefab;
        
        // Crear un tablero de prueba con tamaño 3x3
        boardManager.xSize = 3;
        boardManager.ySize = 3;
        // Crear fichas en un patrón horizontal con 3 fichas iguales en la fila 0
        string[,] charGrid = {
            { "R", "R", "R" },
            { "G", "Y", "P" },
            { "B", "G", "M" }
        };

        boardManager.InitializeBoard(charGrid);
        // Comprobar si la fila superior (0) tiene una combinación de tres fichas "R"
        yield return new WaitForSeconds(0.1f);
        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));
        Assert.AreEqual(3, matches.Count, "No se detectó la combinación de tres fichas.");
    }

    [UnityTest]
    public IEnumerator TestMatchFour()
    {
        // Crear un tablero de prueba con tamaño 3x3
        boardManager.xSize = 4;
        boardManager.ySize = 3;

        // Crear un patrón donde haya una fila con 4 fichas iguales
        string[,] charGrid = {
            { "R", "R", "R", "R" },
            { "G", "Y", "P", "P" },
            { "B", "G", "M", "P" }
        };
        boardManager.InitializeBoard(charGrid);
        yield return new WaitForSeconds(0.1f);
        
        // Comprobar si la fila 0 tiene una combinación de cuatro fichas "R"
        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));
        Assert.AreEqual(4, matches.Count, "No se detectó la combinación de cuatro fichas.");
    }

    [UnityTest]
    public IEnumerator TestNoMatchesAvailable()
    {
        // Crear un tablero de prueba con tamaño 3x3
        boardManager.xSize = 3;
        boardManager.ySize = 3;
        // Crear un tablero donde no haya combinaciones
        string[,] charGrid = {
            { "R", "G", "B" },
            { "Y", "P", "M" },
            { "G", "B", "Y" }
        };
        boardManager.InitializeBoard(charGrid);
        yield return new WaitForSeconds(0.1f);

        // Comprobar que no hay ninguna combinación
        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));
        Assert.AreEqual(0, matches.Count, "Se detectaron combinaciones cuando no deberían haberlas.");
    }
}
