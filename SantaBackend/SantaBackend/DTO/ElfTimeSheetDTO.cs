namespace SantaBackend.DTO
{
    public class ElfTimeSheetDTO
    {
        public int elfID { get; set; }
        public int userID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int roleID { get; set; }
        public string role { get; set; }
        public int shiftID { get; set; }
        public DateOnly date { get; set; }
        public int shiftTypeID { get; set; }
        public string type { get; set; }
        public TimeOnly shiftStartTime { get; set; }
        public TimeOnly shiftEndTime { get; set; }
        public int blockID { get; set; }
        public DateTime unavailableStartTime { get; set; }
        public DateTime unavailableEndTime { get; set; }

        public ElfTimeSheetDTO()
        {
        }
    }
}
