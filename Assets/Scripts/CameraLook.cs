using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float sensitivity = 5;
    public bool InvertX;
    public bool InvertY;

    public Transform player;

    private void Update()
    {
        float mousex = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mousey = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        if (InvertX) mousex = -1 * mousex;
        if (InvertY) mousey = -1 * mousey;

        player.Rotate(new Vector3(0, 1, 0) * mousex);
        transform.Rotate(new Vector3(1, 0, 0) * mousey);
        //transform.localRotation = Quaternion.Euler(0, mousey, 0);
    }
}
