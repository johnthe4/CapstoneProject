namespace CapstoneProject.Models {
    public class PO {
        public Vendor Vendor { get; set; }
        public IEnumerable<Poline> polines { get; set; }
        public decimal PoTotal { get; set; }
    }
}
