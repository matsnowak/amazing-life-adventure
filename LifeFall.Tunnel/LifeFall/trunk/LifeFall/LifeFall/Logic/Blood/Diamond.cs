using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeFall.Logic.Blood
{
    public class Diamond<T> : MovableObject where T: struct
    {

        public enum DiamondType
        {
            GoldDiamond,
            RedDiamond,
            GreenDiamond,
        }


        public DiamondType Type;

        public Diamond()
            : base(Costam.DiamondModel)
        {
            this.CModel.Scale = new Vector3(2f, 2f, 2f);
            this.CModel.textureEnabled = true;

            this.CModel.texture = GetTexture<T>();

        }


        private Texture2D GetTexture<T>() where T : struct
        {
            if (typeof(T) == typeof(Costam.GoldDiamond))
            {
                Type = DiamondType.GoldDiamond;
                return Costam.goldDiamondTexture;

            }
            else if (typeof(T) == typeof(Costam.RedDiamond))
            {
                Type = DiamondType.GoldDiamond;
                return Costam.redDiamondTexture;
            }
            else if (typeof(T) == typeof(Costam.GreenDiamond))
            {
                Type = DiamondType.GoldDiamond;
                return Costam.greenDiamondTexture;
            }

            //default 
            Type = DiamondType.GoldDiamond;
            return Costam.goldDiamondTexture;
            
        }

        public override void OnCollision(Core.Player player)
        {
            switch (Type)
            {
                case DiamondType.GoldDiamond:
                    player.score += 1;
                    break;
                case DiamondType.RedDiamond:
                    player.score += 2;
                    break;
                case DiamondType.GreenDiamond:
                    player.score += 3;
                    break;
            }
        }
    }
}