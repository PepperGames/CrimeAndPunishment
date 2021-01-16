using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent agent;
    // �������� �����, ������� ����������� ������ ���� Player
    static int layerMaskOnlyBuilding;
    // �������� �����, ������� ����������� ��� ����, ����� ���� Player
    private int layerMaskWithoutBuilding;
    private void Awake()
    {
        layerMaskOnlyBuilding = 1 << LayerMask.NameToLayer("Building");
        layerMaskWithoutBuilding = ~layerMaskOnlyBuilding;
    }
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
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMaskWithoutBuilding))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}