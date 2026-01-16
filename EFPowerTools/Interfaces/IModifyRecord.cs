namespace EFPowerTools.Interfaces
{
    internal interface IModifyRecord
    {
        public Guid CreateUserId { get; set; }

        public DateTime CreateTime { get; set; }

        public Guid UpdateUserId { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
