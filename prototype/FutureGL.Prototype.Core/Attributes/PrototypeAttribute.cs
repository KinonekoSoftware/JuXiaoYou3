using System;

namespace FutureGL.Prototype.Core.Attributes
{
   public enum PrototypeKind
   {
      Model,
      Page,
      Interaction,
      PreDesign
   }
   
   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
   public sealed class PrototypeAttribute : Attribute
   {
      public PrototypeAttribute(string group, string name, PrototypeKind kind)
      {
         GroupName = group;
         Name      = name;
         Kind      = kind;
      }
      
      
      public string GroupName { get;  }
      public string Name { get;  }
      public PrototypeKind Kind { get; }
   }
}