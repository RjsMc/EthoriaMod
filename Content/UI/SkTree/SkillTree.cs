using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.GameContent;
using static EthoriaMod.Content.UI.SkTree.SkillTree.SkillTreeNode;

namespace EthoriaMod.Content.UI.SkTree
{

    
    
    public class SkillTree
    {
        public enum GrowDirection
        {
            None,
            Left,
            Up,
            Right,
            Down,
            enumSize
        }
        public SkillTreeNode root;
        public int dist;

        public class SkillTreeNode
        {
            

            public List<List<SkillTreeNode>> Children;
            public List<SkillTreeNode> Parents;
            public int drawX, drawY;
            public string skillName;
            public bool unlocked;
            public GrowDirection growDirection;
            
           
            public SkillTreeNode(int drawX, int drawY, string skillName, GrowDirection growDirection = GrowDirection.None, bool unlocked = false)
            {
                Parents = new List<SkillTreeNode>();
                Children = new List<List<SkillTreeNode>>();
                for (int i = 0; i < (int)GrowDirection.enumSize; i++)
                {
                    Children.Add(new List<SkillTreeNode>());
                }
                this.drawX = drawX;
                this.drawY = drawY;
                this.skillName = skillName;
                this.unlocked = unlocked;
                this.growDirection = growDirection;
            }

            public SkillTreeNode(string skillName, GrowDirection growDirection = GrowDirection.None, bool unlocked = false)
            {
                Parents = new List<SkillTreeNode>();
                Children = new List<List<SkillTreeNode>>();
                for (int i = 0; i < (int) GrowDirection.enumSize; i++)
                {
                    Children.Add(new List<SkillTreeNode>());
                }
                drawX = 0;
                drawY = 0;
                this.skillName = skillName;
                this.unlocked = unlocked;
                this.growDirection = growDirection;
            }

            public SkillTreeNode addChild(string skillName)
            {
                return addChild(skillName, growDirection);
            }

            public SkillTreeNode addChild(string skillName, GrowDirection direction)
            {
                
                SkillTreeNode child = new SkillTreeNode(skillName, direction);

                Children[(int) direction].Add(child);
                child.Parents.Add(this);
                return child;
            }

            
            public void unlock()
            {
                unlocked = true;
            }
        }

        public void drawSkillTree(SpriteBatch spriteBatch, Vector2 displacement)
        {
            SkillTreeNode root = this.root;
            Queue<SkillTreeNode> queue = new Queue<SkillTreeNode>();

            queue.Enqueue(root);

            
            while (queue.Count > 0)
            {

                SkillTreeNode curr = queue.Dequeue();
                Rectangle drawRec = new Rectangle(curr.drawX + (int) displacement.X, curr.drawY + (int) displacement.Y, 10, 10);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawRec, Color.Black);
                Main.NewText(drawRec);
                for (int i = 0; i < (int) GrowDirection.enumSize; i++) { 
                    List<SkillTreeNode> children = curr.Children[i];
                    foreach (SkillTreeNode child in children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

        }
        public void updateChildrenPositions()
        {
            
        }


        public SkillTree(int dist = 10)
        {
            this.dist = dist;
            root = new SkillTreeNode(0, 0, "Start", GrowDirection.None, true);
      
            root.addChild("Warrior", GrowDirection.Left);
            root.addChild("Ranger", GrowDirection.Right);
            root.addChild("Mage", GrowDirection.Up);
            root.addChild("Summoner", GrowDirection.Down);


            updateChildrenPositions();
        }
        

        

    }
}
