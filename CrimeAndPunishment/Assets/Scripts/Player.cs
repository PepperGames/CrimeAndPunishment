using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent agent;

    public LayerMask layerMasForRaycast;
    //// получаем маску, которая затрагивает только слой Player
    //static int layerMaskOnlyBuilding;
    //static int layerMaskOnlyPlayer;
    //// получаем маску, которая затрагивает все слои, кроме слоя Player
    //private int layerMasForRaycast;
    //private void Awake()
    //{
    //    layerMaskOnlyBuilding = 1 << LayerMask.NameToLayer("Building");
    //    layerMaskOnlyPlayer = 1 << LayerMask.NameToLayer("Player");
    //    layerMasForRaycast = ~layerMaskOnlyBuilding;
    //    layerMasForRaycast = ~layerMaskOnlyPlayer;

    //}
    void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMasForRaycast))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}