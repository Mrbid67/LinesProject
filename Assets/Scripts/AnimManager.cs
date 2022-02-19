using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public List<GameObject> spritesPuntosTop = new List<GameObject>();
    public List<GameObject> spritesPuntosBot = new List<GameObject>();
    public List<GameObject> spritesPuntosL = new List<GameObject>();
    public List<GameObject> spritesPuntosR = new List<GameObject>();
    public List<GameObject> spritesPuntosB1 = new List<GameObject>();
    public List<GameObject> spritesPuntosB2 = new List<GameObject>();
    public List<GameObject> spritesPuntosC = new List<GameObject>();
    public List<GameObject> spritesPuntosP1 = new List<GameObject>();
    public List<GameObject> spritesPuntosP2 = new List<GameObject>();
    public Canvas canvasAnim;

    public GameObject sizeG;
    public float[,] pointsMatrix;

    //Puntos de inicio y fin de las partes del dibujo

    //Cabeza
    public List<Vector2> pointsCabezaTop = new List<Vector2>();
    public List<Vector2> pointsCabezaBot = new List<Vector2>();
    public List<Vector2> pointsCabezaLeft = new List<Vector2>();
    public List<Vector2> pointsCabezaRight = new List<Vector2>();

    //cuerpo
    public List<Vector2> pointsCuerpo = new List<Vector2>();
    public List<Vector2> pointsBrazo1 = new List<Vector2>();
    public List<Vector2> pointsBrazo2 = new List<Vector2>();
    public List<Vector2> pointsPierna1 = new List<Vector2>();
    public List<Vector2> pointsPierna2 = new List<Vector2>();

    //Listas con todos los puntos calculados
    public List<Vector2> calculatedPointsTop = new List<Vector2>();
    public List<Vector2> calculatedPointsBot = new List<Vector2>();
    public List<Vector2> calculatedPointsLeft = new List<Vector2>();
    public List<Vector2> calculatedPointsRight = new List<Vector2>();

    public List<Vector2> calculatedPointsCuerpo = new List<Vector2>();
    public List<Vector2> calculatedPointsBrazo1 = new List<Vector2>();
    public List<Vector2> calculatedPointsBrazo2 = new List<Vector2>();
    public List<Vector2> calculatedPointsPierna1 = new List<Vector2>();
    public List<Vector2> calculatedPointsPierna2 = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void anim()
    {
        StartCoroutine(Wait());
    }


    public void generateMonigote()
    {
        //Parte arriba cabeza
        pointsCabezaTop.Add(new Vector2(20, 200));
        pointsCabezaTop.Add(new Vector2(70, 200));

        calculatedPointsTop = BresenhamFixed(pointsCabezaTop);
        Draw(calculatedPointsTop, spritesPuntosTop);

        //Parte abajo cabeza
        pointsCabezaBot.Add(new Vector2(20, 150));
        pointsCabezaBot.Add(new Vector2(70, 150));

        calculatedPointsBot = BresenhamFixed(pointsCabezaBot);
        Draw(calculatedPointsBot, spritesPuntosBot);

        //Parte drch cabeza
        pointsCabezaRight.Add(new Vector2(70, 150));
        pointsCabezaRight.Add(new Vector2(70, 200));

        calculatedPointsRight = BresenhamFixed(pointsCabezaRight);
        Draw(calculatedPointsRight, spritesPuntosR);

        //Parte izq cabeza
        pointsCabezaLeft.Add(new Vector2(20, 150));
        pointsCabezaLeft.Add(new Vector2(20, 200));

        calculatedPointsLeft = BresenhamFixed(pointsCabezaLeft);
        Draw(calculatedPointsLeft, spritesPuntosR);

        //cuerpo
        pointsCuerpo.Add(new Vector2(45, 75));
        pointsCuerpo.Add(new Vector2(45, 150));

        calculatedPointsCuerpo = BresenhamFixed(pointsCuerpo);
        Draw(calculatedPointsCuerpo, spritesPuntosC);

        //Brazo izq
        pointsBrazo1.Add(new Vector2(0, 120));
        pointsBrazo1.Add(new Vector2(45, 135));

        calculatedPointsBrazo1 = BresenhamFixed(pointsBrazo1);
        Draw(calculatedPointsBrazo1, spritesPuntosB1);

        //Brazo drch
        pointsBrazo2.Add(new Vector2(45, 135));
        pointsBrazo2.Add(new Vector2(90, 120));

        calculatedPointsBrazo2 = BresenhamFixed(pointsBrazo2);
        Draw(calculatedPointsBrazo2, spritesPuntosB2);

        //Pierna izq
        pointsPierna1.Add(new Vector2(0, 0));
        pointsPierna1.Add(new Vector2(45, 75));

        calculatedPointsPierna1 = BresenhamFixed(pointsPierna1);
        Draw(calculatedPointsPierna1, spritesPuntosP1);

        //Pierna drch
        pointsPierna2.Add(new Vector2(85, 0));
        pointsPierna2.Add(new Vector2(45, 75));

        calculatedPointsPierna2 = BresenhamFixed(pointsPierna2);
        Draw(calculatedPointsPierna2, spritesPuntosP2);

    }

    public List<Vector2> BresenhamFixed(List<Vector2> points)
    {
        List<Vector2> calcPoints = null;
        if (Mathf.Abs(points[1].y - points[0].y) < Mathf.Abs(points[1].x - points[0].x))
        {
            if (points[0].x > points[1].x)
            {
                Vector2 aux = points[0];
                points[0] = points[1];
                points[1] = aux;
                BresenhamLow(points);
            }
            else
            {
                calcPoints = BresenhamLow(points);
            }
        }
        else
        {
            if (points[0].y > points[1].y)
            {
                Vector2 aux = points[0];
                points[0] = points[1];
                points[1] = aux;
                BresenhamHigh(points);
            }
            else
            {
                calcPoints = BresenhamHigh(points);
            }
        }
        return calcPoints;
    }

    public List<Vector2> BresenhamLow(List<Vector2> points)
    {
        List<Vector2> calculatedPoints = new List<Vector2>();
        int dx = (int)points[1].x - (int)points[0].x;
        int dy = (int)points[1].y - (int)points[0].y;

        int yi = 1;
        if (dy < 0)
        {
            yi = -1;
            dy = -dy;
        }

        int D = (2 * dy) - dx;
        int y = (int)points[0].y;

        for (int x = (int)points[0].x; x < points[1].x; x++)
        {
            calculatedPoints.Add(new Vector2(x, y));
            if (D > 0)
            {
                y = y + yi;
                D = D + (2 * (dy - dx));
            }
            else
            {
                D = D + 2 * dy;
            }
        }

        Debug.Log("Calcualtions done!");
        return calculatedPoints;
    }

    public List<Vector2> BresenhamHigh(List<Vector2> points)
    {
        List<Vector2> calculatedPoints = new List<Vector2>();

        int dx = (int)points[1].x - (int)points[0].x;
        int dy = (int)points[1].y - (int)points[0].y;

        int xi = 1;
        if (dx < 0)
        {
            xi = -1;
            dx = -dx;
        }

        int D = (2 * dx) - dy;
        int x = (int)points[0].x;

        for (int y = (int)points[0].y; y < points[1].y; y++)
        {
            calculatedPoints.Add(new Vector2(x, y));
            if (D > 0)
            {
                x = x + xi;
                D = D + (2 * (dx - dy));
            }
            else
            {
                D = D + 2 * dx;
            }
        }

        Debug.Log("Calcualtions done!");
        return calculatedPoints;
    }


    public void GetMatrix(List<Vector2> calculatedPoints)
    {
        pointsMatrix = new float[calculatedPoints.Count, 2];

        for (int i = 0; i < calculatedPoints.Count; i++)
        {
            pointsMatrix[i, 0] = calculatedPoints[i].x;
            pointsMatrix[i, 1] = calculatedPoints[i].y;
        }
    }

    public void Traslacion(int trasX, int trasY, List<Vector2> calculatedPoints, List<GameObject> spritesPuntos)
    {
        int numPoints = calculatedPoints.Count;
        DestroyLine(calculatedPoints, spritesPuntos);

        for (int i = 0; i < numPoints; ++i)
        {
            pointsMatrix[i, 0] += trasX;
            pointsMatrix[i, 1] += trasY;

            calculatedPoints.Add(new Vector2(pointsMatrix[i, 0], pointsMatrix[i, 1]));
        }

        Draw(calculatedPoints, spritesPuntos);
    }
    public void Draw(List<Vector2> calculatedPoints, List<GameObject> spritesPuntos)
    {
        Vector3 position;

        for (int i = 0; i < calculatedPoints.Count; i++)
        {
            position = Camera.main.ScreenToWorldPoint(new Vector3(calculatedPoints[i].x + (Screen.width / 2), calculatedPoints[i].y + (Screen.height / 2), 0));
            position.z = 0;
            GameObject newG = Instantiate(sizeG, position, Quaternion.identity);
            spritesPuntos.Add(newG);
        }

    }
    public void DestroyLine(List<Vector2> calculatedPoints, List<GameObject> spritesPuntos)
    {
        calculatedPoints.Clear();
        foreach (GameObject g in spritesPuntos)
        {
            Destroy(g);
        }
        spritesPuntos.Clear();
    }

    IEnumerator Wait()
    {
        //Cabeza drch
        while (calculatedPointsTop[1].x != 45)
        {
            GetMatrix(calculatedPointsTop);
            Traslacion(1, 0, calculatedPointsTop, spritesPuntosTop);

            GetMatrix(calculatedPointsBot);
            Traslacion(1, 0, calculatedPointsBot, spritesPuntosBot);

            GetMatrix(calculatedPointsLeft);
            Traslacion(1, 0, calculatedPointsLeft, spritesPuntosL);


            GetMatrix(calculatedPointsRight);
            Traslacion(1, 0, calculatedPointsRight, spritesPuntosR);
            yield return new WaitForSeconds(0.025f);
        }

        //cabeza izq
        while (calculatedPointsTop[1].x != 0)
        {

            GetMatrix(calculatedPointsTop);
            Traslacion(-1, 0, calculatedPointsTop, spritesPuntosTop);

            GetMatrix(calculatedPointsBot);
            Traslacion(-1, 0, calculatedPointsBot, spritesPuntosBot);

            GetMatrix(calculatedPointsLeft);
            Traslacion(-1, 0, calculatedPointsLeft, spritesPuntosL);


            GetMatrix(calculatedPointsRight);
            Traslacion(-1, 0, calculatedPointsRight, spritesPuntosR);
            yield return new WaitForSeconds(0.025f);
        }

        //Cabeza centro
        while (calculatedPointsTop[1].x != 20)
        {

            GetMatrix(calculatedPointsTop);
            Traslacion(1, 0, calculatedPointsTop, spritesPuntosTop);

            GetMatrix(calculatedPointsBot);
            Traslacion(1, 0, calculatedPointsBot, spritesPuntosBot);

            GetMatrix(calculatedPointsLeft);
            Traslacion(1, 0, calculatedPointsLeft, spritesPuntosL);


            GetMatrix(calculatedPointsRight);
            Traslacion(1, 0, calculatedPointsRight, spritesPuntosR);
            yield return new WaitForSeconds(0.025f);
        }

        //brazos arriba
        while (calculatedPointsBrazo1[1].y != 130)
        {

            GetMatrix(calculatedPointsBrazo1);
            Traslacion(0, 1, calculatedPointsBrazo1, spritesPuntosB1);

            GetMatrix(calculatedPointsBrazo2);
            Traslacion(0, 1, calculatedPointsBrazo2, spritesPuntosB2);

            yield return new WaitForSeconds(0.025f);
        }


        //brazos arriba
        while (calculatedPointsBrazo1[1].y != 120)
        {

            GetMatrix(calculatedPointsBrazo1);
            Traslacion(0, -1, calculatedPointsBrazo1, spritesPuntosB1);

            GetMatrix(calculatedPointsBrazo2);
            Traslacion(0, -1, calculatedPointsBrazo2, spritesPuntosB2);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        while (calculatedPointsTop[1].x != 1000)
        {
            GetMatrix(calculatedPointsTop);
            Traslacion(1, 0, calculatedPointsTop, spritesPuntosTop);

            GetMatrix(calculatedPointsBot);
            Traslacion(1, 0, calculatedPointsBot, spritesPuntosBot);

            GetMatrix(calculatedPointsLeft);
            Traslacion(1, 0, calculatedPointsLeft, spritesPuntosL);


            GetMatrix(calculatedPointsRight);
            Traslacion(1, 0, calculatedPointsRight, spritesPuntosR);

            GetMatrix(calculatedPointsBrazo1);
            Traslacion(1, 0, calculatedPointsBrazo1, spritesPuntosB1);

            GetMatrix(calculatedPointsBrazo2);
            Traslacion(1, 0, calculatedPointsBrazo2, spritesPuntosB2);

            GetMatrix(calculatedPointsPierna1);
            Traslacion(1, 0, calculatedPointsPierna1, spritesPuntosP1);

            GetMatrix(calculatedPointsPierna2);
            Traslacion(1, 0, calculatedPointsPierna2, spritesPuntosP2);

            GetMatrix(calculatedPointsCuerpo);
            Traslacion(1, 0, calculatedPointsCuerpo, spritesPuntosC);

            yield return new WaitForSeconds(0.0005f);
        }
    }
}
