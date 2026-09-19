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
     

        public List<SkillTreeNode> nodeList;
        public SkillTreeNode root;
        public int nodeDist;
        public static int defaultSize = 10;

        public Texture2D skillTreeBackground;
        public Texture2D skillTreePlate;
        public Texture2D bowIcon;
        public Texture2D doubleBowIcon;

        public int squareSize;

        public class SkillTreeNode : TagSerializable
        {


            public List<SkillTreeNode> children;
            public List<SkillTreeNode> parents;
            public List<SkillTreeNode> dependencies;
            public List<SkillTreeNode> dependentOnMe;
            public Vector2 drawPos;
            public Point gridIdx;

            public SkillID skillID;
            public bool unlocked;
            public bool hidden;

            public Texture2D myPlate;
            public Texture2D myIcon;

            public SkillTreeNode(Point gridIdx, SkillID skillID, bool hidden = true, bool unlocked = false, Texture2D myIcon = null, Texture2D myPlate = null)
            {
                this.gridIdx = gridIdx;
                this.skillID = skillID;
                this.unlocked = unlocked;
                this.hidden = hidden;

                this.myIcon = myIcon;
                this.myPlate = myPlate;

                children = new List<SkillTreeNode>();
                parents = new List<SkillTreeNode>();
                dependencies = new List<SkillTreeNode>();
                dependentOnMe = new List<SkillTreeNode>();


            }

            public SkillTreeNode AddDependency(SkillTreeNode them)
            {
                dependencies.Add(them);
                them.dependentOnMe.Add(this);
                return this;
            }

            public SkillTreeNode AddEdge(SkillTreeNode child)
            {

                child.parents.Add(this);
                return this;
            }

            public SkillTreeNode AddChild(Point gridIdx, SkillID skillID, List<SkillTreeNode> nodeList, Texture2D myIcon = null, Texture2D myPlate = null)
            {
                SkillTreeNode child = new SkillTreeNode(gridIdx, skillID, true, false, myIcon, myPlate);

                child.parents.Add(this);
                children.Add(child);

                nodeList[(int)child.skillID] = child;
                return child;
            }
            public SkillTreeNode AddChildDirection(Point direction, SkillID skillID, List<SkillTreeNode> nodeList, Texture2D myIcon = null, Texture2D myPlate = null)
            {
                SkillTreeNode child = new SkillTreeNode(gridIdx + direction, skillID, true, false, myIcon, myPlate);
                child.parents.Add(this);

                children.Add(child);
                nodeList[(int)child.skillID] = child;
                return child;
            }

            public void ChangeLockState()
            {
                if (unlocked)
                {
                    Relock();
                } else
                {
                    Unlock();
                }
            }

            public void RelockNoParent()
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
            public void Relock() {
                unlocked = false;
                for (int i = 0; i < dependentOnMe.Count; i++)
                {
                    dependentOnMe[i].Relock();
                }

               
                foreach (SkillTreeNode child in children)
                {
                    child.Relock();
                }
                
                
            }

            public void Unlock()
            {
                unlocked = true;
                foreach (SkillTreeNode child in children)
                {
                    child.hidden = false;
                }
                
            }

            public bool Unlockable()
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
            public void Unlock(bool state)
            {
                if (!unlocked && state)
                {
                    Unlock();
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
                SkillTreeNode ret = new SkillTreeNode(new Point(0, 0), SkillID.None);
                ret.unlocked = tag.GetBool("unlocked");
                ret.hidden = tag.GetBool("hidden");
                return ret;
            }

            public string GetDescription()
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
            skillTreeBackground = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/SkTree/Assets/SkillTreeBackground").Value;
            skillTreePlate = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/SkTree/Assets/SkillTreePlateBorder").Value;
            bowIcon = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/SkTree/Assets/PlaceholderBow").Value;
            doubleBowIcon = ModContent.Request<Texture2D>("EthoriaMod/Content/UI/SkTree/Assets/PlaceholderBow2").Value;

            Point left = new Point(-1, 0);
            Point right = new Point(1, 0);
            Point up = new Point(0, 1);
            Point down = new Point(0, -1);

            

            this.nodeDist = nodeDist;
            this.squareSize = nodeDist;


            root = new SkillTreeNode(new Point(0, 0), SkillID.Start, false);
            nodeList = Enumerable.Repeat((SkillTreeNode)null, (int) SkillID.EnumSize).ToList();
            nodeList[(int) root.skillID] = root;






            // Do Not Edit Above This Line!
            // Step1 Name a node Ex: SkillTreeNode root = new SkillTreeNode(new Point(0, 0), SkillID.Start, false);
            // Step2 Add children to the node there are two ways to do this
            // 1 - AddChildDirection(direction, SkillID, nodeList, textureIcon, plateIcon); Adds child at the position parent.gridIdx + direction 
         
            // 2 - AddChild(gridIdx, SkillId, nodeList, textureIcon, plateIcon); Adds child at raw position gridIdx

            // See code after this for examples


            root.AddChildDirection(left, SkillID.Warrior, nodeList);
            SkillTreeNode ranger = root.AddChildDirection(right, SkillID.Ranger, nodeList);
            SkillTreeNode quickDraw = ranger.AddChildDirection(right, SkillID.Quickdraw, nodeList);
            ranger.AddChildDirection(right,SkillID.Precision, nodeList);

            quickDraw.AddChildDirection(up, SkillID.LoadedShot, nodeList);

            SkillTreeNode dmgBoost1 = quickDraw.AddChildDirection(right, SkillID.DmgBoost1, nodeList); 

            SkillTreeNode doubleShot = dmgBoost1.AddChildDirection(right, SkillID.DoubleShot, nodeList, doubleBowIcon);
            SkillTreeNode velocity = dmgBoost1.AddChildDirection(right, SkillID.Velocity, nodeList);
            

            //doubleShot.addEdge(velocity);






            root.AddChildDirection(up, SkillID.Mage, nodeList);
            SkillTreeNode summoner = root.AddChildDirection(down, SkillID.Summoner, nodeList);



            updatePositions();
        }
        public void DrawSkillTree(SpriteBatch spriteBatch, Vector2 displacement, Vector2 cutoutPosition, Vector2 windowPosition, Rectangle backgroundRect)
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
                if (backgroundRect.Contains(new Point(Main.mouseX, Main.mouseY)) && nodeRect.Contains(new Point(Main.mouseX , Main.mouseY)) && curr.Unlockable())
                {
                    //MouseStrUI.mouseStr = curr.getDescription();

                    Main.instance.MouseText(curr.GetDescription());
                    color = Color.Yellow;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        
                        curr.ChangeLockState();
                    }
                }

                if (curr.unlocked)
                {
                    color = Color.Green;
                }

                foreach (SkillTreeNode child in curr.children)
                {
                    if (child.hidden)
                    {
                        continue;
                    }
                    int childDrawXScreen = (int)(Main.screenWidth * (child.drawPos.X + displacement.X));
                    int childDrawYScreen = (int)(Main.screenHeight * (child.drawPos.Y + displacement.Y));
                    HelperFunctions.drawLine(spriteBatch, new Vector2(drawXScreen, drawYScreen), new Vector2(childDrawXScreen, childDrawYScreen), Color.Black, 2);

                    queue.Enqueue(child);
                }
                

                spriteBatch.Draw(skillTreeBackground, rect, color);
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
        public void updatePositions()
        {
            
            foreach (SkillTreeNode node in nodeList)
            {
                if (node == null) continue;
                // (0, 0) -> midx, midy
                float dx = (float) (squareSize * node.gridIdx.X) / (float) Main.screenWidth;
                float dy = (float) (squareSize * node.gridIdx.Y) / (float) Main.screenHeight;

                node.drawPos = new Vector2(0.5f + dx , 0.5f + dy); 
            }
        }


        public void UpdateSkillEffects()
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
                    l[i].Unlock(savedNodes[i].unlocked);
                    l[i].hidden = savedNodes[i].hidden; 
                }
            }
            return ret;

        }

        public static Func<TagCompound, SkillTree> DESERIALIZER = Load;

    }
}
