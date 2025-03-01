using Unity.VisualScripting;
using UnityEngine;
using Weapon;

namespace Interactive
{
    public class GrabsController : MonoBehaviour
    {
        private static Transform _lGrab;
        private static Transform _rGrab;

        private void Start()
        {
            _lGrab = transform.GetChild(0).GetComponent<Transform>();
            _rGrab = transform.GetChild(1).GetComponent<Transform>();
        }

        #region Grab

        public static void GrabLeft(Transform childTransform, WeaponTransformObject weaponTransformObject = null)
        {
            LetGoLeftGrab();
            Grab(childTransform, _lGrab, weaponTransformObject);
        }

        public static void GrabRight(Transform childTransform, WeaponTransformObject weaponTransformObject = null)
        {
            LetGoRightGrab();
            Grab(childTransform, _rGrab, weaponTransformObject);
        }

        private static void Grab(Transform childTransform, Transform grabTransform,
            WeaponTransformObject weaponTransformObject = null)
        {
            if (childTransform.TryGetComponent<Rigidbody>(out Rigidbody componentRigidbody))
                Destroy(componentRigidbody);
            if (childTransform.TryGetComponent<MeshCollider>(out MeshCollider componentMeshCollider))
                Destroy(componentMeshCollider);

            childTransform.gameObject.layer = LayerMask.NameToLayer("Default");
            childTransform.parent = grabTransform;
            childTransform.localPosition =
                weaponTransformObject == null ? Vector3.zero : weaponTransformObject.position;
            childTransform.localRotation =
                weaponTransformObject == null ? Quaternion.identity : weaponTransformObject.rotation;
            childTransform.localScale = weaponTransformObject == null ? Vector3.one : weaponTransformObject.scale;
        }

        #endregion

        #region LetGo

        public static void LetGo()
        {
            LetGoLeftGrab();
            LetGoRightGrab();
        }

        public static void LetGoLeftGrab() => LetGo(_lGrab);

        public static void LetGoRightGrab() => LetGo(_rGrab);

        private static void LetGo(Transform grabTransform)
        {
            if (grabTransform.childCount <= 0) return;
            var tempTransform = grabTransform.GetChild(0);
            tempTransform.gameObject.AddComponent<Rigidbody>();
            tempTransform.gameObject.AddComponent<MeshCollider>().convex = true;
            tempTransform.gameObject.layer = LayerMask.NameToLayer("Interactive");
            tempTransform.parent = null;
        }

        #endregion

        #region Remove

        public static void RemoveChildrenLeft() => RemoveChildren(_lGrab);
        public static void RemoveChildrenRight() => RemoveChildren(_rGrab);

        public static void RemoveChildrens()
        {
            RemoveChildrenLeft();
            RemoveChildrenRight();
        }

        private static void RemoveChildren(Transform grabTransform)
        {
            if (grabTransform.childCount > 0)
                Destroy(grabTransform.GetChild(0).gameObject);
        }

        #endregion
    }
}