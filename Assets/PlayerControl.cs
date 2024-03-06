using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    NavMeshAgent agent;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Ohjaaminen on toteutettu vastaavasti kuin Unityn harjoitustehtävissä.
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Luetaan hiiren osoittama paikka jos vasenta nappia on painettu.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        // Sammutetaan peli ESC napista.
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Peli sammui!");
        }
    }
}
