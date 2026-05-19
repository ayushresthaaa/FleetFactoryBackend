using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class ReportRepository(AppDbContext _context) : IReportRepository
    {
        public async Task<DashboardDTO> GetAdminDashboardAsync()
        {
            var today = DateTime.UtcNow.Date;
            var monthStart = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var totalRevenue = await _context.SalesInvoices
                .Where(s => s.CreatedAt >= monthStart && s.CreatedAt <= DateTime.UtcNow)
                .SumAsync(s => s.TotalAmount);

            var totalPurchases = await _context.PurchaseInvoices
                .Where(p => p.CreatedAt >= monthStart && p.CreatedAt <= DateTime.UtcNow)
                .SumAsync(p => p.TotalAmount);

            var customerCount = await _context.CustomerProfiles.CountAsync();

            var lowStockCount = await _context.Parts
                .CountAsync(p => p.StockQty < 10 && p.IsActive);

            var pendingAppointments = await _context.Appointments
                .CountAsync(a => a.Status == AppointmentStatus.Pending);

            return new DashboardDTO
            {
                Cards =
                {
                    new SummaryCardDTO { Label = "Monthly Revenue", Value = totalRevenue },
                    new SummaryCardDTO { Label = "Monthly Purchases", Value = totalPurchases },
                    new SummaryCardDTO { Label = "Estimated Profit", Value = totalRevenue - totalPurchases },
                    new SummaryCardDTO { Label = "Customers", Value = customerCount },
                    new SummaryCardDTO { Label = "Low Stock Parts", Value = lowStockCount },
                    new SummaryCardDTO { Label = "Pending Appointments", Value = pendingAppointments }
                }
            };
        }

        public async Task<DashboardDTO> GetStaffDashboardAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var todayAppointments = await _context.Appointments
                .CountAsync(a => a.ScheduledAt >= today && a.ScheduledAt < tomorrow);

            var pendingCredits = await _context.CustomerProfiles
                .CountAsync(c => c.CreditBalance > 0);

            var todaySales = await _context.SalesInvoices
                .Where(s => s.CreatedAt >= today && s.CreatedAt < tomorrow)
                .SumAsync(s => s.TotalAmount);

            var invoiceCount = await _context.SalesInvoices
                .CountAsync(s => s.CreatedAt >= today && s.CreatedAt < tomorrow);

            return new DashboardDTO
            {
                Cards =
                {
                    new SummaryCardDTO { Label = "Today Appointments", Value = todayAppointments },
                    new SummaryCardDTO { Label = "Pending Credit Customers", Value = pendingCredits },
                    new SummaryCardDTO { Label = "Today Sales", Value = todaySales },
                    new SummaryCardDTO { Label = "Today Invoice Count", Value = invoiceCount }
                }
            };
        }

        public async Task<FinancialSummaryDTO> GetFinancialSummaryAsync(DateTime fromDate, DateTime toDate)
        {
            var totalRevenue = await _context.SalesInvoices
                .Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate)
                .SumAsync(s => s.TotalAmount);

            var totalPurchases = await _context.PurchaseInvoices
                .Where(p => p.CreatedAt >= fromDate && p.CreatedAt <= toDate)
                .SumAsync(p => p.TotalAmount);

            var salesCount = await _context.SalesInvoices
                .CountAsync(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate);

            var purchaseCount = await _context.PurchaseInvoices
                .CountAsync(p => p.CreatedAt >= fromDate && p.CreatedAt <= toDate);

            return new FinancialSummaryDTO
            {
                FromDate = fromDate,
                ToDate = toDate,
                TotalRevenue = totalRevenue,
                TotalPurchases = totalPurchases,
                NetProfit = totalRevenue - totalPurchases,
                SalesInvoiceCount = salesCount,
                PurchaseInvoiceCount = purchaseCount
            };
        }
        public async Task<List<ChartPointDTO>> GetRevenueTrendAsync(
            DateTime fromDate,
            DateTime toDate,
            string groupBy
        )
        {
            var query = _context.SalesInvoices
                .Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate);

            if (groupBy.ToLower() == "month")
            {
                var monthlyData = await query
                    .GroupBy(s => new
                    {
                        s.CreatedAt.Year,
                        s.CreatedAt.Month
                    })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        Revenue = g.Sum(x => x.TotalAmount)
                    })
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToListAsync();

                return monthlyData
                    .Select(x => new ChartPointDTO
                    {
                        Label = $"{x.Year}-{x.Month:D2}",
                        Value = x.Revenue
                    })
                    .ToList();
            }

            var dailyData = await query
                .GroupBy(s => s.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return dailyData
                .Select(x => new ChartPointDTO
                {
                    Label = x.Date.ToString("yyyy-MM-dd"),
                    Value = x.Revenue
                })
                .ToList();
        }

        public async Task<List<ReportRowDTO>> GetTopSellingPartsAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.SalesInvoiceItems
                .Where(i => i.SalesInvoice.CreatedAt >= fromDate && i.SalesInvoice.CreatedAt <= toDate)
                .GroupBy(i => i.Part.Name)
                .Select(g => new ReportRowDTO
                {
                    Name = g.Key,
                    Count = g.Sum(x => x.Quantity),
                    Value = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<ChartPointDTO>> GetPaymentMethodsAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.SalesInvoices
                .Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate && s.PaymentMethod != null)
                .GroupBy(s => s.PaymentMethod)
                .Select(g => new ChartPointDTO
                {
                    Label = g.Key.ToString()!,
                    Value = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();
        }

        public async Task<SummaryCardDTO> GetProfitEstimateAsync(DateTime fromDate, DateTime toDate)
        {
            var summary = await GetFinancialSummaryAsync(fromDate, toDate);

            return new SummaryCardDTO
            {
                Label = "Estimated Profit",
                Value = summary.NetProfit
            };
        }

        public async Task<List<ReportRowDTO>> GetHighSpendersAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.SalesInvoices
                .Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate)
                .GroupBy(s => s.Customer.FullName)
                .Select(g => new ReportRowDTO
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Value = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<ReportRowDTO>> GetRegularCustomersAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.SalesInvoices
                .Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate)
                .GroupBy(s => s.Customer.FullName)
                .Select(g => new ReportRowDTO
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Value = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<PendingCreditDTO>> GetPendingCreditsAsync()
        {
            return await _context.CustomerProfiles
                .Where(c => c.CreditBalance > 0)
                .Select(c => new PendingCreditDTO
                {
                    CustomerId = c.Id,
                    CustomerName = c.FullName,
                    Phone = c.Phone,
                    CreditBalance = c.CreditBalance
                })
                .OrderByDescending(x => x.CreditBalance)
                .ToListAsync();
        }

        public async Task<List<ReportRowDTO>> GetFrequentVehiclesAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.Appointments
                .Where(a => a.ScheduledAt >= fromDate && a.ScheduledAt <= toDate && a.Vehicle != null)
                .GroupBy(a => a.Vehicle!.VehicleNumber)
                .Select(g => new ReportRowDTO
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Value = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<ChartPointDTO>> GetAppointmentStatsAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.Appointments
                .Where(a => a.ScheduledAt >= fromDate && a.ScheduledAt <= toDate)
                .GroupBy(a => a.Status)
                .Select(g => new ChartPointDTO
                {
                    Label = g.Key.ToString(),
                    Value = g.Count()
                })
                .ToListAsync();
        }
    }
}