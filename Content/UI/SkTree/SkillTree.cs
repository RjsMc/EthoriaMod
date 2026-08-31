using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
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
            public Vector2 drawPos;
            public int drawX, drawY;
            public string skillName;
            public bool unlocked;
            public GrowDirection growDirection;
            
           
            public SkillTreeNode(float drawX, float drawY, string skillName, GrowDirection growDirection = GrowDirection.None, bool unlocked = false)
            {
                Parents = new List<SkillTreeNode>();
                Children = new List<List<SkillTreeNode>>();
                for (int i = 0; i < (int)GrowDirection.enumSize; i++)
                {
                    Children.Add(new List<SkillTreeNode>());
                }
               
                this.drawPos = new Vector2(drawX, drawY);
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

                this.drawPos = new Vector2(0, 0);
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
                float drawXScreen = Main.screenWidth * (curr.drawPos.X + displacement.X);
                float drawYScreen = Main.screenHeight * (curr.drawPos.Y + displacement.Y);
                Rectangle rect = new Rectangle((int) drawXScreen, (int) drawYScreen, 10, 10);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, Color.Black);
              
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
            Queue<SkillTreeNode> queue = new Queue<SkillTreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                SkillTreeNode curr = queue.Dequeue();
                for (int i = 0; i < (int)GrowDirection.enumSize; i++)
                {
                    switch ((GrowDirection)i)
                    {
                        case GrowDirection.Left:

                            break;

                        case GrowDirection.Right:

                            break;

                        case GrowDirection.Up:

                            break;


                        case GrowDirection.Down:

                            break;

                    }
                }
            }
        }
            

        public SkillTree(int dist = 10)
        {
            this.dist = dist;
            root = new SkillTreeNode(0.5f, 0.5f, "Start", GrowDirection.None, true);
      
            root.addChild("Warrior", GrowDirection.Left);
            root.addChild("Ranger", GrowDirection.Right);
            root.addChild("Mage", GrowDirection.Up);
            root.addChild("Summoner", GrowDirection.Down);


            updateChildrenPositions();
        }
        

        

    }
}
