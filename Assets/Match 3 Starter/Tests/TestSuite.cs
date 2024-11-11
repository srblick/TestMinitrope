using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    private BoardManager boardManager;
    private GameObject tilePrefab;
    private Tile[,] testTiles;

    [SetUp]
    public void Setup()
    {
        // Inicializar un objeto BoardManager vacío.
        GameObject boardManagerObj = new GameObject();
        boardManager = boardManagerObj.AddComponent<BoardManager>();
        
        // Suponiendo que el prefab del "tile" ya está configurado
        //tilePrefab = new GameObject();  // Crear un prefab vacío solo para la prueba
        boardManager.tilePrefab = tilePrefab;
        
        // Crear un tablero de prueba con tamaño 3x3
        testTiles = new Tile[3, 3];
        boardManager.xSize = 3;
        boardManager.ySize = 3;
        
    }

    [Test]
    public void TestMatchThree()
    {
        // Crear fichas en un patrón horizontal con 3 fichas iguales en la fila 0
        string[,] charGrid = {
            { "R", "R", "R" },
            { "G", "Y", "P" },
            { "B", "G", "M" }
        };

        boardManager.InitializeBoard(charGrid);
        // Comprobar si la fila superior (0) tiene una combinación de tres fichas "R"
        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));
        Assert.AreEqual(3, matches.Count, "No se detectó la combinación de tres fichas.");
    }
}
