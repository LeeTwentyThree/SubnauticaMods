using System.Collections.Generic;
using Nautilus.Handlers;
using TheRedPlague.PrefabFiles;
using TheRedPlague.PrefabFiles.Equipment;
using TheRedPlague.StructureFormat;
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
        // --- Force field island ---
        
        var animatedLightScale = new Vector3(0.5f, 0.25f, 0.25f);

        // --- Plague heart island ---
        
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
        
        // pedestal
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo("23d96661-8e2e-4485-9b22-d84707394a0e", new Vector3(-1320.027f, -228.474f, 280.102f), new Vector3(356, 0.4f, 349), new Vector3(1.5f, 1, 1.5f)));
        
        // ghost leviathan
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo("54701bfc-bb1a-4a84-8f79-ba4f76691bef", new Vector3(-1253.01f, -159.05f, 276.41f)));
        
        // --- Base game precursor bases ---
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.WarperHeart.ClassID, new Vector3(-262.546f, -814.698f, 300.219f), new Vector3(9, 0.335f, 4.289f)));
        
        // --- Infected divers ---
        
        // lifepod 2
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver3.ClassID, new Vector3(-481.45f, -496.54f, 1323.83f)));

        // lifepod 3
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.SkeletonCorpse.ClassID, new Vector3(-28.18f, -19.40f, 406.94f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver2.ClassID, new Vector3(-27.92f, -19.85f, 411.73f)));
        
        // lifepod 4
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.InfectedCorpseInfo.ClassID, new Vector3(712.27f, 2.09f, 160.94f)));

        // lifepod 6
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.SkeletonCorpse.ClassID, new Vector3(359.76f, -115.58f, 306.60f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.InfectedCorpseInfo.ClassID, new Vector3(366.26f, -114.64f, 305.62f)));

        // lifepod 7
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.InfectedCorpseInfo.ClassID, new Vector3(-56.19f, -180.21f, -1039.19f)));
        
        // lifepod 12
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.InfectedCorpseInfo.ClassID, new Vector3(1119.06f, -269.02f, 564.86f)));

        // lifepod 13
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.SkeletonCorpse.ClassID, new Vector3(-926.35f, -178.92f, 506.76f)));
        
        // lifepod 17
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.InfectedCorpseInfo.ClassID, new Vector3(-515.96f, -95.58f, -56.83f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.SkeletonCorpse.ClassID, new Vector3(-512.98f, -95.68f, -57.87f)));

        // lifepod 19
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.SkeletonCorpse.ClassID, new Vector3(-810.37f, -299.89f, -877.06f)));
        
        // --- Cyclops Wreck ---
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver2.ClassID, new Vector3(-170.46f, -810.40f, 347.80f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver3.ClassID, new Vector3(-172.28f, -811.31f, 340.79f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver1.ClassID, new Vector3(-173.03f, -810.65f, 330.95f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver3.ClassID, new Vector3(-174.38f, -810.78f, 314.75f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver4.ClassID, new Vector3(-176.86f, -810.27f, 313.91f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver2.ClassID, new Vector3(-178.58f, -812.03f, 322.13f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver1.ClassID, new Vector3(-178.33f, -812.17f, 329.48f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver4.ClassID, new Vector3(-177.56f, -808, 335.30f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver1.ClassID, new Vector3(-175.68f, -812.89f, 343.14f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver3.ClassID, new Vector3(-178.71f, -798.35f, 346.96f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver4.ClassID, new Vector3(-187.57f, -806.75f, 326.85f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver3.ClassID, new Vector3(-175.81f, -802.11f, 322.91f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver4.ClassID, new Vector3(-163.41f, -805.39f, 336.32f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver2.ClassID, new Vector3(-173.39f, -810.16f, 332.21f)));
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.BoneArmorDatabox.ClassID, new Vector3(-170.754f, -812.900f, 333.665f), new Vector3(0, 0, 43.07f)));
        
        // Administrator drop pod
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.SkeletonCorpse.ClassID, new Vector3(-175.49f, -664.36f, 3286.42f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.MutantDiver4.ClassID, new Vector3(-175.12f, -659.21f, 3286.98f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(PlagueKnife.Info.ClassID, new Vector3(-175.803f, -666.628f, 3286.333f), new Vector3(82.25f, 211.03f, 182.54f)));
        
        // Drifters
        
        var randomGenerator = new System.Random(51034581);
        for (int i = 0; i < 80; i++)
        {
            var angle = (float) randomGenerator.NextDouble() * Mathf.PI * 2f;
            var distance = Mathf.Pow((float) randomGenerator.NextDouble(), 1/2f) * 1500f;
            var height = 20 + (float) randomGenerator.NextDouble() * 30;
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(ModPrefabs.DrifterHivemindSpawn.ClassID,
                new Vector3(Mathf.Cos(angle) * distance, height, Mathf.Sin(angle) * distance)));
        }
    }
}