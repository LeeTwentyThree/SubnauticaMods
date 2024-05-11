using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public static class FleshBlobData
{
    public static FleshBlobPath[] Paths { get; } = new FleshBlobPath[]
    {
        // Dunes 1
        new FleshBlobPath(new Vector2(-1000, 1200), new Vector2(-1050, 1500), new Vector2(-1180, 1667),
            new Vector2(-1126, 1430), new Vector2(-1200, 1230), new Vector2(-1685, 770), new Vector2(-1500, 700),
            new Vector2(-1660, 400), new Vector2(-1500, 240), new Vector2(-1700, 100), new Vector2(-1680, -64),
            new Vector2(-1400, 100), new Vector2(-1060, -60), new Vector2(-1220, 320), new Vector2(-1470, 500),
            new Vector2(-1378, 726), new Vector2(-1130, 1130)),
        // Dunes 2
        new FleshBlobPath(new Vector2(-1430, 800), new Vector2(-1270, 400), new Vector2(-1450, 170),
            new Vector2(-1580, 476)),
        // Safe shallows & Kelp forest
        new FleshBlobPath(new Vector2(-822, 201), new Vector2(-170, 280), new Vector2(-283, -213),
            new Vector2(-80, -300), new Vector2(-280, -530), new Vector2(70, -380), new Vector2(60, -180),
            new Vector2(240, -500), new Vector2(400, -160), new Vector2(440, 30), new Vector2(100, 80),
            new Vector2(0, 280), new Vector2(400, 600), new Vector2(340, 777), new Vector2(-133, 550),
            new Vector2(-110, 340), new Vector2(-110, 140),
            new Vector2(0, -50), new Vector2(-280, 140)),
        // Southwest
        new FleshBlobPath(new Vector2(-633, -1260), new Vector2(-600, -1737), new Vector2(-1126, -1361),
            new Vector2(-1400, -1381), new Vector2(-1330, -820), new Vector2(-1600, -766),
            new Vector2(-1575, -150), new Vector2(-1120, -600), new Vector2(-1100, -200), new Vector2(-1430, -550),
            new Vector2(-1240, -950), new Vector2(-1230, -1230), new Vector2(-840, -1280),
            new Vector2(-1100, -1100), new Vector2(), new Vector2(-960, -680), new Vector2(-700, -600),
            new Vector2(-710, -430), new Vector2(-590, -476), new Vector2(-670, -750),
            new Vector2(-830, -800), new Vector2(-820, -1210), new Vector2(-472, -1080), new Vector2(-400, -750),
            new Vector2(-255, -773), new Vector2(-300, -1500), new Vector2(15, -1600), new Vector2(-120, -1670),
            new Vector2(-380, -1500),
            new Vector2(-472, -1600), new Vector2(-523, -1154), new Vector2(-750, -1236)),
        // Northwest
        new FleshBlobPath(new Vector2(-640, 1373), new Vector2(-430, 1000), new Vector2(-315, 580),
            new Vector2(-930, 400), new Vector2(-670, 770), new Vector2(-1100, 700),
            new Vector2(-880, 1020), new Vector2(-740, 1100), new Vector2(-800, 1450), new Vector2(-760, 1700),
            new Vector2(-100, 1500), new Vector2(50, 900), new Vector2(-75, 700),
            new Vector2(-500, 1400)),
        // Northeast
        new FleshBlobPath(new Vector2(150, 1450), new Vector2(1130, 1700), new Vector2(200, 1260),
            new Vector2(1400, 1550), new Vector2(200, 1000), new Vector2(1580, 1200), new Vector2(500, 800),
            new Vector2(700, 180), new Vector2(1330, 650)),
        // Southeast
        new FleshBlobPath(new Vector2(200, -1000), new Vector2(400, -540), new Vector2(500, -800),
            new Vector2(660, -760), new Vector2(-640, -1100), new Vector2(360, -1444), new Vector2(-70, -1130)),
        // Plague heart island
        new FleshBlobPath(new Vector2(-1438.00f, 291.77f), new Vector2(-1354.77f, 312.60f), new Vector2(-1343, 318),
            new Vector2(-1305.97f, 301.15f), new Vector2(-1275.35f, 262.93f), new Vector2(-1311.97f, 178.71f),
            new Vector2(-1382.28f, 235.76f), new Vector2(-1412.04f, 213.54f), new Vector2(-1453.31f, 263.35f))
    };
}