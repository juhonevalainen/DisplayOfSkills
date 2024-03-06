using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    NavMeshAgent agent;

    float distanceToTarget;
    bool goingToSafe = false; // Jos tosi, niin vihollinen siirtyy kohti punaista mökkiä, eikä se välitä muista komennoista.
    Vector3 myPosition; // Vihollisen sijainti.
    Vector3 homePosition; // Vihollisen aloitussijainti.
    Vector3 playerPosition; // Pelaajan sijainti.

    [SerializeField]
    LayerMask layerMask; // Seinät ja lattiat, jotka estävät näkyvyyden.
    [SerializeField]
    Transform target; // Pelaaja mitä seurataan.
    [SerializeField]
    Transform safePlace;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        homePosition = transform.position;
    }

    // Vihollinen etsii pelaajaa tietyltä alueelta, jos pelaaja on alueella niin sen on myös nähtävä pelaaja.
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

    // Etäisyys pelaajaan.
    private bool DistanceToPlayer()
    {
        myPosition = agent.transform.position;
        playerPosition = target.transform.position;

        distanceToTarget = Vector3.Distance(myPosition, playerPosition);

        // Etäisyys milloin jahdataan pelaajaa.
        if(5 <= distanceToTarget && distanceToTarget < 10 )
        {
            return true;
        }

        // Jos etäisyys on tarpeeksi pieni, niin siirrytään turvalliseen punaiseen mökkiin.
        if(distanceToTarget < 5)
        {
            if(SeePlayer() )
            {
                GoToSafePlace();
            }
        }

        return false;
    }

    // Yrittää piirtää janan itsestään pelaajaan, toimii silminä. Unityssa piirretään debug janat.
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

    // Palautetaan viholllinen mistä lähti liikkeelle.
    private void BackToSpawn()
    {
        if(!goingToSafe)
        {
            agent.ResetPath(); // Pysäytetään vihollinen.
            agent.SetDestination(homePosition); // Palautetaan mistä lähti liikkeelle.
        }
    }

    // Siirrytään punaiseen mökkiin jos pelaaja on liian lähellä.
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
