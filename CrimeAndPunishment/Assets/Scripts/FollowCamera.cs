using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Mesh mesh;
    public Material material;
    // получаем маску, которая затрагивает только слой Building
    static int layerMaskOnlyBuilding;

    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottomLimit;
    [SerializeField]
    float upperLimit;

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
        hits = Physics.CapsuleCastAll( player.position, transform.position, CapsuleCastRadius, transform.forward, Vector3.Distance(transform.position, player.position), layerMaskOnlyBuilding);
        hits = hits.Where(val => val.collider.gameObject.transform.position.z < player.transform.position.z).ToArray();
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
        DrowCapsule();
        DrowCameraBorder();
    }

    void DrowCapsule()
    {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(player.transform.position, CapsuleCastRadius);
            Gizmos.DrawSphere(transform.position, CapsuleCastRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(transform.position.x + CapsuleCastRadius, transform.position.y, transform.position.z), new Vector3(player.transform.position.x + CapsuleCastRadius, player.transform.position.y, player.transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + CapsuleCastRadius, transform.position.z), new Vector3(player.transform.position.x, player.transform.position.y + CapsuleCastRadius, player.transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x - CapsuleCastRadius, transform.position.y, transform.position.z), new Vector3(player.transform.position.x - CapsuleCastRadius, player.transform.position.y, player.transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - CapsuleCastRadius, transform.position.z), new Vector3(player.transform.position.x, player.transform.position.y - CapsuleCastRadius, player.transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + CapsuleCastRadius), new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + CapsuleCastRadius));
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z - CapsuleCastRadius), new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - CapsuleCastRadius));
    }
    void DrowCameraBorder()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(leftLimit,10,upperLimit), new Vector3(rightLimit,10,upperLimit));
        Gizmos.DrawLine(new Vector3(leftLimit,10,bottomLimit), new Vector3(rightLimit,10,bottomLimit));
        Gizmos.DrawLine(new Vector3(leftLimit,10,upperLimit), new Vector3(leftLimit, 10, bottomLimit));
        Gizmos.DrawLine(new Vector3(rightLimit, 10,upperLimit), new Vector3(rightLimit,10, bottomLimit));
    }
    //камера следует за игроком
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * smooth);
        //границы для камеры
        transform.position = new Vector3
            (
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            transform.position.y,
            Mathf.Clamp(transform.position.z, bottomLimit, upperLimit)
            );
    }
}