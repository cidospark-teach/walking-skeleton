using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WalkingSkeletonApi.Data;
using WalkingSkeletonApi.Data.EFCore;
using WalkingSkeletonApi.Data.Repositories.EFCoreRepositories;
using WalkingSkeletonApi.Helpers;
using WalkingSkeletonApi.Models;
using WalkingSkeletonApi.Services;

namespace WalkingSkeletonApi
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
            services.AddControllers();

            services.AddDbContextPool<WalkingSkeletonDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireLowercase = false;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = false;
                //options.Password.RequiredUniqueChars = 0;

                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<WalkingSkeletonDbContext>()
            .AddDefaultTokenProviders();

            services.AddTransient<SeederClass>();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddAutoMapper();
            services.AddCors();
            services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WLK-Skeleton", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {
                var param = new TokenValidationParameters();
                param.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]));
                param.ValidateIssuer = false;
                param.ValidateAudience = false;
                option.TokenValidationParameters = param;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeederClass seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //seeder.SeedMe().Wait();

            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "WalkingSkeleton-v1"));
        }
    }
}
