namespace MultiTenancyDemo.Contracts;

public interface ITenantable
{
    string TenantId { get; set; }
}
