using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EthoriaMod.Content
{

    
    
    public class SkillTree
    {

        public SkillTreeNode root;
        public int dist;

        public class SkillTreeNode
        {
            public List<SkillTreeNode>[GrowDirection.enumSize] Children;
            public List<SkillTreeNode> Parents;
            public int drawX, drawY;
            public string skillName;
            public bool unlocked;
            public GrowDirection growDirection;
            
            public enum GrowDirection
            {
                None,
                Left,
                Up,
                Right,
                Down,
                enumSize
            }
            public SkillTreeNode(int drawX, int drawY, string skillName, GrowDirection growDirection = GrowDirection.None, bool unlocked = false)
            {
                this.drawX = drawX;
                this.drawY = drawY;
                this.skillName = skillName;
                this.unlocked = unlocked;
                this.growDirection = growDirection;
            }

            public SkillTreeNode(string skillName, GrowDirection growDirection = GrowDirection.None, bool unlocked = false)
            {
                this.drawX = 0;
                this.drawY = 0;
                this.skillName = skillName;
                this.unlocked = unlocked;
                this.growDirection = growDirection;
            }

            public SkillTreeNode addChild(string skillName)
            {
                return this.addChild(skillName, growDirection);
            }

            public SkillTreeNode addChild(string skillName, GrowDirection direction)
            {
                SkillTreeNode child = new SkillTreeNode(skillName, direction);
                Children[direction].Add(child);
                child.Parents.Add(this);
                return child;
            }

            
            public void unlock()
            {
                unlocked = true;
            }
        }


        public void updateChildrenPositions()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                
            }
        }


        public SkillTree(int dist = 10)
        {
            this.dist = dist;

            root =  new SkillTreeNode(0, 0, "Start", SkillTreeNode.GrowDirection.None, true);

            root.addChild("Warrior", SkillTreeNode.GrowDirection.Left);
            root.addChild("Ranger", SkillTreeNode.GrowDirection.Right);
            root.addChild("Mage", SkillTreeNode.GrowDirection.Up);
            root.addChild("Summoner", SkillTreeNode.GrowDirection.Down);


            this.updateChildrenPosition();
        }
        

        

    }
}
