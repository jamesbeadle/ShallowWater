using ShallowWater.Game.Pound;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class GlascoteLock
    {
        private static readonly Color Brick = new Color(0.46f, 0.26f, 0.20f);
        private static readonly Color GateTimber = new Color(0.10f, 0.09f, 0.08f);
        private static readonly double[] Banks = { PoundLimits.TowpathSide, -PoundLimits.TowpathSide };
        private const float ChamberHalfWidthMetres = 1.1f;
        private const float WallReachMetres = 7.5f;
        private const float WallTopMetres = 0.6f;
        private const float WallFootMetres = -1f;
        private const float GateTopMetres = 0.95f;
        private const float GateFootMetres = -0.85f;
        private const float GateThicknessMetres = 0.3f;
        private const float BeamLengthMetres = 4f;
        private const float BeamThicknessMetres = 0.3f;
        private const float BeamOverTheWallMetres = 0.5f;
        private const float TopGateFromTheEndMetres = 1f;
        private const float Half = 0.5f;

        public static void CloseItsBottomGates(Pound pound)
        {
            var brick = Paint.Of(Brick);
            var timber = Paint.Of(GateTimber);
            foreach (var bank in Banks)
            {
                ChamberWall(pound, bank, brick);
                GateLeaf(pound, bank, timber);
                BalanceBeam(pound, bank, timber);
            }
            TopGate(pound, timber);
        }

        private static void ChamberWall(Pound pound, double bank, Material brick)
        {
            var chamberLength = (float)PoundLimits.LockGatesFromGlascoteMetres;
            var along = pound.LockGatesAlong + chamberLength * Half;
            var across = bank * (ChamberHalfWidthMetres + WallReachMetres) * Half;
            var size = new Vector3(WallReachMetres - ChamberHalfWidthMetres, WallTopMetres - WallFootMetres, chamberLength);
            Slab(pound, "Lock wall", brick, along, across, (WallTopMetres + WallFootMetres) * Half, size);
        }

        private static void GateLeaf(Pound pound, double bank, Material timber)
        {
            var size = new Vector3(ChamberHalfWidthMetres, GateTopMetres - GateFootMetres, GateThicknessMetres);
            var across = bank * ChamberHalfWidthMetres * Half;
            Slab(pound, "Lock gate", timber, pound.LockGatesAlong, across, (GateTopMetres + GateFootMetres) * Half, size);
        }

        private static void BalanceBeam(Pound pound, double bank, Material timber)
        {
            var along = pound.LockGatesAlong + BeamLengthMetres * Half;
            var across = bank * (ChamberHalfWidthMetres + BeamOverTheWallMetres);
            var size = new Vector3(BeamThicknessMetres, BeamThicknessMetres, BeamLengthMetres);
            Slab(pound, "Balance beam", timber, along, across, WallTopMetres + BeamThicknessMetres * Half, size);
        }

        private static void TopGate(Pound pound, Material timber)
        {
            var along = pound.LengthMetres - TopGateFromTheEndMetres;
            var size = new Vector3(ChamberHalfWidthMetres * 2, GateTopMetres - GateFootMetres, GateThicknessMetres);
            Slab(pound, "Lock gate", timber, along, 0, (GateTopMetres + GateFootMetres) * Half, size);
        }

        private static void Slab(Pound pound, string name, Material paint, double along, double across, float height, Vector3 size)
        {
            var position = PoundPlacement.OnThePound(pound, along, across, height);
            var turn = PoundPlacement.AlongThePound(pound, along);
            Blocks.Place(PrimitiveType.Cube, name, paint, new Placement(position, size, turn));
        }
    }
}
