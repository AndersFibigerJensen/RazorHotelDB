using Microsoft.Data.SqlClient;
using RazorHotelDB.Models;
using System;

namespace RazorHotelDB.Services
{
    public abstract class Connection
    {
        protected String connectionString;
        public IConfiguration Configuration { get; }

        public Connection(IConfiguration configuration)
        {
            connectionString = Secret.Connectionstring;
            Configuration = configuration;
            //connectionString = Configuration["ConnectionStrings:SimplyConnection"];
            //connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public Connection(string connectionstring)
        {
            Configuration = null;
            this.connectionString= connectionstring;
        }
        

    }
}
