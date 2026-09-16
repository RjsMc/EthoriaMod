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
            LoadedShot,
            DmgBoost1,
            Velocity,
            DoubleShot,
            TripleShot,
            Unwavering,
            Precision,
            Mage,
            
            Summoner,
            
            
            
            EnumSize

        }
        public enum GrowDirection
        {
            None,
            Left,
            Up,
            Right,
            Down,
            EnumSize
        }

        public List<SkillTreeNode> nodeList;
        public SkillTreeNode root;
        public int nodeDist;
        public static int defaultSize = 10;
        public Texture2D skillTreePlate;
        public Texture2D bowIcon;
        public class SkillTreeNode : TagSerializable
        {


            public List<List<SkillTreeNode>> children;
            public List<SkillTreeNode> parents;
            public List<SkillTreeNode> dependencies;
            public List<SkillTreeNode> dependentOnMe;
            public Vector2 drawPos;
            public SkillID skillID;
            public bool unlocked;
            public GrowDirection growDirection;
            public int w;
            public int h;
            public bool hidden;

            public Texture2D myPlate;
            public Texture2D myIcon;
            public SkillTreeNode(float drawX, float drawY, SkillID skillID, GrowDirection growDirection = GrowDirection.None, bool hidden = true, bool unlocked = false)
            {
                dependencies = new List<SkillTreeNode>();
                dependentOnMe = new List<SkillTreeNode>();
                parents = new List<SkillTreeNode>();
                children = new List<List<SkillTreeNode>>();
                for (int i = 0; i < (int)GrowDirection.EnumSize; i++)
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
                dependencies = new List<SkillTreeNode>();
                dependentOnMe = new List<SkillTreeNode>();
                parents = new List<SkillTreeNode>();
                children = new List<List<SkillTreeNode>>();
                for (int i = 0; i < (int)GrowDirection.EnumSize; i++)
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

            public SkillTreeNode addDependency(SkillTreeNode them)
            {
                dependencies.Add(them);
                them.dependentOnMe.Add(this);
                return this;
            }

            public SkillTreeNode addEdge(SkillTreeNode child)
            {

                child.parents.Add(this);
                return this;
            }

            public SkillTreeNode addChild(SkillID skillID, List<SkillTreeNode> nodeList)
            {
                return addChild(skillID, growDirection, nodeList);
            }


            public SkillTreeNode addChild(SkillID skillID, GrowDirection direction, List<SkillTreeNode> nodeList)
            {

                SkillTreeNode child = new SkillTreeNode(skillID, direction);

                children[(int)direction].Add(child);
                child.parents.Add(this);

                nodeList[(int)child.skillID] = child;
                return child;
            }

            public void changeLockState()
            {
                if (unlocked)
                {
                    relock();
                } else
                {
                    unlock();
                }
            }

            public void relockNoParent()
            {
                if (unlocked == false)
                {
                    return;
                }
                bool hasParentUnlocked = false;
                for (int i = 0; i < parents.Count; i++) { 
                    if (parents[i].unlocked)
                    {
                        hasParentUnlocked = true;
                        break;
                    }
                }

                unlocked = hasParentUnlocked;
            }
            public void relock() {
                unlocked = false;
                for (int i = 0; i < dependentOnMe.Count; i++)
                {
                    dependentOnMe[i].relock();
                }

                for (int i = 0; i < (int) GrowDirection.EnumSize; i++)
                {
                    List<SkillTreeNode> directionalChildren = children[i];
                    foreach (SkillTreeNode child in directionalChildren)
                    {
                        child.relock();
                    }
                }
                
            }

            public void unlock()
            {
                unlocked = true;
                for (int i = 0; i < (int) GrowDirection.EnumSize; i++)
                {
                    foreach (SkillTreeNode child in children[i])
                    {
                        child.hidden = false;
                    }
                }
            }

            public bool unlockable()
            {
                bool oneParentUnlocked = false;
                for (int i = 0; i < parents.Count; i++)
                {
                    if (parents[i].unlocked)
                    {
                        oneParentUnlocked = true;
                    }
                }
                if (parents.Count > 0 && !oneParentUnlocked)
                {
                    return false;
                }
                for (int i = 0; i < dependencies.Count; i++)
                {
                    if (!dependencies[i].unlocked)
                    {
                        return false;
                    }
                }
                return true;
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

                    case SkillID.LoadedShot:
                        return "Right click ability shoots an arrow on demand";

                    case SkillID.DmgBoost1:
                        return "Increase ranged damage by 25%";

                    case SkillID.Velocity:
                        return "Bows shoot at a higher accuracy and velocity";
                       
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

        public SkillTree(int nodeDist = 250)
        {
            skillTreePlate = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/SkTree/Assets/SkillTreePlateBorder").Value;
            bowIcon = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/SkTree/Assets/PlaceholderBow").Value;

            this.nodeDist = nodeDist;
            root = new SkillTreeNode(0.5f, 0.5f, SkillID.Start, GrowDirection.None);
            nodeList = Enumerable.Repeat((SkillTreeNode)null, (int) SkillID.EnumSize).ToList();
            nodeList[(int) root.skillID] = root;

            root.addChild(SkillID.Warrior, GrowDirection.Left, nodeList);
            SkillTreeNode ranger = root.addChild(SkillID.Ranger, GrowDirection.Right, nodeList);
            SkillTreeNode quickDraw = ranger.addChild(SkillID.Quickdraw, nodeList);
            ranger.addChild(SkillID.Precision, nodeList);

            ranger.addDependency(root);

            quickDraw.addChild(SkillID.LoadedShot, GrowDirection.Up, nodeList);

            SkillTreeNode dmgBoost1 = quickDraw.addChild(SkillID.DmgBoost1, nodeList); 

            SkillTreeNode doubleShot = dmgBoost1.addChild(SkillID.DoubleShot, nodeList);
            SkillTreeNode velocity = dmgBoost1.addChild(SkillID.Velocity, nodeList);
            

            //doubleShot.addEdge(velocity);






            root.addChild(SkillID.Mage, GrowDirection.Up, nodeList);
            SkillTreeNode summoner = root.addChild(SkillID.Summoner, GrowDirection.Down, nodeList);



            updateChildrenPositions();
        }
        public void drawSkillTree(SpriteBatch spriteBatch, Vector2 displacement, Vector2 cutoutPosition, Vector2 windowPosition, Rectangle backgroundRect)
        {
            SkillTreeNode root = this.root;
            Queue<SkillTreeNode> queue = new Queue<SkillTreeNode>();

            float zoom = 1f / SkillTreeUI.skillTreeZoom;

            queue.Enqueue(root);

            while (queue.Count > 0) 
            {

                SkillTreeNode curr = queue.Dequeue();
                int drawXScreen = (int)(Main.screenWidth * (curr.drawPos.X + displacement.X));
                int drawYScreen = (int)(Main.screenHeight * (curr.drawPos.Y + displacement.Y));

                int plateW = skillTreePlate.Width;
                int plateH = skillTreePlate.Height;

                Rectangle rect = new Rectangle(drawXScreen - plateW / 2, drawYScreen - plateH / 2, plateW, plateH);

                int windowDx = (int) (windowPosition.X - cutoutPosition.X);
                int windowDy = (int) (windowPosition.Y - cutoutPosition.Y);


                int nodeRectW = (int) ((float) plateW * zoom);
                int nodeRectX = (int) ((float) (drawXScreen - Main.screenWidth / 2) * zoom);
                nodeRectX += Main.screenWidth / 2 + windowDx - (nodeRectW / 2);

                int nodeRectH = (int) ((float) plateH * zoom);
                int nodeRectY = (int) ((float) (drawYScreen - Main.screenHeight / 2) * zoom);
                nodeRectY += Main.screenHeight / 2 + windowDy - (nodeRectH / 2);
                Rectangle nodeRect = new Rectangle(nodeRectX, nodeRectY, nodeRectW, nodeRectH);




                Color color = Color.White;
                if (backgroundRect.Contains(new Point(Main.mouseX, Main.mouseY)) && nodeRect.Contains(new Point(Main.mouseX , Main.mouseY)) && curr.unlockable())
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

              
                for (int i = 0; i < (int) GrowDirection.EnumSize; i++) { 
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

                if (curr.myIcon != null)
                {
                    spriteBatch.Draw(curr.myIcon, rect, color);
                } else
                {
                    spriteBatch.Draw(bowIcon, rect, color);
                }
                spriteBatch.Draw(skillTreePlate, rect, color);
               
                
            }
        }
        public void updateChildrenPositions()
        {
            Queue<SkillTreeNode> queue = new Queue<SkillTreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                SkillTreeNode curr = queue.Dequeue();
                
                for (int i = 0; i < (int)GrowDirection.EnumSize; i++)
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
                if (nodeList[i] != null && nodeList[i].unlocked)
                {
                    switch (nodeList[i].skillID)
                    {
                        case SkillID.Ranger:
                            player.GetDamage(DamageClass.Ranged) += 0.25f; 
                            break;

                        case SkillID.Quickdraw:
                            player.GetAttackSpeed(DamageClass.Ranged) += 0.25f;
                            break;

                        case SkillID.DmgBoost1:
                            player.GetDamage(DamageClass.Ranged) += 0.25f;
                            break;

                        case SkillID.DoubleShot:
                            ethPlayer.numArrows++;
                            break;

                        case SkillID.TripleShot:
                            ethPlayer.numArrows++;
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
