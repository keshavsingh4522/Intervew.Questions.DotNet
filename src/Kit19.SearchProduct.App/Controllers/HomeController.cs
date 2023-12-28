using Kit19.SearchProduct.App.Context;
using Kit19.SearchProduct.App.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Pipelines;

namespace Kit19.SearchProduct.App.Controllers;

public class HomeController : Controller
{
    private readonly Kit19DbContext _context;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, Kit19DbContext kit19DbContext)
    {
        _logger = logger;
        _context = kit19DbContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Search()
    {
        return View(new ProductSearchViewModel());
    }

    [HttpPost]
    public IActionResult Search(ProductSearchViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Call the stored procedure and pass the search criteria
            // For simplicity, let's assume we're using ADO.NET here.
            using (var conn = new MySqlConnection("server=127.0.0.1; database=Kit19;user=root;password=''"))
            {

                using (var cmd = new MySqlCommand("sp_SearchProducts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters using MySqlConnector types

                    // Add named parameters
                    cmd.Parameters.AddWithValue("@p_ProductName", model.ProductName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Size", model.Size ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Price", model.Price.HasValue ? (object)model.Price.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_MfgDate", model.MfgDate.HasValue ? (object)model.MfgDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Category", model.Category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Conjunction", model.Conjunction ?? "AND");


                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        // List to hold the results
                        List<Product> products = new List<Product>();

                        while (reader.Read())
                        {
                            // Process the results
                            // Create a new Product instance
                            var product = new Product
                            {
                                // Use the appropriate data type conversion methods
                                // such as GetString, GetInt32, GetDecimal, etc.
                                // and handle possible DBNull values
                                ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? null : reader.GetString(reader.GetOrdinal("ProductName")),
                                Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString(reader.GetOrdinal("Size")),
                                Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Price")),
                                MfgDate = reader.IsDBNull(reader.GetOrdinal("MfgDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MfgDate")),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category"))
                            };

                            // Add the Product instance to the list
                            products.Add(product);
                        }
                        model.SearchResults = products;
                    }
                }
            }
        }
        return View(model);
    }
}
