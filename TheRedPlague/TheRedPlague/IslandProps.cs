using Nautilus.Handlers;
using UnityEngine;

namespace TheRedPlague;

public static class IslandProps
{
    private const string AnimatedLightClassId = "ForceFieldIslandLight";
    private const string AlienRobotClassID = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
    
    public static void AddIslandPropSpawns()
    {
        var animatedLightScale = new Vector3(0.5f, 0.25f, 0.25f);
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-41.757f, 307.086f, -33.187f), new Vector3(326.504f, 172.957f, 182.122f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-36.066f, 304.599f, -36.635f), new Vector3(52.801f, 351.071f, 342.123f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-60.397f, 306.631f, -32.461f), new Vector3(353.174f, 272.766f, 172.837f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-53.199f, 310.016f, -15.484f), new Vector3(1.635f, 324.532f, 185.110f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-50.164f, 307.744f, -21.824f), new Vector3(66.509f, 266.686f, 243.503f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-42.620f, 306.512f, -25.497f), new Vector3(338.246f, 352.451f, 37.897f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-33.901f, 302.194f, -31.067f), new Vector3(322.229f, 22.068f, 300.632f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-51.961f, 305.807f, -26.401f), new Vector3(0.473f, 357.134f, 198.738f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-54.111f, 303.441f, -29.011f), new Vector3(60.069f, 183.712f, 182.147f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-59.141f, 304.790f, -26.266f), new Vector3(345.713f, 304.664f, 153.112f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-57.398f, 303.601f, -31.915f), new Vector3(11.782f, 14.577f, 102.210f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-70.367f, 311.286f, -20.820f), new Vector3(6.540f, 0.426f, 7.438f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-68.960f, 312.609f, -15.169f), new Vector3(326.325f, 114.223f, 202.146f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-64.405f, 310.836f, -15.819f), new Vector3(62.418f, 173.642f, 176.146f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-67.055f, 302.892f, -21.329f), new Vector3(332.279f, 31.614f, 262.148f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-67.779f, 301.718f, -31.214f), new Vector3(34.441f, 330.908f, 280.130f), animatedLightScale));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AlienRobotClassID, new Vector3(-52.030f, 309.000f, -16.060f), Vector3.zero));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AlienRobotClassID, new Vector3(-42.568f, 307.000f, -29.522f), Vector3.zero));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AlienRobotClassID, new Vector3(-62.802f, 302.410f, -25.161f), Vector3.zero));
    }
}