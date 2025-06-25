namespace Interfaces
{
    public interface ISavable
    {
        public string SaveId { get; }
        public string Save();

        public void Load(string json);
    }
}