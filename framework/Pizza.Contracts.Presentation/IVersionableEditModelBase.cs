namespace Pizza.Contracts.Presentation
{
    public interface IVersionableEditModelBase : IEditModelBase
    {
        byte[] Version { get; set; }
    }
}