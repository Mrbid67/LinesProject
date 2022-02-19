using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public Canvas canvasLineas;
    public Canvas canvasTransformaciones;
    public Canvas canvasAnim;
    public Canvas canvasFractales;
    public GameObject master;

    // Start is called before the first frame update
    void Start()
    {
        canvasTransformaciones.enabled = false;
        canvasLineas.enabled = true;
        canvasAnim.enabled = false;
        canvasFractales.enabled = false;
    }


    public void showLineas() {
        canvasTransformaciones.enabled = false;
        canvasLineas.enabled = true;
        canvasAnim.enabled = false;
        canvasFractales.enabled = false;
    }

    public void showTransformaciones() {
        canvasTransformaciones.enabled = true;
        canvasLineas.enabled = false;
        canvasAnim.enabled = false;
        canvasFractales.enabled = false;
    }

    public void showAnim()
    {
        canvasTransformaciones.enabled = false;
        canvasLineas.enabled = false;
        canvasAnim.enabled = true;
        canvasFractales.enabled = false;
        master.GetComponent<LinesMaster>().DestroyLines();
    }

    public void showFractals() {
        canvasTransformaciones.enabled = false;
        canvasLineas.enabled = false;
        canvasAnim.enabled = false;
        canvasFractales.enabled = true;
        master.GetComponent<LinesMaster>().DestroyLines();
    }
}
