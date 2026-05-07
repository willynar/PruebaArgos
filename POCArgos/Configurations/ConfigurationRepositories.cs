using POCArgos.Interfaces;
using POCArgos.Logic;

namespace POCArgos.Configurations
{
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
