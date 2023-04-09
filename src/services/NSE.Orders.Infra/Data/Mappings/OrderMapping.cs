namespace NSE.Orders.Infra.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(c => c.Id);

        builder.OwnsOne(p => p.Address, e =>
        {
            e.Property(pe => pe.Street)
                .HasColumnName("Street");

            e.Property(pe => pe.Number)
                .HasColumnName("Number");

            e.Property(pe => pe.Complement)
                .HasColumnName("Complement");

            e.Property(pe => pe.Neighborhood)
                .HasColumnName("Neighborhood");

            e.Property(pe => pe.ZipCode)
                .HasColumnName("ZipCode");

            e.Property(pe => pe.City)
                .HasColumnName("City");

            e.Property(pe => pe.State)
                .HasColumnName("State");
        });

        builder.Property(c => c.Code)
            .HasDefaultValueSql("NEXT VALUE FOR MySequence");

        // 1 : N => Order : ItemsOrder
        builder.HasMany(c => c.ItemsOrder)
            .WithOne(c => c.Order)
            .HasForeignKey(c => c.OrderId);

        builder.ToTable("Orders");
    }
}
