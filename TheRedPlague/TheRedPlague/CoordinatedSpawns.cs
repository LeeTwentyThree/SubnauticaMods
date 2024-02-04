using Nautilus.Handlers;
using UnityEngine;

namespace TheRedPlague;

public static class CoordinatedSpawns
{
    private const string AnimatedLightClassId = "ForceFieldIslandLight";
    private const string AnimatedLight2ClassId = "ForceFieldIslandLight2";
    private const string AlienRobotClassID = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
    private const string PedestalClassID = "78009225-a9fa-4d21-9580-8719a3368373";
    private const string SkyrayClassID = "6a1b444f-138f-46fa-88bb-d673a2ceb689";
    private const string WarperClassID = "510a71f0-ab6d-4c6a-aa54-a19b3f1c436c";
    
    public static void RegisterCoordinatedSpawns()
    {
        // Force field island
        
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
            
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-86.282f, 313.889f ,-77.578f), new Vector3(13.347f, 0.737f, 6.290f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-70.778f, 314.329f ,-61.721f), new Vector3(357.261f, 0.200f, 351.665f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-67.838f, 311.601f ,-36.161f), new Vector3(355.100f, 0.174f, 355.931f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-57.648f, 311.728f ,-67.915f), new Vector3(353.138f, 359.848f, 2.539f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-39.203f, 314.595f ,-46.810f), new Vector3(3.533f, 359.882f, 356.166f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-33.754f, 311.370f ,-23.261f), new Vector3(357.549f, 359.691f, 14.369f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-56.914f, 316.780f ,-25.002f), new Vector3(344.945f, 1.823f, 346.270f)));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(SkyrayClassID, new Vector3(-42.805f, 314.471f, -60.152f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(SkyrayClassID, new Vector3(-40.189f, 313.932f, -42.701f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(SkyrayClassID, new Vector3(-76.973f, 317.361f, -65.962f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(SkyrayClassID, new Vector3(-29.68f, 314.71f, -33.66f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(SkyrayClassID, new Vector3(-29.68f, 314.71f, -33.66f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(SkyrayClassID, new Vector3(-72.50f, 320.16f, -29.85f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(SkyrayClassID, new Vector3(-63.68f, 318.05f, -50.97f)));
        
        // Plague heart island
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(WarperClassID, new Vector3(-1299.49f, -213.19f, 261.80f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(WarperClassID, new Vector3(-1317.45f, -221.93f, 282.85f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(WarperClassID, new Vector3(-1326.79f, -206.33f, 271.77f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(WarperClassID, new Vector3(-1328.26f, -194.21f, 276.48f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(WarperClassID, new Vector3(-1325.30f, -193.53f, 263.26f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(WarperClassID, new Vector3(-1316.44f, -192.38f, 299.38f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(WarperClassID, new Vector3(-1321.84f, -180.67f, 281.85f)));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AlienRobotClassID, new Vector3(-1327.03f, -192.24f, 283.95f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AlienRobotClassID, new Vector3(-1327.09f, -205.73f, 272.02f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AlienRobotClassID, new Vector3(-1320.22f, -208.44f, 267.77f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AlienRobotClassID, new Vector3(-1318.65f, -217.43f, 277.81f)));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-1326.821f, -204.724f, 268.104f), new Vector3(69.953f, 267.875f, 247.969f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-1315.812f, -211.522f, 269.300f), new Vector3(346.008f, 339.376f, 112.006f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-1316.877f, -215.208f, 281.525f), new Vector3(332.411f, 206.049f, 173.499f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-1332.575f, -207.004f, 276.217f), new Vector3(326.698f, 74.948f, 222.625f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-1324.527f, -223.692f, 283.557f), new Vector3(304.007f, 64.642f, 260.079f), animatedLightScale));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLightClassId, new Vector3(-1311.866f, -229.181f, 279.475f), new Vector3(15.476f, 0.230f, 1.693f), animatedLightScale));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-1328.006f, -197.536f, 290.486f), new Vector3(352.5f, 359.786f, 3.281f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLight2ClassId, new Vector3(-1328.065f, -196.200f, 290.320f), new Vector3(5, 180, 357), new Vector3(0.3f, 0.2f, 0.15f)));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PedestalClassID, new Vector3(-1326.911f, -196.478f, 264.966f), new Vector3(353.537f, 0.451f, 352.025f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(AnimatedLight2ClassId, new Vector3(-1326.737f, -195.149f, 264.818f), new Vector3(357.000f, 359.971f, 353.000f), new Vector3(0.3f, 0.2f, 0.15f)));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo("23d96661-8e2e-4485-9b22-d84707394a0e", new Vector3(-1320.027f, -228.474f, 280.102f), new Vector3(356, 0.4f, 349), new Vector3(1.5f, 1, 1.5f)));
        
        // Base game precursor bases
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.WarperHeart.ClassID, new Vector3(-262.546f, -814.698f, 300.219f), new Vector3(9, 0.335f, 4.289f)));
    }
}