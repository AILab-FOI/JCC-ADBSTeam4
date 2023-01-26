namespace SantaBackend.DTO
{
    public class SantaDetailDTO
    {
        public int santaClausID { get; set; }
        public int userID { get; set; }
        public List<WorkshopDTO> workshops { get; set; } = new List<WorkshopDTO>();
        public string firstName { get; set; }
        public string lastName { get; set; }

        public SantaDetailDTO()
        {
        }
    }

}
