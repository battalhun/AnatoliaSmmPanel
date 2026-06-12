using AnatoliaSmmPanel.Models;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Data.Models.Admin;

namespace AnatoliaSmmPanel.Areas.Admin.Data;

public class AdminContext : DbContext
{
    

    public AdminContext(DbContextOptions<AdminContext> options)
        : base(options)
    {
    }

   
}