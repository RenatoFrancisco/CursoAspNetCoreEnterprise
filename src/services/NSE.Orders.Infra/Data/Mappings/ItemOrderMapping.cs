namespace NSE.Orders.Infra.Data.Mappings;

public class ItemOrderMapping : IEntityTypeConfiguration<ItemOrder>
{
    public void Configure(EntityTypeBuilder<ItemOrder> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ProductName)
            .IsRequired()
            .HasColumnType("varchar(250)");

        // 1 : N => Order : ItemsOrder
        builder.HasOne(c => c.Order)
            .WithMany(c => c.ItemsOrder);

        builder.ToTable("ItemsOrder");
    }
}
