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

    public Rigidbody rb;
    public Transform transf;
    ROSConnection ros;
    private string topicName="odom";
    private uint flag;
    private Quaternion orient;
    private Vector3 posit;
    private Vector3 lin_vel;
    private Vector3 ang_vel;
    // Publish the transf's position and rotation every N seconds
    public GameObject cube;
    public RFHandTransform RFtrans;
    public float publishMessageFrequency = 0.5f;
    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.instance;
        flag = 1;
        
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {

            // Initialization
            RosMessageTypes.Std.UInt32 seq = new RosMessageTypes.Std.UInt32(flag);
            RosMessageTypes.Std.Time tiempo = new RosMessageTypes.Std.Time((uint)Time.time, (uint)Time.time);
            string frame_id = rb.name;
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
            (orient,posit,lin_vel,ang_vel) = RFtrans.Left2Right(transf,rb);
            RosMessageTypes.Geometry.Point position = new RosMessageTypes.Geometry.Point(posit[0],posit[1],posit[2]);
            RosMessageTypes.Geometry.Quaternion orientation = new RosMessageTypes.Geometry.Quaternion(
            orient.x,
            orient.y,
            orient.z,
            orient.w

            );
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
            // transf.GetWorldPose(out _pos, out _rot);
            // Finally send the message to server_endpoint.py running in ROS
            ros.Send(topicName, targetPos);

            timeElapsed = 0;
        }
    }
}