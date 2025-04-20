namespace ChatSample.Infrastructures.BaseModules
{
    public class EntityBase 
    {
        public Guid userId {  get; set; } = Guid.NewGuid();

        public DateTime creation {  get; set; } = DateTime.UtcNow;

    }
}
