using UnityEngine;




namespace DarkHorizon
{
    public class RotatePl : MonoBehaviour
    {
        public float sensitivity;
        public float smoothing;

        Vector2 mouseLook;
        Vector2 smoothV;

        public Camera cam;
        public GameObject head;


        private void FixedUpdate()
        {
            var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 85f);
            cam.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            head.transform.localRotation = Quaternion.AngleAxis(mouseLook.y * 0.6f, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
        }
    }
}

