using Net.Pkcs11Interop.HighLevelAPI;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Pkcs11Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            // https://github.com/Pkcs11Interop/Pkcs11Interop/issues/239
            // https://github.com/Pkcs11Interop/Pkcs11Interop/issues/168#issuecomment-729985741
            // https://github.com/Pkcs11Interop/pkcs11-logger/blob/master/test/Pkcs11LoggerTests/Settings.cs#L108-L113
            // https://github.com/Pkcs11Interop/Pkcs11Interop/pull/208
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                NativeLibrary.SetDllImportResolver(typeof(Pkcs11InteropFactories).Assembly, CustomDllImportResolver);
            }

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
