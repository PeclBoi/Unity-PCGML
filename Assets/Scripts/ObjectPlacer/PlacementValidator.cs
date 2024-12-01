using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementValidator : MonoBehaviour
{
    public Collider Collider;
    private RaycastHit boxHit;
    public float margin = 0.2f;

    private float CornerOffsetX => Collider.bounds.size.x / 2 + margin;
    private float ColliderLenghtX => Collider.bounds.size.x;
    private float CornerOffsetZ => Collider.bounds.size.z / 2 + margin;
    private float ColliderLenghtZ => Collider.bounds.size.z;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(transform.position + new Vector3(CornerOffsetX, 0, CornerOffsetZ), 0.2f);
        //Gizmos.DrawSphere(transform.position + new Vector3(-CornerOffsetX, 0, -CornerOffsetZ), 0.2f);

        var corner1 = transform.position + new Vector3(CornerOffsetX, 0, CornerOffsetZ);
        var corner2 = transform.position + new Vector3(-CornerOffsetX, 0, -CornerOffsetZ);


        Ray ray1 = new Ray(corner1, -transform.forward);
        Ray ray1_reflected = ReflectRay(ray1, ray1.GetPoint(ColliderLenghtZ + margin * 2));

        Ray ray2 = new Ray(corner1, -transform.right);
        Ray ray2_reflected = ReflectRay(ray2, ray2.GetPoint(ColliderLenghtX + margin * 2));

        Ray ray3 = new Ray(corner2, transform.forward);
        Ray ray3_reflected = ReflectRay(ray3, ray3.GetPoint(ColliderLenghtZ + margin * 2));

        Ray ray4 = new Ray(corner2, transform.right);
        Ray ray4_reflected = ReflectRay(ray4, ray4.GetPoint(ColliderLenghtX + margin * 2));


        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray1);
        Gizmos.DrawRay(ray2);
        Gizmos.DrawRay(ray3);
        Gizmos.DrawRay(ray4);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray1_reflected);
        Gizmos.DrawRay(ray2_reflected);
        Gizmos.DrawRay(ray3_reflected);
        Gizmos.DrawRay(ray4_reflected);


        Gizmos.color = Color.red;

        ////Check if there has been a hit yet
        if (Physics.BoxCast(transform.position, transform.localScale, -transform.up, out boxHit, transform.rotation, 300f))
        {
            if (boxHit.collider.tag == "Road")
            {
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(transform.position, -transform.up * boxHit.distance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(transform.position + -transform.up * boxHit.distance, transform.localScale);
            }
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, -transform.up * 300f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + -transform.up * 300f, transform.localScale);
        }

    }

    private Ray ReflectRay(Ray ray, Vector3 origin)
    {
        return new Ray()
        {
            origin = origin,
            direction = -ray.direction
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Validate();
        }
    }

    public bool Validate()
    {
        var corner1 = transform.position + new Vector3(CornerOffsetX, 0, CornerOffsetZ);
        var corner2 = transform.position + new Vector3(-CornerOffsetX, 0, -CornerOffsetZ);

        Ray ray1 = new Ray(corner1, -transform.forward);
        Ray ray2 = new Ray(corner1, -transform.right);
        Ray ray3 = new Ray(corner2, transform.forward);
        Ray ray4 = new Ray(corner2, transform.right);

        Ray ray1_reflected = ReflectRay(ray1, ray1.GetPoint(ColliderLenghtZ + margin * 2));
        Ray ray2_reflected = ReflectRay(ray2, ray2.GetPoint(ColliderLenghtX + margin * 2));
        Ray ray3_reflected = ReflectRay(ray3, ray3.GetPoint(ColliderLenghtZ + margin * 2));
        Ray ray4_reflected = ReflectRay(ray4, ray4.GetPoint(ColliderLenghtX + margin * 2));

        RaycastHit hit;

        if (Physics.OverlapSphere(transform.position, 0f).Length > 1)
        {
            return false;
        }
        else if (

            Physics.Raycast(ray1, out hit, ColliderLenghtZ) ||
            Physics.Raycast(ray1_reflected, out hit, ColliderLenghtZ) ||
            Physics.Raycast(ray2, out hit, ColliderLenghtX) ||
            Physics.Raycast(ray2_reflected, out hit, ColliderLenghtX) ||
            Physics.Raycast(ray3, out hit, ColliderLenghtZ) ||
            Physics.Raycast(ray3_reflected, out hit, ColliderLenghtZ) ||
            Physics.Raycast(ray4, out hit, ColliderLenghtX) ||
            Physics.Raycast(ray4_reflected, out hit, ColliderLenghtX))
        {

            if (hit.collider.gameObject.GetComponent<PlacementValidator>())
            {
                return false;
            }

        }
        else if (Physics.BoxCast(Collider.bounds.center, transform.localScale, -transform.up, out hit, transform.rotation, 300f))
        {
            if (hit.collider.tag == "Road")
            {
                return false;
            }
        }

        return true;
    }


}
