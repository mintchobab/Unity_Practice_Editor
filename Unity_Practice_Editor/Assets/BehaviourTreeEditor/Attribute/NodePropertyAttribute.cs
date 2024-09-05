using System;

namespace Mintchobab 
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NodePropertyAttribute : Attribute
    {
        public NodePropertyAttribute() { }
    }
}
