using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestSuite
{
    private BoardManager boardManager;
    private GameObject tilePrefab;
    private List<Sprite> candies;
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

        // Cargar los sprites específicos desde la carpeta "Resources/Sprites"
        candies = new List<Sprite>
        {
            Resources.Load<Sprite>("Sprites/Characters/Red"),
            Resources.Load<Sprite>("Sprites/Characters/Green"),
            Resources.Load<Sprite>("Sprites/Characters/Blue"),
            Resources.Load<Sprite>("Sprites/Characters/Yellow"),
            Resources.Load<Sprite>("Sprites/Characters/Purple"),
            Resources.Load<Sprite>("Sprites/Characters/Multi")
        };
        // Verificar que los sprites se cargaron correctamente
        foreach (var sprite in candies)
        {
            Assert.IsNotNull(sprite, "Uno de los sprites no se pudo cargar correctamente.");
        }
        boardManager.candies = candies;
    }

    [UnityTest]
    public IEnumerator TestMatchFour()
    {
        // Crear un tablero de prueba con tamaño 3x3
        boardManager.xSize = 4;
        boardManager.ySize = 4;

        // Crear un patrón donde haya una fila con 4 fichas iguales
        string[,] charGrid = {
            { "R", "R", "R", "R" },
            { "G", "Y", "P", "P" },
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

    [UnityTest]
    public IEnumerator TestTileShiftAfterClear()
    {
        // Crear un tablero con una combinación de tres fichas en la columna 0
        string[,] charGrid = {
            { "R", "G", "B" },
            { "R", "G", "P" },
            { "R", "B", "M" }
        };
        boardManager.InitializeBoard(charGrid);

        // Buscar la combinación de tres fichas en la columna 0
        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));
        Assert.AreEqual(3, matches.Count, "No se detectó la combinación de tres fichas.");

        // Ejecutar la eliminación de las fichas coincidentes y mover las fichas hacia abajo
        yield return boardManager.ClearMatches(matches);
        
        // Verificar si las fichas se han movido correctamente
        boardManager.ShiftTilesDown();

        // Comprobar que el tablero se ha reorganizado correctamente
        Assert.IsNotNull(boardManager.tiles[0, 2].GetComponent<Tile>().GetCandyType(), "Las fichas no se movieron correctamente hacia abajo.");
    }

    [UnityTest]
    public IEnumerator TestScoreIncrementOnMatch()
    {
        // Crear un tablero con una combinación de tres fichas en la fila 0
        string[,] charGrid = {
            { "R", "R", "R" },
            { "G", "Y", "P" },
            { "B", "G", "M" }
        };
        boardManager.InitializeBoard(charGrid);

        // Iniciar el puntaje a 0
        GUIManager.instance.Score = 0;
        
        // Buscar la combinación de tres fichas en la fila 0
        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));
        Assert.AreEqual(3, matches.Count, "No se detectó la combinación de tres fichas.");

        // Incrementar el puntaje
        yield return boardManager.ClearMatches(matches);

        // Comprobar si el puntaje ha aumentado
        Assert.AreEqual(150, GUIManager.instance.Score, "El puntaje no se incrementó correctamente.");
    }

}
