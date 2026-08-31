using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Common.Helpers;
using Humanizer.DateTimeHumanizeStrategy;
using log4net.DateFormatter;
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
        public int nodeDist;
        public static int defaultSize = 10;
        public class SkillTreeNode
        {
            

            public List<List<SkillTreeNode>> Children;
            public List<SkillTreeNode> Parents;
            public Vector2 drawPos;
            public string skillName;
            public bool unlocked;
            public GrowDirection growDirection;
            public int w;
            public int h;

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

                w = defaultSize;
                h = defaultSize;
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
                w = defaultSize;
                h = defaultSize;
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
                int drawXScreen = (int) (Main.screenWidth * (curr.drawPos.X + displacement.X));
                int drawYScreen = (int) (Main.screenHeight * (curr.drawPos.Y + displacement.Y));
                Rectangle rect = new Rectangle(drawXScreen - defaultSize / 2, drawYScreen - defaultSize / 2, defaultSize, defaultSize);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, Color.Black);
              
                for (int i = 0; i < (int) GrowDirection.enumSize; i++) { 
                    List<SkillTreeNode> children = curr.Children[i];


                    foreach (SkillTreeNode child in children)
                    {

                        int childDrawXScreen = (int)(Main.screenWidth * (child.drawPos.X + displacement.X));
                        int childDrawYScreen = (int)(Main.screenHeight * (child.drawPos.Y + displacement.Y));
                        HelperFunctions.drawLine(spriteBatch, new Vector2(drawXScreen, drawYScreen), new Vector2(childDrawXScreen, childDrawYScreen), Color.Black);

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
                    float floatDist = (float) nodeDist;
                    List<SkillTreeNode> directionalChildren = curr.Children[i];
                    Vector2 delta = new Vector2(curr.drawPos.X, curr.drawPos.Y);
                    Vector2 childDirection = new Vector2(0, 0);
                    
                    switch ((GrowDirection)i)
                    {
                        case GrowDirection.Left:
                            delta.X -= floatDist / Main.screenWidth;
                            childDirection.Y++;
                            break;

                        case GrowDirection.Right:
                            delta.X += floatDist / Main.screenWidth;
                            childDirection.Y++;
                            break;

                        case GrowDirection.Up:
                            delta.Y -= floatDist / Main.screenHeight;
                            childDirection.X++;
                            break;


                        case GrowDirection.Down:
                            delta.Y += floatDist / Main.screenHeight;
                            childDirection.X++;
                            break;

                    }
                    int c = 0;
                    foreach (SkillTreeNode child in directionalChildren)
                    {
                        Vector2 midPos = delta;
                        Vector2 scaleRatio = new Vector2(Main.screenWidth, Main.screenHeight);
                        int numChildren = directionalChildren.Count;

                        int childrenSpan = nodeDist * (numChildren - 1) + (defaultSize * numChildren);


                        Vector2 startPos = midPos - (childDirection * ((childrenSpan / 2) - (defaultSize / 2))) / scaleRatio;

                        startPos += (childDirection * ((defaultSize * c) + (nodeDist * c))) / scaleRatio;

                        child.drawPos = startPos;
                        queue.Enqueue(child);


                        c++;
                    }
                }
            }
        }
            

        public SkillTree(int nodeDist = 100)
        {
            this.nodeDist = nodeDist;
            root = new SkillTreeNode(0.5f, 0.5f, "Start", GrowDirection.None, true);
      
            root.addChild("Warrior", GrowDirection.Left);
            root.addChild("Ranger", GrowDirection.Right);
            root.addChild("Mage", GrowDirection.Up);
            SkillTreeNode summoner = root.addChild("Summoner", GrowDirection.Down);
            summoner.addChild("Fireball");
            summoner.addChild("BigBalls");


            updateChildrenPositions();
        }
        

        

    }
}
