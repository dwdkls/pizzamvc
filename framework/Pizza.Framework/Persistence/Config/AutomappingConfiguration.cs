using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using Pizza.Framework.Utils;
using Pizza.Persistence.Attributes;

namespace Pizza.Framework.Persistence.Config
{
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Member member)
        {
            return member.IsProperty && member.CanWrite && base.ShouldMap(member);
        }

        public override bool IsComponent(Type type)
        {
            return type.GetAttribute<ComponentAttribute>() != null;
        }

        public override string GetComponentColumnPrefix(Member member)
        {
            return member.Name + "_";
        }
    }
}