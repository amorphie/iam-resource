using System;
using BBT.Prism.Application.Services;

namespace BBT.Resource.Resources;

public class ResourceAppService(IServiceProvider serviceProvider)
    : ApplicationService(serviceProvider), IResourceAppService;
