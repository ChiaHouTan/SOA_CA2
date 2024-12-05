namespace SOA_CA2.Models
{
    public class ApiKey
    {
        public string Key { get; set; }
        public string Role { get; set; } // "Public" or "Admin"
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
