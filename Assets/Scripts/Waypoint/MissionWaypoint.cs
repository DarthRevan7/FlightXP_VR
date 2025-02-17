using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//Da mettere sulla camera che renderizza la UI.
public class MissionWaypoint : MonoBehaviour
{

    //Immagine del waypoint
    [SerializeField] private Image img;
    //Canvas che contiene il waypoint come immagine
    [SerializeField] private Canvas waypointCanvas;
    //Target del waypoint (nel nostro caso è il toro)
    public Transform target;
    //Testo x distanza in metri
    [SerializeField] private TMP_Text metri;
    //offset del waypoint (eventuale)
    [SerializeField] private Vector3 offset;
    //Riferimento alla camera della UI perché ne abbiamo 2.
    [SerializeField] private Camera cameraUI;
    [SerializeField] private Transform plane;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxAngleRotation = 18f;
    [SerializeField] private float crossProdThreshold = 5f;
    [SerializeField] private Quaternion rotazioneIniziale;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotazioneIniziale = waypointCanvas.GetComponent<RectTransform>().rotation;

    }

    // Update is called once per frame
    void Update()
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        //transform.position = plane.transform.position;
        //transform.rotation = plane.transform.rotation;

        

        if(target != null)
        {
            int distanzaTarget = (int)Vector3.Distance(target.position, plane.transform.position);

            Vector3 direzioneTarget = target.position - plane.transform.position;

            float angolo = Mathf.Acos(Vector3.Dot(target.position.normalized, plane.forward))*180/Mathf.PI;
            

            float angoloLimitato = angolo * maxAngleRotation/180;
            
            Quaternion newRotation;

            if(Vector3.Cross(plane.transform.forward, direzioneTarget).y > 0)
            {
                newRotation = Quaternion.Euler(0,angoloLimitato,0);
                Debug.Log(message: angoloLimitato.ToString());
            }
            else
            {
                newRotation = Quaternion.Euler(0,-angoloLimitato,0);
                Debug.Log(message: (-angoloLimitato).ToString());
            }

            

            waypointCanvas.transform.localRotation = newRotation;



            /*

            Quaternion rotazioneVersoTarget = Quaternion.LookRotation(direzioneTarget);

            float angoloRotazione = Quaternion.Angle(rotazioneIniziale, rotazioneVersoTarget);

            if(angoloRotazione > maxAngleRotation)
            {
                float angoloLimitato = Mathf.Sign(Quaternion.Angle(rotazioneIniziale, rotazioneVersoTarget)) * maxAngleRotation;
                rotazioneVersoTarget = Quaternion.Euler(0, 0, rotazioneIniziale.eulerAngles.z + angoloLimitato);
            }
            
            

            // Verifica che la rotazione non faccia girare la UI dietro la telecamera
            Vector3 directionToCamera = Camera.main.transform.position - waypointCanvas.GetComponent<RectTransform>().position;
            float angleToCamera = Vector3.Angle(waypointCanvas.GetComponent<RectTransform>().forward, directionToCamera);
            if (angleToCamera > 90f)
            {
                 // La UI è dietro la telecamera, la ruotiamo per farla tornare visibile
                rotazioneVersoTarget = Quaternion.Euler(0, rotazioneIniziale.eulerAngles.y, 0); // Mantenere la rotazione iniziale
            }

            // Applica la rotazione al canvas
            waypointCanvas.GetComponent<RectTransform>().rotation = rotazioneVersoTarget;

            /*
            //canvasRectTransform.position = plane.position;
            
            //direzioneTarget.y = 0;

            
            //Prodotto vettoriale tra player e waypoint per definire se il waypoint è a dx o sx
            float crossProd = Vector3.Cross(transform.up,direzioneTarget).y;// /(transform.position.magnitude * direzioneTarget.magnitude));
            Debug.Log(crossProd.ToString());
            

            if(Mathf.Abs(waypointCanvas.transform.rotation.y) < maxAngleRotation)
            {
                if(crossProd > crossProdThreshold)  
                {
                    waypointCanvas.transform.Rotate(Vector3.up * rotationSpeed  * Time.deltaTime);
                }
                else if(crossProd < -crossProdThreshold)
                {
                    waypointCanvas.transform.Rotate(Vector3.up * -rotationSpeed  * Time.deltaTime);
                }
            }
            

            




            /*
            Vector2 pos = cameraUI.WorldToScreenPoint(target.position + offset);

            //Se il waypoint è dietro al giocatore
            if(Vector3.Dot((target.position - transform.position), transform.forward) < 0)
            {
                if(pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x,minX,maxX);
            pos.y = Mathf.Clamp(pos.y,minY,maxY);

            img.transform.position = pos;
            */

            if(Vector3.Dot(plane.transform.forward,direzioneTarget.normalized) < 0)
            {
                distanzaTarget*=-1;
            }
            
            metri.text = distanzaTarget.ToString() + "m";
        }
    }
}
