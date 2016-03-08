using Pizza.Contracts.Attributes;

namespace KebabManager.Common.Enums
{
    public enum AnimalSpecies
    {
        [EnumDisplayName("Cat")]
        Cat,
        [EnumDisplayName("Dog")]
        Dog,
        [EnumDisplayName("Frozen chicken")]
        FrozenChicken,
        [EnumDisplayName("Fish and chips")]
        Fish,
        [EnumDisplayName("Big bird")]
        BigBird
    }
}