﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.ExternalService;
using PaymentGateway.WebApi.Swagger;

namespace PaymentGateway.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(o => o.EnableEndpointRouting = false);

            //services.AddSingleton<IEventSender, EventSender>();
            var firstAssembly = typeof(ListOfAccounts).Assembly; // handlere c1..c3
            var secondAssembly = typeof(AllEventsHandler).Assembly; // catch all
            services.AddMediatR(firstAssembly, secondAssembly); // get all IRequestHandler and INotificationHandler classes

            /*services.AddTransient<CreateAccount>();

            //services.AddSingleton<AccountOptions>(new AccountOptions { InitialBalance = 200 });
            services.AddSingleton<AccountOptions>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var options = new AccountOptions
                {
                    InitialBalance = config.GetValue("AccountOptions:InitialBalance", 0)
                };
                return options;
            });

            //services.Configure<AccountOptions>(Configuration.GetSection("AccountOptions"));
            */
            services.RegisterBusinessServices(Configuration);
            services.AddSwagger(Configuration["Identity:Authority"]);

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {

            app.UseCors(cors =>
            {
                cors.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
#pragma warning disable MVC1005 // Cannot use UseMvc with Endpoint Routing.
            app.UseMvc();
#pragma warning restore MVC1005 // Cannot use UseMvc with Endpoint Routing.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment gateway Api");
                //c.OAuthClientId("CharismaFinancialServices");
                //c.OAuthScopeSeparator(" ");
                c.EnableValidator(null);
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
