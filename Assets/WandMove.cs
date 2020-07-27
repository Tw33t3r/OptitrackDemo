using UnityEngine;
using System.Collections;

public class WandMove : MonoBehaviour {
    public string DeviceName = "Wand@10.2.224.199:3883";
    //private static GameController gameController;
   
    private int deviceID = -1;
   
    // Use this for initialization
    void Start()
    {
        //gameController = GetComponent<GameController>();
        
       deviceID = VRPNController.VrpnStart(DeviceName,
                                             null,
                                             null,
                                             new VRPNController.TrackerCallback(OnTrackerMoved));
    }


    // Update is called once per frame
    void Update()
    {
        // Poll for VRPN events
        VRPNController.VrpnUpdate(deviceID);    

    }
    

    // Free the VRPN and Vrpn2Unity resources
    void OnDestroy()
    {
        if (deviceID != -1)
        {
            VRPNController.VrpnOnDestroy(deviceID);
            deviceID = -1;
        }
        else
        {
            Debug.Log("Demo Controller: No VRPN resources were allocated");
        }
    }

 
    public void OnTrackerMoved(int sensor_id,
                                      double pos_x, double pos_y, double pos_z,
                                      double quat_x, double quat_y, double quat_z, double quat_w)
    {
        //Debug.Log("Pos_X: " + pos_x + "Pos_Y: " + pos_y);

        

        GameObject.Find("Wand").transform.position = new Vector3((float)pos_x, (float)pos_y, -(float)pos_z);

         Quaternion trackerQuaternion = new Quaternion();
         trackerQuaternion.Set((float) quat_x,  (float) quat_y,  -(float) quat_z,  -(float) quat_w); //public void Set(float newX, float newY, float newZ, float newW);
         GameObject.Find("Wand").transform.rotation = trackerQuaternion;

         //using Eulers...
        //GameObject.Find("Cube").transform.rotation = Quaternion.Euler((float) quat_x, (float)quat_y, (float)quat_z);
    }


   


}
