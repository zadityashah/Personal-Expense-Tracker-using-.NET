using PersonalExpenseTracker.DataAccess.Services;
using PersonalExpenseTracker.DataAccess.Services.Interface;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;


namespace PersonalExpenseTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddMudServices();

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<ITransactionService, TransactionService>();

            builder.Services.AddScoped<ITagService, TagService>();



#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}