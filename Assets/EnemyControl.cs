using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    NavMeshAgent agent;

    float distanceToTarget;
    bool goingToSafe = false; // Jos tosi, niin vihollinen siirtyy kohti punaista m�kki�, eik� se v�lit� muista komennoista.
    Vector3 myPosition; // Vihollisen sijainti.
    Vector3 homePosition; // Vihollisen aloitussijainti.
    Vector3 playerPosition; // Pelaajan sijainti.

    [SerializeField]
    LayerMask layerMask; // Sein�t ja lattiat, jotka est�v�t n�kyvyyden.
    [SerializeField]
    Transform target; // Pelaaja mit� seurataan.
    [SerializeField]
    Transform safePlace;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        homePosition = transform.position;
    }

    // Vihollinen etsii pelaajaa tietylt� alueelta, jos pelaaja on alueella niin sen on my�s n�ht�v� pelaaja.
    void Update()
    {
       if(DistanceToPlayer())
        {
            if(SeePlayer())
            {
                if(!goingToSafe)
                {
                    agent.SetDestination(playerPosition);
                }
            }
            else
            {
                BackToSpawn();
            }
        }
       else
        {
            BackToSpawn();
        }
    }

    // Et�isyys pelaajaan.
    private bool DistanceToPlayer()
    {
        myPosition = agent.transform.position;
        playerPosition = target.transform.position;

        distanceToTarget = Vector3.Distance(myPosition, playerPosition);

        // Et�isyys milloin jahdataan pelaajaa.
        if(5 <= distanceToTarget && distanceToTarget < 10 )
        {
            return true;
        }

        // Jos et�isyys on tarpeeksi pieni, niin siirryt��n turvalliseen punaiseen m�kkiin.
        if(distanceToTarget < 5)
        {
            if(SeePlayer() )
            {
                GoToSafePlace();
            }
        }

        return false;
    }

    // Yritt�� piirt�� janan itsest��n pelaajaan, toimii silmin�. Unityssa piirret��n debug janat.
    private bool SeePlayer()
    {
        if(!Physics.Linecast(agent.transform.position, target.transform.position, layerMask))
        {
            Debug.DrawLine(myPosition, playerPosition, Color.blue, 2.0f); // Debuggailua.
            return true;
        }
        else
        {
            Debug.DrawLine(myPosition, playerPosition, Color.red, 2.0f); // Debuggailua.
        }
        return false;
    }

    // Palautetaan viholllinen mist� l�hti liikkeelle.
    private void BackToSpawn()
    {
        if(!goingToSafe)
        {
            agent.ResetPath(); // Pys�ytet��n vihollinen.
            agent.SetDestination(homePosition); // Palautetaan mist� l�hti liikkeelle.
        }
    }

    // Siirryt��n punaiseen m�kkiin jos pelaaja on liian l�hell�.
    private void GoToSafePlace()
    {
        goingToSafe = true;
        agent.SetDestination(safePlace.transform.position);
        Invoke("GoToSafePlaceHelper", 6.0f);
    }

    private void GoToSafePlaceHelper()
    {
        goingToSafe = false;
    }
}
