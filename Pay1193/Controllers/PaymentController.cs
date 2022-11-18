using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Pay1193.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IPayService _payService;
        private readonly IEmployee _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceService _nationalInsuranceService;
        private decimal overtimeHrs;
        private decimal contractualEarnings;
        private decimal overtimeEarnings;
        private decimal totalEarnings;
        private decimal tax;
        private decimal unionFee;
        private decimal studentLoan;
        private decimal nationalInsurance;
        private decimal totalDeduction;
        public PaymentController(IPayService payService, IEmployee employeeService, ITaxService taxService, INationalInsuranceService nationalInsuranceService)
        {
            _payService = payService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceService = nationalInsuranceService;
        }

        public IActionResult Index()
        {
            var payRecords = _payService.GetAll().Select(pay => new PaymentRecordIndexViewModel
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                FullName = pay.FullName,
                PayDate = pay.DatePay,
                PayMonth = pay.MonthPay,
                TaxYearId = pay.TaxYearId,
                Year = _payService.GetTaxYearById(pay.TaxYearId).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeductions = pay.EarningDeduction,
                NetPayment = pay.NetPayment,
                Employee = pay.Employee
            });
            Console.WriteLine(payRecords);
            return View(payRecords);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payService.GetAllTaxYears();
            var model = new PaymentRecordCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            Console.WriteLine(model);
            if (ModelState.IsValid)
            {
                var payment = new PaymentRecord
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    FullName = _employeeService.GetById(model.EmployeeId).FullName,
                    DatePay = model.PayDate,
                    MonthPay = model.PayMonth,
                    TaxYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HourWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours = overtimeHrs = _payService.OvertimeHours(model.HoursWorked, model.ContractualHours),
                    ContractualEarnings = contractualEarnings = _payService.ContractualEarnings(model.ContractualHours, model.HoursWorked, model.HourlyRate),
                    OvertimeEarnings = overtimeEarnings = _payService.OvertimeEarnings(_payService.OvertimeRate(model.HourlyRate), overtimeHrs),
                    TotalEarnings = totalEarnings = _payService.TotalEarnings(overtimeEarnings, contractualEarnings),
                    Tax = tax = _taxService.TaxAmount(totalEarnings),
                    UnionFee = unionFee = _employeeService.UnionFees(model.EmployeeId),
                    SLC = studentLoan = _employeeService.StudentLoanRepaymentAmount(model.EmployeeId, totalEarnings),
                    NiC = nationalInsurance = _nationalInsuranceService.NIContribution(totalEarnings),
                    EarningDeduction = totalDeduction = _payService.TotalDeduction(tax, nationalInsurance, studentLoan, unionFee),
                    NetPayment = _payService.NetPay(totalEarnings, totalDeduction)
                };
                await _payService.CreateAsync(payment);
                return RedirectToAction("Index");
            }
            ViewBag.employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payService.GetAllTaxYears();
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var payment = _payService.GetById(id);
            if (payment == null)
            {
                return NotFound();
            }

            var detailModel = new PaymentRecordDetailViewModel()
            {
                Id = payment.Id,
                EmployeeId = payment.EmployeeId,
                FullName = payment.FullName,
                NiNo = _employeeService.GetById(payment.EmployeeId).NationalInsuranceNo,
                PayDate = payment.DatePay,
                PayMonth = payment.MonthPay,
                TaxYearId = payment.TaxYearId,
                Year = _payService.GetTaxYearById(payment.TaxYearId).YearOfTax,
                TaxCode = payment.TaxCode,
                HourlyRate = payment.HourlyRate,
                HoursWorked = payment.HourWorked,
                ContractualHours = payment.ContractualHours,
                OvertimeHours = payment.OvertimeHours,
                OvertimeEarnings = payment.OvertimeEarnings,
                OvertimeRate = _payService.OvertimeRate(payment.HourlyRate),
                ContractualEarnings = payment.ContractualEarnings,
                Tax = payment.Tax,
                NIC = payment.NiC,
                UnionFee = payment.UnionFee,
                SLC = payment.SLC,
                TotalEarnings = payment.TotalEarnings,
                TotalDeductions = payment.EarningDeduction,
                Employee = payment.Employee,
                TaxYear = payment.TaxYear,
                NetPayment = payment.NetPayment
            };
            return View(detailModel);
        }
    }
}
