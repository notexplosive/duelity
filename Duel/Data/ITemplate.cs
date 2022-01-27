namespace Duel.Data
{
    public interface ITemplate
    {
        TagCollection Tags { get; }
        string NameInLibrary { get; set; }
    }
}
