using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EthoriaMod.Content
{

    
    public class SkillTreeNode
    {
        public List<SkillTreeNode> Children;
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

        public SkillTreeNode addChild(string skillName)
        {
            SkillTreeNode child = new SkillTreeNode(0, 0, skillName, this.growDirection);
            Children.Add(child);
            child.Parents.Add(this);

            return child;
        }
        public SkillTreeNode addChild(string skillName, GrowDirection direction)
        {
            SkillTreeNode child = new SkillTreeNode(this.drawX, this.drawY, skillName, direction);
            Children.Add(child);
            child.Parents.Add(this);
            
            return child;
        }

        public void updateChildrenPositions()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                
            }
        }

        public void unlock()
        {
            unlocked = true;
        }
    }
    public class SkillTree
    {



        public SkillTreeNode root;
        public SkillTree()
        {
            root =  new SkillTreeNode(0, 0, "Start", SkillTreeNode.GrowDirection.None, true);

            root.addChild("Warrior", SkillTreeNode.GrowDirection.Left);
            root.addChild("Ranger", SkillTreeNode.GrowDirection.Right);
            root.addChild("Mage", SkillTreeNode.GrowDirection.Up);
            root.addChild("Summoner", SkillTreeNode.GrowDirection.Down);
        }
        

        

    }
}
