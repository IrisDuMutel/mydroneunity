# mydrone

Drone simulation with realistic physics used to test Control and Navigation algorithms. 

The drone contains many approaches to be controlled:

- Throughout BasicControl.cs script. With this script, we can control the drone in three different ways:
        - With the keyboard
        - With ROS through force commands
        - With ROS through roll, pitch, yaw and vertical velocity commands
    This approach is mainly inspired by Berkeley drone project. You can find it here:
    ''' https://github.com/UAVs-at-Berkeley/UnityDroneSim '''

- If BasicControl.cs box is unchecked, then we must use OdomSub.cs, which subscribes to the motor forces and does the transforms between right and left RFs. This approach is used together with a Simulink model that acts as control and navigation.  


