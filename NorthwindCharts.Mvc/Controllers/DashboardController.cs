﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindCharts.Database.AppDbContextModels;
using NorthwindCharts.Mvc.Dtos;

namespace NorthwindCharts.Mvc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBestSellingProducts()
        {
            var sqlQuery = @"
                SELECT TOP 5
                    P.ProductName,
                    SUM(OD.Quantity) AS TotalQuantity
                FROM
                    [Order Details] OD
                JOIN
                    Products P ON OD.ProductID = P.ProductID
                GROUP BY
                    P.ProductName
                ORDER BY
                    TotalQuantity DESC";

            var topProducts=await _db.Database.SqlQueryRaw<BestSellingProductDto>(sqlQuery)
                .ToListAsync();
            return Json(topProducts);
        }
    }
}
