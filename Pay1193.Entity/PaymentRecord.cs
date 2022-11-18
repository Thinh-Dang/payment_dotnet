using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Entity
{
    public class PaymentRecord
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Display(Name = "Name")]
        public string? FullName { get; set; }
        public DateTime DatePay { get; set; }
        public string? MonthPay { get; set; }
        [ForeignKey("TaxYear")]
        public int TaxYearId { get; set; }
        public TaxYear TaxYear { get; set; }

        public string TaxCode { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal HourlyRate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourWorked { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ContractualHours { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OvertimeHours { get; set; }
        [Column(TypeName = "money")]
        public decimal ContractualEarnings { get; set; }
        [Column(TypeName = "money")]
        public decimal OvertimeEarnings { get; set; }
        [Column(TypeName = "money")]
        public decimal NiC { get; set; }
        [Column(TypeName = "money")]
        public decimal Tax { get; set; }
        [Column(TypeName = "money")]
        public decimal UnionFee { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> SLC { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalEarnings { get; set; }
        [Column(TypeName = "money")]
        public decimal EarningDeduction { get; set; }
        [Column(TypeName = "money")]
        public decimal NetPayment { get; set; }
    }
}
