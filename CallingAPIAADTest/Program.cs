
namespace CallingAPIAADTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IDownstreamAPIClient, DownstreamAPIClient>();

            builder.Services.AddSingleton(x => new DownstreamAPIOptions 
            { 
                APIEndpointURL = Environment.GetEnvironmentVariable("DownstreamAPIURL"),
                Audience = Environment.GetEnvironmentVariable("DownstreamAPIAudience")
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
