using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public float smooth = 5.0f;
    public Vector3 offset = new Vector3(0, 13, -7);

    public float CapsuleCastRadius = 0.5f;
    private List<InterferingObject> interferingObjects = new List<InterferingObject>();

    // получаем маску, которая затрагивает только слой Building
    static int layerMaskOnlyBuilding;
   
    private void Awake()
    {
        layerMaskOnlyBuilding = 1 << LayerMask.NameToLayer("Building");
    }
    void Update()
    {
        RaycastHit[] hits;
        hits = Physics.CapsuleCastAll(transform.position, player.position, CapsuleCastRadius, transform.forward, Mathf.Infinity, layerMaskOnlyBuilding);
        print(hits.Length);
        if (interferingObjects.Count > 0 && interferingObjects != null)
        {
            foreach (InterferingObject io in interferingObjects)
            {
                io.Status2Old();
                print(io.status);
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
                        print("checked");
                        io.Status2Checked();
                        inList = true;
                        break;
                    }
                }
                if (!inList)
                {
                    item.collider.gameObject.GetComponent<Building>().MakeTransparent();
                    interferingObjects.Add(new InterferingObject(item.collider.gameObject));
                    print("добавили нового");
                }
            }
        }
        if (interferingObjects.Count > 0 && interferingObjects != null)
        {
            print("interferingObjects.Count " + interferingObjects.Count);
            foreach (InterferingObject io in interferingObjects)
            {
                print("вошли в форыч");
                print("статус в форыч " + io.status);
                print(io.status == "old");
                if (io.status == "old")
                {
                    print("old for remove");
                    io.interferingObject.GetComponent<Building>().MakeItOpaque();
                    interferingObjects.Remove(io);
                }
            }
        }
        //item.collider.gameObject.GetComponent<Building>().MakeTransparent();
        /*
        всех что были, сделать старыми.........
        если колво объектов больше 0...........
        проверить для каждого в полученом массиве, есть ли он в списке
        -если да то перевести в чекед
        -если нет, то добавить
        
        все старые превратить обратно в видимое
        все новые сделать невидимыми
         */
    }
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) ;
        Gizmos.DrawRay(transform.position * 20, player.position );
    }
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * smooth);
    }

    private struct InterferingObject
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
}