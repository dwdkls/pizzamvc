using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using Pizza.Contracts.Persistence.Attributes;
using Pizza.Framework.Utils;

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