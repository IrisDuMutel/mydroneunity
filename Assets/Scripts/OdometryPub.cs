using RosMessageTypes.RoboticsDemo;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// Working
/// </summary>
using Odometry = RosMessageTypes.Nav.Odometry;
using Header = RosMessageTypes.Std.Header;
public class OdometryPub : MonoBehaviour
{

    public Rigidbody _RigidBody;
    public Transform BodyTransform;
    public Transform InertialTransform;
    ROSConnection ros;
    private string topicName="odom";
    private uint flag;
    private Quaternion orient;
    private Vector3 posit;
    private Vector3 lin_vel;
    private Vector3 ang_vel;
    // Publish the BodyTransform's position and rotation every N seconds
    public GameObject cube;
    public RFHandTransform RFtrans;
    private Vector3 last_pos;
    private Vector3 inert_linvel;
    public float publishMessageFrequency = 0.5f;
    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        // QualitySettings.vSyncCount = 1;
        // Application.targetFrameRate = 30;
        last_pos = InertialTransform.position;
        ros = ROSConnection.instance;
        flag = 1;
        
    }
    void Awake () {
        #if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30; // Set the framerate you want and check it iunder 'stats' during in gamemode
        #endif
    }
    private void Update()
    {
        
        // timeElapsed += Time.deltaTime;
        // Debug.Log(Time.fixedDeltaTime);
        // if (timeElapsed > publishMessageFrequency)
        // { 

            // Initialization
            RosMessageTypes.Std.UInt32 seq = new RosMessageTypes.Std.UInt32(flag);
            RosMessageTypes.Std.Time tiempo = new RosMessageTypes.Std.Time((uint)Time.time, (uint)Time.time);
            string frame_id = _RigidBody.name;
            Header header = new Header(flag, tiempo, frame_id);
            RosMessageTypes.Std.Float64 cov = new RosMessageTypes.Std.Float64(0);
            //Header header;
            //header.seq = flag;
            flag++;
            //header.frame_id
            // int[,] array2D = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };

            double[] covar = new double[]{ 0.01, 0.00, 0.00, 0.00, 0.00, 0.00,
                          .00, 0.01, 0.00, 0.00, 0.00, 0.00,
                          .00, 0.00, 0.01, 0.00, 0.00, 0.00,
                          .00, 0.00, 0.00, 0.10, 0.00, 0.00,
                          .00, 0.00, 0.00, 0.00, 0.10, 0.00,
                          0.00, 0.00, 0.00, 0.00, 0.00, 0.10};

            // Computation of the linear velocity of the inertial RF
            if(last_pos != InertialTransform.position) {
                inert_linvel = InertialTransform.position - last_pos;
                inert_linvel /= Time.deltaTime;
                last_pos = InertialTransform.position;
            }
            // transformation to right hand RF with correct orientation (z upwards)
            (orient,posit,lin_vel,ang_vel) = RFtrans.Left2Right(BodyTransform,inert_linvel,_RigidBody);
            RosMessageTypes.Geometry.Point position = new RosMessageTypes.Geometry.Point(posit[0],posit[1],posit[2]);
            RosMessageTypes.Geometry.Quaternion orientation = new RosMessageTypes.Geometry.Quaternion(
            orient.x,
            orient.y,
            orient.z,
            orient.w
            );
            // For now, linear velocity is given in inertial reference frame
            RosMessageTypes.Geometry.Vector3 linear = new RosMessageTypes.Geometry.Vector3(lin_vel[0],lin_vel[1],lin_vel[2]);
            RosMessageTypes.Geometry.Vector3 angular = new RosMessageTypes.Geometry.Vector3(ang_vel[0],ang_vel[1],ang_vel[2]);
             

            RosMessageTypes.Geometry.Pose pose = new RosMessageTypes.Geometry.Pose(position, orientation);
            RosMessageTypes.Geometry.Twist twist = new RosMessageTypes.Geometry.Twist(linear, angular);

            RosMessageTypes.Geometry.PoseWithCovariance posewithcov = new RosMessageTypes.Geometry.PoseWithCovariance(pose,covar);
            RosMessageTypes.Geometry.TwistWithCovariance twistwithcov = new RosMessageTypes.Geometry.TwistWithCovariance(twist, covar);
            Odometry targetPos = new Odometry(
                header,
                "body",
                posewithcov,
                twistwithcov

            
            ) ;
            // BodyTransform.GetWorldPose(out _pos, out _rot);
            // Finally send the message to server_endpoint.py running in ROS
            ros.Send(topicName, targetPos);

            timeElapsed = 0;
        // }
    }
}