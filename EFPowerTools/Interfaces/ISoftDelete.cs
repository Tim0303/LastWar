namespace EFPowerTools.Interfaces
{
    internal interface ISoftDelete
    {
        bool IsDeleted { get; set; }

        public Guid? DeleteUserId { get; set; }

        DateTime? DeletedTime { get; set; }
    }
}
