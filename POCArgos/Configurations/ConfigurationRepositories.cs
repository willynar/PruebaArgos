using POCArgos.Interfaces;
using POCArgos.Services;
using POCArgos.Data.Repositories;

namespace POCArgos.Configurations
{
    public static class ConfigurationRepositories
    {
        #region ConfigureRepositories

        /// <summary>
        /// Configuración de servicios y repositorios para la Inversión de Dependencias.
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Repositorios (Capa de Acceso a Datos)
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICatalogRepository, CatalogRepository>();

            // Servicios (Capa de Lógica de Negocio)
            services.AddScoped<ICatalogsService, Catalogs>();
            services.AddScoped<IOrderService, Orders>();

            return services;
        }

        #endregion
    }
}
