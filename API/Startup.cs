using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          services.AddDbContext<DataContext>(opt =>
        {
           opt.UseInMemoryDatabase("Books");
        });
       
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,DataContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

         context.Database.EnsureCreated();

       if (context.Books.Any() == false)
    {
        Book Lotr = new Book { Title = "Lord of the Rings",Description = "Legolas doing skateboard shoots."
       ,Author = "J.R.R Tolkien",Price = 250.00M, CoverImage = "lotr.jpg" };
        Book ChildhoodsEnd = new Book { Title = @"Childhood's End",Description = "Epic Sci-fi classic"
       ,Author = "Arthur C. Clarke",Price = 300.00M, CoverImage = "childhood.jpg" };
        Book DragonLance = new Book { Title = "Dragon Lance:Dragons of Autumn Sun",Description = "Dragon Lives don't matter."
       ,Author = "Margaret Weis",Price = 250.00M, CoverImage = "dragonlance.jpg" };
        Book Ubik = new Book { Title = "Ubik",Description = "Minds are blown."
       ,Author = "Philip K. Dick",Price = 250.00M, CoverImage = "ubik.jpg" };

        context.Add(Lotr);
        context.Add(ChildhoodsEnd);
        context.Add(DragonLance);
        context.Add(Ubik);
        context.SaveChanges();

    }
          //  app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
