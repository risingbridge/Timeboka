namespace TimebokaUI;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddSingleton<IDbConnection, DbConnection>();
        builder.Services.AddSingleton<IHourEntry, MongoHourEntry>();
    }
}