using Pizza.Contracts.Presentation.Attributes;

namespace KebabManager.Contracts.Common
{
    public enum CustomerType
    {
        [EnumDisplayName("Individual customer")]
        Individual,
        [EnumDisplayName("Enterprise customer")]
        Enterprise,
        [EnumDisplayName("Government customer")]
        Government,
    }

    public enum AnimalSpecies
    {
        [EnumDisplayName("Cat")]
        Cat,
        [EnumDisplayName("Dog")]
        Dog,
        [EnumDisplayName("Frozen chicken")]
        FrozenChicken,
    }
}
