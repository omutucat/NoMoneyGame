using System;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Pieces
{
    public static class PieceTextures
    {
        private static Texture2D PAWN_IMAGE = Resources.Load<Texture2D>("Images/Pieces/Pawn");
        private static Texture2D HERO_IMAGE = Resources.Load<Texture2D>("Images/Pieces/Hero");
        private static Texture2D GHOST_IMAGE = Resources.Load<Texture2D>("Images/Pieces/Ghost");
        private static Texture2D TROLL_IMAGE = Resources.Load<Texture2D>("Images/Pieces/Troll");

        public static Texture2D PieceTexture(PieceBase piece) => piece switch
        {
            Pawn => PAWN_IMAGE,
            Hero => HERO_IMAGE,
            Ghost => GHOST_IMAGE,
            Troll => TROLL_IMAGE,
            _ => throw new ArgumentException("Invalid piece type")
        };
    }
}
