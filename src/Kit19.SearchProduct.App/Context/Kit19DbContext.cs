using Kit19.SearchProduct.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Kit19.SearchProduct.App.Context;

public class Kit19DbContext: DbContext
{
    public Kit19DbContext(DbContextOptions<Kit19DbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}
