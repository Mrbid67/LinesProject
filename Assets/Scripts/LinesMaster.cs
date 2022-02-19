using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinesMaster : MonoBehaviour
{

    public Vector3 worldPosition;

    //LISTAS Y ARRAYS
    public List<Vector2> points;
    public List<Vector2> calculatedPoints;
    public float[,] pointsMatrix;

    //PUNTOS
    public int size;
    public GameObject size1, size2, size3, size4;
    public List<GameObject> spritesPuntos = new List<GameObject>();



    //UI
    public Canvas canvasLineas;
    public Canvas canvasTransform;
    public TMP_Text coordText;
    public Slider sizeSlider;
    public TMP_Dropdown AlgDropdown;

    public TMP_InputField inputTrasX;
    public TMP_InputField inputTrasY;
    public TMP_InputField inputFact;
    public TMP_InputField inputDeg;
    public TMP_InputField inputCizX;
    public TMP_InputField inputCizY;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canvasLineas.enabled == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            coordText.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x + .1f, Camera.main.ScreenToWorldPoint(mousePos).y - .35f, 0);
            worldPosition = mousePos;
            worldPosition = new Vector3(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);
            worldPosition.y -= (Screen.height / 2);
            worldPosition.x -= (Screen.width / 2);
            coordText.SetText(worldPosition.x + "/" + worldPosition.y);

            if (Input.GetMouseButtonDown(0))
            {
                if ((worldPosition.x >= -960 && worldPosition.x <= 960) && (worldPosition.y >= -365 && worldPosition.y <= 450))
                {
                    points.Add(worldPosition);
                }
                if (points.Count == 2)
                {
                    Debug.Log("Pos 1= " + points[0] + " Pos 2= " + points[1]);
                    if (AlgDropdown.value == 0)
                    {
                        SlopeIntercept(points);
                    }
                    else if (AlgDropdown.value == 1)
                    {
                        FixedSlopeIntercept(points);
                    }
                    else if (AlgDropdown.value == 2)
                    {
                        DigitalDifferentialAnalyzer(points);
                    }
                    else if (AlgDropdown.value == 3)
                    {
                        Bresenham(points);
                    }
                    else if (AlgDropdown.value == 4)
                    {
                        BresenhamFixed(points);
                    }
                    points.Clear();
                }
            }
            else if (canvasTransform.enabled)
            {


            }
        }
    }

    public void GetTrasValue()
    {
        int x = 0;
        int y = 0;

        bool parsedx = false;
        bool parsedy = false;

        if (inputTrasX.text == "")
        {
            x = 0;
        }
        if (inputTrasY.text == "")
        {
            y = 0;
        }

        parsedx = int.TryParse(inputTrasX.text, out x);

        parsedy = int.TryParse(inputTrasY.text, out y);

        Debug.Log("X:" + x + " Y:" + y);

        if (calculatedPoints != null /*&& parsedx && parsedy*/)
        {
            Traslacion(x, y);
        }
    }

    public void GetCizValue()
    {
        int x = 0;
        int y = 0;

        bool parsedx = false;
        bool parsedy = false;

        if (inputCizX.text == "")
        {
            x = 0;
        }
        if (inputCizY.text == "")
        {
            y = 0;
        }

        parsedx = int.TryParse(inputCizX.text, out x);

        parsedy = int.TryParse(inputCizY.text, out y);

        Debug.Log("X:" + x + " Y:" + y);

        if (calculatedPoints != null /*&& parsedx && parsedy*/)
        {
            Cizalla(x, y);
        }
    }

    public void GetEscValue()
    {
        float value;
        bool parsedValue = false;

        if (inputFact.text == "")
        {
            value = 1;
        }
        else
        {
            parsedValue = float.TryParse(inputFact.text, out value);
        }

        if (calculatedPoints != null)
        {
            Debug.Log(value);
            Escalado(value);
        }
    }

    public void GetRotValue()
    {
        int value;
        bool parsedValue = false;

        if (inputDeg.text == "")
        {
            value = 0;
        }
        else
        {
            parsedValue = int.TryParse(inputDeg.text, out value);
        }

        if (calculatedPoints != null)
        {
            Debug.Log(value);
            Rotacion(value);
        }
    }
    public void SlopeIntercept(List<Vector2> points)
    {
        Debug.Log("SlopeIntercept");

        int y = (int)points[0].y;
        int x = (int)points[0].x;
        float m = (points[1].y - points[0].y) / (points[1].x - points[0].x);
        float b = points[0].y - (m * points[0].x);

        while (x <= points[1].x)
        {
            calculatedPoints.Add(new Vector2(x, y));
            x++;
            y = Mathf.RoundToInt((m * x) + b);
        }

        Debug.Log("Calcualtions done!");
        Draw();
    }

    public void FixedSlopeIntercept(List<Vector2> points)
    {
        Debug.Log("FixedSlopeIntercept");

        //Second quadrant mod
        if (points[1].x < points[0].x)
        {
            Vector2 aux = points[0];
            points[0] = points[1];
            points[1] = aux;
        }

        int y = (int)points[0].y;
        int x = (int)points[0].x;
        float m = (points[1].y - points[0].y) / (points[1].x - points[0].x);
        float b = points[0].y - (m * points[0].x);

        //mod 2 V Lines
        if (points[0].x == points[1].x)
        {

            while (y < points[1].y)
            {
                y++;
                calculatedPoints.Add(new Vector2(x, y));
            }
        }

        else if (Mathf.Abs(m) > 1)
        {
            if (points[0].y < points[1].y)
            {
                y = (int)points[0].y;

                //Third mod
                while (y <= points[1].y)
                {
                    calculatedPoints.Add(new Vector2(x, y));
                    y++;
                    x = Mathf.RoundToInt(((float)y - b) / m);
                }
            }
            else
            {
                y = (int)points[1].y;
                //Third mod
                while (y <= points[0].y)
                {
                    calculatedPoints.Add(new Vector2(x, y));
                    y++;
                    x = Mathf.RoundToInt(((float)y - b) / m);
                }
            }
        }
        else
        {
            while (x <= points[1].x)
            {
                calculatedPoints.Add(new Vector2(x, y));
                x++;
                y = Mathf.RoundToInt((m * x) + b);
            }
        }

        Debug.Log("Calcualtions done!");
        Draw();
    }
    public void DigitalDifferentialAnalyzer(List<Vector2> points)
    {
        Debug.Log("DDA");

        float dx = (int)points[1].x - (int)points[0].x;
        float dy = (int)points[1].y - (int)points[0].y;

        float M;

        if (Mathf.Abs(dx) >= Mathf.Abs(dy))
        {
            M = Mathf.Abs(dx);
        }
        else
        {
            M = Mathf.Abs(dy);
        }

        float dxAux = dx / M;
        float dyAux = dy / M;

        float x = points[0].x + .5f;
        float y = points[0].y + .5f;

        float i = 0;

        while (i <= M)
        {
            calculatedPoints.Add(new Vector2(Mathf.Floor(x), Mathf.Floor(y)));
            x = x + dxAux;
            y = y + dyAux;
            i++;
        }

        Debug.Log("Calcualtions done!");
        Draw();
    }

    public void Bresenham(List<Vector2> points)
    {
        Debug.Log("Bresenham");


        int y = (int)points[0].y;
        float dx = (int)points[1].x - (int)points[0].x;
        float dy = (int)points[1].y - (int)points[0].y;
        float e = 2 * dy - dx;

        for (int x = (int)points[0].x; x < (int)points[1].x; x++)
        {
            calculatedPoints.Add(new Vector2(x, y));
            if (e > 0)
            {
                y++;
                e = e - 2 * dx;
            }
            e = e + 2 * dy;
        }

        Debug.Log("Calcualtions done!");
        Draw();
    }

    public void BresenhamFixed(List<Vector2> points)
    {
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
                BresenhamLow(points);
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
                BresenhamHigh(points);
            }
        }
    }

    public void BresenhamLow(List<Vector2> points)
    {
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
        Draw();
    }

    public void BresenhamHigh(List<Vector2> points)
    {
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
        Draw();
    }



    public void Draw()
    {

        Vector3 position;
        GameObject sizeG;
        size = (int)sizeSlider.value;
        switch (size)
        {
            case 1:
                sizeG = size1;
                break;
            case 2:
                sizeG = size2;
                break;
            case 3:
                sizeG = size3;
                break;
            case 4:
                sizeG = size4;
                break;
            default:
                sizeG = size1;
                break;
        }

        for (int i = 0; i < calculatedPoints.Count; i++)
        {
            position = Camera.main.ScreenToWorldPoint(new Vector3(calculatedPoints[i].x + (Screen.width / 2), calculatedPoints[i].y + (Screen.height / 2), 0));
            position.z = 0;
            GameObject newG = Instantiate(sizeG, position, Quaternion.identity);
            spritesPuntos.Add(newG);
        }

    }
    public void GetMatrix()
    {
        pointsMatrix = new float[calculatedPoints.Count, 2];

        for (int i = 0; i < calculatedPoints.Count; i++)
        {
            pointsMatrix[i, 0] = calculatedPoints[i].x;
            pointsMatrix[i, 1] = calculatedPoints[i].y;
        }
        Debug.Log(pointsMatrix.Length);
    }

    public void Traslacion(int trasX, int trasY)
    {
        int numPoints = calculatedPoints.Count;
        DestroyLines();

        for (int i = 0; i < numPoints; ++i)
        {
            pointsMatrix[i, 0] += trasX;
            pointsMatrix[i, 1] += trasY;

            calculatedPoints.Add(new Vector2(pointsMatrix[i, 0], pointsMatrix[i, 1]));
        }

        Draw();
    }


    public void Escalado(float value)
    {
        int numPoints = calculatedPoints.Count;
        DestroyLines();

        for (int i = 0; i < numPoints; ++i)
        {
            pointsMatrix[i, 0] /= value;
            pointsMatrix[i, 1] /= value;

            calculatedPoints.Add(new Vector2(pointsMatrix[i, 0], pointsMatrix[i, 1]));
        }

        Draw();
    }
    public void Rotacion(int value)
    {
        float v;
        int numPoints = calculatedPoints.Count;
        float calcX, calcY;
        DestroyLines();

        v = Mathf.Deg2Rad * value;

        for (int i = 0; i < numPoints; ++i)
        {
            calcX = pointsMatrix[i, 0] * Mathf.Cos(v) - (pointsMatrix[i, 1] * Mathf.Sin(v));
            calcY = pointsMatrix[i, 0] * Mathf.Sin(v) + (pointsMatrix[i, 1] * Mathf.Cos(v));

            calculatedPoints.Add(new Vector2(calcX, calcY));

            pointsMatrix[i, 0] = calcX;
            pointsMatrix[i, 1] = calcY;
        }

        Draw();
    }

    public void Cizalla(int x, int y)
    {

        int numPoints = calculatedPoints.Count;

        float calcX, calcY;
        float cX = x;
        float cY = y;

        DestroyLines();

        for (int i = 0; i < numPoints; ++i)
        {
            calcX = pointsMatrix[i, 0] + (pointsMatrix[i, 1] * (cX / 10));
            calcY = (pointsMatrix[i, 0] * (cY / 10)) + pointsMatrix[i, 1];

            calculatedPoints.Add(new Vector2(calcX, calcY));

            pointsMatrix[i, 0] = calcX;
            pointsMatrix[i, 1] = calcY;
        }

        Draw();
    }

    public void ReflexX() {
        int numPoints = calculatedPoints.Count;
        DestroyLines();

        for (int i = 0; i < numPoints; ++i)
        {
            pointsMatrix[i, 1] = -pointsMatrix[i, 1];
            calculatedPoints.Add(new Vector2(pointsMatrix[i, 0], pointsMatrix[i, 1]));
        }

        Draw();
    }

    public void ReflexY()
    {
        int numPoints = calculatedPoints.Count;
        DestroyLines();

        for (int i = 0; i < numPoints; ++i)
        {
            pointsMatrix[i, 0] = -pointsMatrix[i, 0];
            calculatedPoints.Add(new Vector2(pointsMatrix[i, 0], pointsMatrix[i, 1]));
        }

        Draw();
    }
    public void DestroyLines()
    {
        calculatedPoints.Clear();
        foreach (GameObject g in spritesPuntos)
        {
            Destroy(g);
        }
        points.Clear();
        spritesPuntos.Clear();
    }

    public void Quit()
    {
        Application.Quit();
    }
}

