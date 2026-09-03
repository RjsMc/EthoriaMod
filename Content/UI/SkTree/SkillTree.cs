using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using EthoriaMod.Common.Helpers;
using EthoriaMod.Content.EthPlayer;
using Humanizer.DateTimeHumanizeStrategy;
using log4net.Core;
using log4net.DateFormatter;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using rail;
using ReLogic.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static EthoriaMod.Content.UI.SkTree.SkillTree.SkillTreeNode;

namespace EthoriaMod.Content.UI.SkTree
{

    
    
    public class SkillTree : TagSerializable
    {

        public enum SkillID
        {
            None,
            Start,
            Warrior,
            Ranger,
            Quickdraw,
            HeavyString,
            DoubleShot,
            TripleShot,
            Unwavering,
            Precision,
            Mage,
            Summoner,

        }
        public enum GrowDirection
        {
            None,
            Left,
            Up,
            Right,
            Down,
            enumSize
        }
        public List<SkillTreeNode> nodeList;
        public SkillTreeNode root;
        public int nodeDist;
        public static int defaultSize = 10;
        public class SkillTreeNode : TagSerializable 
        {
            

            public List<List<SkillTreeNode>> children;
            public List<SkillTreeNode> parents;
            public Vector2 drawPos;
            public SkillID skillID;
            public bool unlocked;
            public GrowDirection growDirection;
            public int w;
            public int h;
            public bool hidden;

            public SkillTreeNode(float drawX, float drawY, SkillID skillID, GrowDirection growDirection = GrowDirection.None, bool hidden = true, bool unlocked = false)
            {
                parents = new List<SkillTreeNode>();
                children = new List<List<SkillTreeNode>>();
                for (int i = 0; i < (int)GrowDirection.enumSize; i++)
                {
                    children.Add(new List<SkillTreeNode>());
                }
               
                this.drawPos = new Vector2(drawX, drawY);
                this.skillID = skillID;
                this.unlocked = unlocked;
                this.growDirection = growDirection;
                this.hidden = hidden;
                w = defaultSize;
                h = defaultSize;
            }

            public SkillTreeNode(SkillID skillID, GrowDirection growDirection = GrowDirection.None, bool hidden = true, bool unlocked = false)
            {
                parents = new List<SkillTreeNode>();
                children = new List<List<SkillTreeNode>>();
                for (int i = 0; i < (int) GrowDirection.enumSize; i++)
                {
                    children.Add(new List<SkillTreeNode>());
                }

                this.drawPos = new Vector2(0, 0);
                this.skillID = skillID;
                this.unlocked = unlocked;
                this.growDirection = growDirection;
                this.hidden = hidden;
                w = defaultSize;
                h = defaultSize;
            }

            public SkillTreeNode addChild(SkillID skillID, List<SkillTreeNode> nodeList)
            {
                return addChild(skillID, growDirection, nodeList);
            }

            public SkillTreeNode addChild(SkillID skillID, GrowDirection direction, List<SkillTreeNode> nodeList)
            {
                
                SkillTreeNode child = new SkillTreeNode(skillID, direction);

                children[(int) direction].Add(child);
                child.parents.Add(this);

                nodeList.Add(child);
                return child;
            }

            public void changeLockState()
            {
                if (unlocked)
                {
                    unlocked = false;
                } else
                {
                    unlock();
                }
            }
            public void unlock()
            {
                unlocked = true;
                for (int i = 0; i < (int) GrowDirection.enumSize; i++)
                {
                    foreach (SkillTreeNode child in children[i])
                    {
                        child.hidden = false;
                    }
                }
            }
            public void unlock(bool state)
            {
                if (!unlocked && state)
                {
                    unlock();
                }
            }

            public TagCompound SerializeData()
            {
                return new TagCompound
                {
                    {"unlocked", unlocked},
                    {"hidden", hidden}
                };
            }

            public static SkillTreeNode Load(TagCompound tag)
            {
                SkillTreeNode ret = new SkillTreeNode(SkillID.None);
                ret.unlocked = tag.GetBool("unlocked");
                ret.hidden = tag.GetBool("hidden");
                return ret;
            }

            public string getDescription()
            {
                switch (skillID)
                {


                    case SkillID.Start:
                        return "Once upon a time...";



                    case SkillID.Warrior:
                        return "Path of the Warrior";

                    case SkillID.Ranger:
                        return "Path of the Ranger\nIncrease ranged damage by 25%";

                    case SkillID.Quickdraw:
                        return "Increase bow draw speed by 25%";

                    case SkillID.DoubleShot:
                        return "Bows shoot an extra arrow";

                    case SkillID.TripleShot:
                        return "Bows shoot an extra arrow";
                        

                    case SkillID.Mage:
                        return "Path of the Mage";

                    case SkillID.Summoner:
                        return "Path of the Summoner";






                }
                return "";
            }

            public static Func<TagCompound, SkillTreeNode> DESERIALIZER = Load;
        }

        public SkillTree(int nodeDist = 100)
        {
            this.nodeDist = nodeDist;
            root = new SkillTreeNode(0.5f, 0.5f, SkillID.Start, GrowDirection.None);
            nodeList = new List<SkillTreeNode>();
            nodeList.Add(root);

            root.addChild(SkillID.Warrior, GrowDirection.Left, nodeList);
            SkillTreeNode ranger = root.addChild(SkillID.Ranger, GrowDirection.Right, nodeList);
            SkillTreeNode quickDraw = ranger.addChild(SkillID.Quickdraw, nodeList);
            ranger.addChild(SkillID.Precision, nodeList);
            
            quickDraw.addChild(SkillID.HeavyString, GrowDirection.Up, nodeList);
            SkillTreeNode doubleShot = quickDraw.addChild(SkillID.DoubleShot, nodeList);
            quickDraw.addChild(SkillID.Unwavering, nodeList);

            doubleShot.addChild(SkillID.TripleShot, nodeList);

            root.addChild(SkillID.Mage, GrowDirection.Up, nodeList);
            SkillTreeNode summoner = root.addChild(SkillID.Summoner, GrowDirection.Down, nodeList);



            updateChildrenPositions();
        }
        public void drawSkillTree(SpriteBatch spriteBatch, Vector2 displacement, Vector2 cutoutPosition, Vector2 windowPosition, Rectangle backgroundRect)
        {
            SkillTreeNode root = this.root;
            Queue<SkillTreeNode> queue = new Queue<SkillTreeNode>();

            queue.Enqueue(root);

            while (queue.Count > 0) 
            {

                SkillTreeNode curr = queue.Dequeue();
                int drawXScreen = (int)(Main.screenWidth * (curr.drawPos.X + displacement.X));
                int drawYScreen = (int)(Main.screenHeight * (curr.drawPos.Y + displacement.Y));
                Rectangle rect = new Rectangle(drawXScreen - defaultSize / 2, drawYScreen - defaultSize / 2, defaultSize, defaultSize);

                int windowDx = (int) (windowPosition.X - cutoutPosition.X);
                int windowDy = (int) (windowPosition.Y - cutoutPosition.Y);


                Rectangle nodeRect = new Rectangle(drawXScreen + windowDx - defaultSize / 2, drawYScreen + windowDy - defaultSize / 2, defaultSize, defaultSize);
                Color color = Color.Black;
                if (backgroundRect.Contains(new Point(Main.mouseX, Main.mouseY)) && nodeRect.Contains(new Point(Main.mouseX , Main.mouseY)))
                {
                    //MouseStrUI.mouseStr = curr.getDescription();

                    Main.instance.MouseText(curr.getDescription());
                    color = Color.Yellow;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        
                        curr.changeLockState();
                    }
                }

                if (curr.unlocked)
                {
                    color = Color.Green;
                }

              
                for (int i = 0; i < (int) GrowDirection.enumSize; i++) { 
                    List<SkillTreeNode> children = curr.children[i];


                    foreach (SkillTreeNode child in children)
                    {
                        if (child.hidden)
                        {
                            continue;
                        }
                        int childDrawXScreen = (int)(Main.screenWidth * (child.drawPos.X + displacement.X));
                        int childDrawYScreen = (int)(Main.screenHeight * (child.drawPos.Y + displacement.Y));
                        HelperFunctions.drawLine(spriteBatch, new Vector2(drawXScreen, drawYScreen), new Vector2(childDrawXScreen, childDrawYScreen), Color.Black);

                        queue.Enqueue(child);
                    }
                }


                spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, color);
               
                
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
                    List<SkillTreeNode> directionalChildren = curr.children[i];
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


        public void updateSkillEffects()
        {
            Player player = Main.LocalPlayer;
            EthoriaPlayer ethPlayer = player.GetModPlayer<EthoriaPlayer>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].unlocked)
                {
                    switch (nodeList[i].skillID)
                    {
                        case SkillID.Ranger:
                            player.GetDamage(DamageClass.Ranged) += 0.25f; 
                            break;

                        case SkillID.Quickdraw:
                            player.GetAttackSpeed(DamageClass.Ranged) += 0.25f;
                            break;

                        case SkillID.HeavyString:
                            player.GetModPlayer<EthoriaPlayer>();
                            break;

                        case SkillID.DoubleShot:
                            ethPlayer.numArrows++;
                            break;

                        case SkillID.TripleShot:
                            ethPlayer.numArrows += 100;
                            break;


                    }                    
                }
            }
        }

        public TagCompound SerializeData()
        {
            return new TagCompound
            {
                {"nodeList", nodeList}
            };
        }

        public static SkillTree Load(TagCompound tag)
        {
            SkillTree ret = new SkillTree();
            
            if (tag.ContainsKey("nodeList"))
            {
                List<SkillTreeNode> savedNodes = (List<SkillTreeNode>)tag.GetList<SkillTreeNode>("nodeList");
                List<SkillTreeNode> l = ret.nodeList;

                for (int i = 0; i < l.Count; i++)
                {
                    l[i].unlock(savedNodes[i].unlocked);
                    l[i].hidden = savedNodes[i].hidden; 
                }
            }
            return ret;

        }

        public static Func<TagCompound, SkillTree> DESERIALIZER = Load;

    }
}
