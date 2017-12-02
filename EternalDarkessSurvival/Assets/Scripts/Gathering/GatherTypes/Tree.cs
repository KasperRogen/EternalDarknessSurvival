namespace Gathering.GatherTypes
{
    public class Tree : Gatherable
    {
        void Start()
        {
            resourceType = PublicEnums.ResourceType.Wood;
            resourceCount = 40;
        }
    }
}