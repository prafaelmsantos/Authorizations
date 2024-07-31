namespace Authorizations.API
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
            //DB Connection
            var defaultConnection = Environment.GetEnvironmentVariable("CONNECTION_STRINGS") ?? Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(context => context.UseNpgsql(defaultConnection));

            services.AddGraphQLServices();
            services.AddPersistenceServices();
            services.AddNotificationsGrpcService();

            //JWT
            //Cada vez que criptografamos com uma chave, tambem temos de discriptografar com a mesma
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Indica que estou a trabalhar com a arquitetura MVC com Views Controllers. Permite chamar o meu controller
            //NewsoftJson para evitar um loop infinito no retorno
            //AddJsonOptions para os Enums, onde para cada item do meu Enum, retorna um Id
            services.AddControllers()
                    .AddJsonOptions(x =>
                        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                    .AddNewtonsoftJson(x =>
                        x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Authorizations.API",
                    Description = "An ASP.NET Core Web API for Authorizations",
                    Contact = new OpenApiContact()
                    {
                        Name = "Auto Moreira Portugal",
                        Email = "automoreiraportugal@gmail.com"
                    },
                    Version = "1.0"
                });

                //Rafael - JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header utilizando o Bearer.
                                Entre com 'Bearer ' [espaço] então coloque o seu token.
                                Exemplo: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"

                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("X-Version"));
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            //CORS - Dado qualquer header da requisição por http vinda de qualquer metodo (get, post, delete..) e vindos de qualquer origem
            services.AddCors(o => o.AddPolicy("CustomPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Token-Expired"); ;
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //CORS
            app.UseCors("CustomPolicy");

            //GraphQL
            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/graphql",
                Path = "/playground"
            });

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorizations.API v1"));

            //HTTPS
            app.UseHttpsRedirection();

            //Indica que vou trabalhar por rotas.
            app.UseRouting();
            app.UseWebSockets();

            //Auth- 1º autentica e depois autoriza
            app.UseAuthentication();
            app.UseAuthorization();

            //E vou retornar determinados endpoints de acordo com a configuração destas rotas dentro do meu controller
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("There is http and gRPC communication endpoints.");
                });
            });
        }
    }
}
