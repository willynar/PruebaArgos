using POCArgos.Interfaces;
using POCArgos.Logic;

namespace backend_vex.Configuration
{
    /// <summary>
    /// configuraciones necesarias para inyeccion de dependencias
    /// </summary>
    public static class ConfigurationRepositories
    {
        #region ConfigureRepositories

        /// <summary>
        /// repositorios EVT
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICatalogsService, Catalogs>();
            services.AddScoped<IOrderService, Orders>();

            return services;
        }

        #endregion
    }
}
