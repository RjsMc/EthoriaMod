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

        public SkillTreeNode(int drawX, int drawY, string skillName, bool unlocked = false)
        {
            this.drawX = drawX;
            this.drawY = drawY;
            this.skillName = skillName;
            this.unlocked = unlocked;
        }

        public SkillTreeNode addChild(string skillName)
        {
            SkillTreeNode child = new SkillTreeNode(0, 0, skillName);
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


        public SkillTreeNode root = new SkillTreeNode(0, 0, "Start", true);

        

        

    }
}
