using System;

namespace Mintchobab
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NodeFieldAttribute : Attribute
    {
        public NodeFieldAttribute() { }
    }
}
