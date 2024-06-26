﻿namespace MultiTenancyDemo.Settings;

public class Tenant
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ConnectionString { get; set; }
}
