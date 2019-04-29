using UnityEngine;

public class ParallaxBackground : MonoBehaviour {
    public Transform background;
    public float smoothing = 1f;
    public float parallaxScale;

    private Vector3 previousCamPos;
    private Transform cam;

    private void Awake() {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    private void Start() {
        parallaxScale = background.position.z*-1;
    }

    private void Update() {
        float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;

        if( Mathf.Abs(previousCamPos.x - cam.position.x) > 40){
            background.transform.position = new Vector3(cam.position.x, cam.position.y, background.position.z);
            previousCamPos = cam.position;

            return;
        }

        float backgroundTargetPosX = background.position.x + parallax;

        Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, background.position.y, background.position.z);
        
        background.position = Vector3.Lerp(background.position, backgroundTargetPos, smoothing* Time.deltaTime);
        
        previousCamPos = cam.position;
    }

}