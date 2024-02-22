using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Outbox.Application
{
    public static class DependencyInjection
    {
        public static void ApplicationServicesInjection(this IServiceCollection services)
        {
            services.AddMediatR(AssemblyReference.Assembly);
        }
    }
}
