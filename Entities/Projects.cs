namespace PlataformaMarcenaria.API.Entities
{
    public class Project
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long ClientId { get; set; }
        public string Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string AddressStreet { get; set; }
        public string AddressNumber { get; set; }
        public string AddressComplement { get; set; }
        public string AddressNeighborhood { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressZipCode { get; set; }
        public List<string> Requirements { get; set; }
        public List<string> Images { get; set; }
    }
}
