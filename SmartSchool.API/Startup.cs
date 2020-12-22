using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartSchool.API.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API
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
      services.AddDbContext<SmartContext>(
          context => context.UseSqlite(Configuration.GetConnectionString("Default"))
      );

      //Mapeamento entre DTO e Models via reflection
      //Com esta linha é feita uma injeção de dependência que é recebido pela controller
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      //Necessário quando você tem um Loop em um Json. Aluno dentro de discpli e disciplina dentro de aluno, por exemplo.
      services.AddControllers().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );
      services.AddControllers();

      //Injeção de dependencia que pode ser recebido como parâmetro na controller
      services.AddScoped<IRepository, Repository>();

      services.AddVersionedApiExplorer(options =>
      {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
      }).AddApiVersioning(options =>
      {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
      });

      var apiProviderDescription = services.BuildServiceProvider()
                            .GetService<IApiVersionDescriptionProvider>();

      services.AddSwaggerGen(
        options =>
        {
          foreach (var description in apiProviderDescription.ApiVersionDescriptions)
          {
            options.SwaggerDoc(
              description.GroupName,
              new Microsoft.OpenApi.Models.OpenApiInfo()
              {
                Title = "SmartSchool API",
                Version = description.ApiVersion.ToString(),
                Description = "WebAPI desenvolvida a partir de um curso da Udemy",
                License = new Microsoft.OpenApi.Models.OpenApiLicense
                {
                  Name = "SmartSchool License",
                  Url = new Uri("http://mit.com")
                },
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                  Name = "Ricardo Colzani",
                  Url = new Uri("http://rcolzani.com")
                }
              });
          }

          var xmlCommentsFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
          var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

          options.IncludeXmlComments(xmlCommentsFullPath);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                  IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseSwagger().UseSwaggerUI(options =>
      {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
          options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
        options.RoutePrefix = "";
      });
      //app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
