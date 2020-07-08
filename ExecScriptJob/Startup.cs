using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ExecScriptJob.JWT;
using ExecScriptJob.Model.FreeSqlHelper;
using ExecScriptJob.SwaggerModel;
using FreeSql;
using JWTToken.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VueCliMiddleware;

namespace ExecScriptJob
{
    /// <summary>
    /// 主机启动后配置类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            string SqlLiteConn = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                SqlLiteConn = Configuration.GetConnectionString("SqlLiteLinux");
            }
            else
            {
                SqlLiteConn = Configuration.GetConnectionString("SqlLiteWin");
            }
            var fsql1 = new FreeSqlBuilder().UseConnectionString(DataType.Sqlite, SqlLiteConn)
            .Build<SqlLiteFlag>();
            services.AddSingleton(fsql1);

            services.AddTransient<ITokenHelper, TokenHelper>();
            //读取配置文件配置的jwt相关配置
            services.Configure<JWTConfig>(Configuration.GetSection("JWTConfig"));
            //启用JWT
            services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
            AddJwtBearer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "api文档",
                    Description = "脚本执行平台api"
                });
                // 为 Swagger 设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DocInclusionPredicate((docName, description) => true);
                //添加对控制器的标签(描述)
                c.DocumentFilter<ApplyTagDescriptions>();//显示类名
                c.CustomSchemaIds(type => type.FullName);// 可以解决相同类名会报错的问题
                c.OperationFilter<AuthTokenHeaderParameter>();
            });
            services.AddScoped<TokenFilter>();




            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "脚本执行平台api");
                c.RoutePrefix = "doc";//设置根节点访问
                //c.DocExpansion(DocExpansion.None);//折叠
                c.DefaultModelsExpandDepth(-1);//不显示Schemas
            });


            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "ClientApp";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve", port: 20001);
                }

            });
        }
    }
}
