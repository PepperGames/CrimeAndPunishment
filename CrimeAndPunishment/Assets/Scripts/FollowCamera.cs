using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public float smooth = 5.0f;
    public Vector3 offset = new Vector3(0, 13, -7);

    public float CapsuleCastRadius = 0.5f;
    private float CapsuleCastDistance;
    private List<InterferingObject> interferingObjects = new List<InterferingObject>();

    // получаем маску, которая затрагивает только слой Building
    static int layerMaskOnlyBuilding;

    private void Awake()
    {
        layerMaskOnlyBuilding = 1 << LayerMask.NameToLayer("Building");
    }
    void Update()
    {
        /*
        всех что были, сделать старыми.........
        если колво объектов больше 0...........
        проверить для каждого в полученом массиве, есть ли он в списке
        -если да то перевести в чекед
        -если нет, то добавить
        все старые превратить обратно в видимое
        все новые сделать невидимыми
         */
        RaycastHit[] hits;
        hits = Physics.CapsuleCastAll(transform.position, player.position, CapsuleCastRadius, transform.forward, Vector3.Distance(transform.position, player.position), layerMaskOnlyBuilding);
        if (interferingObjects.Count > 0 && interferingObjects != null)
        {
            foreach (InterferingObject io in interferingObjects)
            {
                io.Status2Old();
            }
        }

        if (hits.Length > 0 && hits != null)
        {
            foreach (RaycastHit item in hits)
            {
                bool inList = false;
                foreach (InterferingObject io in interferingObjects)
                {
                    if (item.collider.gameObject == io.interferingObject)
                    {
                        io.Status2Checked();
                        inList = true;
                        break;
                    }
                }
                if (!inList)
                {
                    item.collider.gameObject.GetComponent<Building>().MakeTransparent();
                    interferingObjects.Add(new InterferingObject(item.collider.gameObject));
                }
            }
        }
        if (interferingObjects.Count > 0 && interferingObjects != null)
        {

            for (int i = 0; i < interferingObjects.Count; i++)
            {
                if (interferingObjects[i].status == "old")
                {
                    interferingObjects[i].interferingObject.GetComponent<Building>().MakeItOpaque();
                    interferingObjects.RemoveAt(i--);
                }
            }
        }
    }
    //вспомогательный класс который содержит обьект который должен быть невидимым
    private class InterferingObject
    {
        public GameObject interferingObject;
        public string status; //новый, старый, только что провереный, new, old, checked

        public InterferingObject(GameObject interferingObject)
        {
            this.interferingObject = interferingObject;
            status = "new";
        }

        public void Status2Old()
        {
            status = "old";
        }

        public void Status2Checked()
        {
            status = "checked";
        }
    }
    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = player.position;
        Gizmos.DrawLine(transform.position, direction);
    }

    //камера следует за игроком
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * smooth);
    }


}