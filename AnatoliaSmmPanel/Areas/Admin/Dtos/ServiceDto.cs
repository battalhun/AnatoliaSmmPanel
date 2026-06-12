namespace AnatoliaSmmPanel.Areas.Admin.Dtos
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public decimal Rate { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public bool IsActive { get; set; }

        public string CategoryName { get; set; }
        public string ProviderName { get; set; }

        public string ExternalServiceId { get; set; }
        public decimal ExternalRate { get; set; }
        public int ExternalMin { get; set; }
        public int ExternalMax { get; set; }

        public bool Dripfeed { get; set; }
        public bool Cancel { get; set; }
        public bool Refill { get; set; }
    }
}
