using Net.Pkcs11Interop.HighLevelAPI;
using Serilog.Debugging;
using Serilog;
using System.Reflection;
using System.Runtime.InteropServices;
using Pkcs11Test.CustomClasses;
using Serilog.Events;

namespace Pkcs11Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Logger.
            SelfLog.Enable(p => File.AppendAllText("c:\\logs\\datapagemarker\\sg.txt", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " - " + p));
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .WriteTo.Logger(p => p
                    .WriteTo.File(
                        @"C:\src\_Temp\Pkcs11Test\Logs\Log.txt",
                        LogEventLevel.Warning,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Properties} {Message:lj}{NewLine}{Exception}",
                        flushToDiskInterval: TimeSpan.FromSeconds(1),
                        shared: true
                    )
                )
                .CreateLogger();
            Log.Warning("APP START");

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var adapter = new HsmAdapter();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // https://github.com/Pkcs11Interop/Pkcs11Interop/issues/239
                // https://github.com/Pkcs11Interop/Pkcs11Interop/issues/168#issuecomment-729985741
                // https://github.com/Pkcs11Interop/pkcs11-logger/blob/master/test/Pkcs11LoggerTests/Settings.cs#L108-L113
                // https://github.com/Pkcs11Interop/Pkcs11Interop/pull/208
                NativeLibrary.SetDllImportResolver(typeof(Pkcs11InteropFactories).Assembly, CustomDllImportResolver);
                adapter.Initialize(@"/pkcs11/libcs_pkcs11_R2.so", 0, "12345678");
            }
            else
            {
                adapter.Initialize(@"C:\src\_Temp\Pkcs11Test\Files\cs_pkcs11_R2.dll", 0, "12345678");
            }
            builder.Services.AddSingleton<IHsmAdapter>(adapter);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        static IntPtr CustomDllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? dllImportSearchPath)
        {
            string mappedLibraryName = (libraryName == "libdl") ? "libdl.so.2" : libraryName;
            // return NativeLibrary.Load(mappedLibraryName, assembly, dllImportSearchPath);
            return NativeLibrary.Load(mappedLibraryName);
        }
    }
}
