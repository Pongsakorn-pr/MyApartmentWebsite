namespace webapi.Model
{
    public class Apartment
    {
        public int bill_id { get; set; }  
        public string room_number { get; set; }
        public DateTime bill_month_year { get; set; }
        public decimal room_rent { get; set; }
        public int water_reading_meter { get; set; }
        public int water_unit_fees { get; set; }
        public int garbage_fees { get; set; }
        public decimal other_fees { get; set; }
        public int previous_meter_month { get; set; }
        public int water_diff { get; set; }
        public decimal total_amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }  
    }
}
