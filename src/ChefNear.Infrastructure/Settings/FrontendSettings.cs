using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Infrastructure.Settings;

public class FrontendSettings
{
    public string BaseUrl { get; init; } = default!;

    public FrontendRoutes Routes { get; init; } = new();
}

public class FrontendRoutes
{
    public string PaymentResultUrl { get; init; } = default!;
}
