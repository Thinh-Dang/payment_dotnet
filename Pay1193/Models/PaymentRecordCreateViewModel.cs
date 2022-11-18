using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;

namespace Pay1193.Models
{
    public class PaymentRecordCreateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Employee is required")]
        [RegularExpression(@"[0-9]")]
        [Display(Name = "Full Name")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public string? FullName { get; set; }
        public string? NiNo { get; set; }
        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "Pay Month")]
        public string PayMonth { get; set; } = DateTime.Today.Month.ToString();
        [Display(Name = "Tax Year")]
        [RegularExpression(@"[0-9]")]
        public int TaxYearId { get; set; }
        public string TaxCode { get; set; } = "1250L";
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }
        [Display(Name = "Contractual Hours")]
        public decimal ContractualHours { get; set; } = 144m;
        public decimal OvertimeHours { get; set; }
        public decimal ContractualEarnings { get; set; }
        public decimal OvertimeEarnings { get; set; }
        public decimal Tax { get; set; }
        public decimal NIC { get; set; }
        public decimal? UnionFee { get; set; }
        public decimal? SLC { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }
    }
}
