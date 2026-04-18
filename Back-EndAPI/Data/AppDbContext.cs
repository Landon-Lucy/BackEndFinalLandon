using System;
using System.Collections.Generic;
using Back_EndAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aisle> Aisles { get; set; }

    public virtual DbSet<AisleBay> AisleBays { get; set; }

    public virtual DbSet<AisleShelf> AisleShelves { get; set; }

    public virtual DbSet<Bay> Bays { get; set; }

    public virtual DbSet<Bin> Bins { get; set; }

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
    public virtual DbSet<CustomerOrderStatus> CustomerOrderStatuses { get; set; }

    public virtual DbSet<Discrepancy> Discrepancies { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<OrderedItem> OrderedItems { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<ReceivedHistory> ReceivedHistories { get; set; }

    public virtual DbSet<ReceivedItem> ReceivedItems { get; set; }

    public virtual DbSet<ReceivedShipment> ReceivedShipments { get; set; }

    public virtual DbSet<Shelf> Shelves { get; set; }

    public virtual DbSet<ShippedItem> ShippedItems { get; set; }

    public virtual DbSet<SoldItem> SoldItems { get; set; }

    public virtual DbSet<TransferRecord> TransferRecords { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aisle>(entity =>
        {
            entity.HasKey(e => e.AisleNumber).HasName("aisle_pkey");

            entity.Property(e => e.AisleNumber).ValueGeneratedNever();
        });

        modelBuilder.Entity<AisleBay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aisle_bay_pkey");

            entity.HasOne(d => d.AisleNumberNavigation).WithMany(p => p.AisleBays).HasConstraintName("aisle_bay_aisle_number_fkey");

            entity.HasOne(d => d.BayNumberNavigation).WithMany(p => p.AisleBays).HasConstraintName("aisle_bay_bay_number_fkey");
        });

        modelBuilder.Entity<AisleShelf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aisle_shelf_pkey");

            entity.HasOne(d => d.AisleNumberNavigation).WithMany(p => p.AisleShelves).HasConstraintName("aisle_shelf_aisle_number_fkey");

            entity.HasOne(d => d.ShelfLetterNavigation).WithMany(p => p.AisleShelves).HasConstraintName("aisle_shelf_shelf_letter_fkey");
        });

        modelBuilder.Entity<Bay>(entity =>
        {
            entity.HasKey(e => e.BayNumber).HasName("bay_pkey");

            entity.Property(e => e.BayNumber).ValueGeneratedNever();
        });

        modelBuilder.Entity<Bin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("storagelocation_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Team2Part2\".storagelocation_id_seq'::regclass)");

            entity.HasOne(d => d.AisleBay).WithMany(p => p.Bins).HasConstraintName("storagelocation_bay_numberid_fkey");

            entity.HasOne(d => d.AisleShelf).WithMany(p => p.Bins).HasConstraintName("storagelocation_shelf_numberid_fkey");

            entity.HasOne(d => d.SkuNumberNavigation).WithMany(p => p.Bins).HasConstraintName("fk_sku_number");
        });

        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.Tracking).HasName("box_pkey");

            entity.Property(e => e.Tracking).ValueGeneratedNever();

            entity.HasOne(d => d.CustomerOrder).WithMany(p => p.Boxes).HasConstraintName("box_customer_order_id_fkey");
        });

        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("carrier_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<CustomerOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_order_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.DateTimeOrdered).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Carrier).WithMany(p => p.CustomerOrders).HasConstraintName("customer_order_carrier_id_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerOrders).HasConstraintName("customer_order_customer_id_fkey");
        });

        modelBuilder.Entity<Discrepancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("discrepancy_pkey");

            entity.HasOne(d => d.OrderedItem).WithMany(p => p.Discrepancies).HasConstraintName("discrepancy_ordered_item_id_fkey");

            entity.HasOne(d => d.ReceivedItem).WithMany(p => p.Discrepancies).HasConstraintName("discrepancy_received_item_id_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.SkuNumber).HasName("item_pkey");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("login_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.Logins).HasConstraintName("login_customer_id_fkey");
        });

        modelBuilder.Entity<OrderedItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ordered_item_pkey");

            entity.HasOne(d => d.Purchase).WithMany(p => p.OrderedItems).HasConstraintName("ordered_item_purchase_id_fkey");

            entity.HasOne(d => d.SkuNumberNavigation).WithMany(p => p.OrderedItems).HasConstraintName("ordered_item_sku_number_fkey");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_order_pkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.PurchaseOrders).HasConstraintName("purchase_order_vendorid_fkey");
        });

        modelBuilder.Entity<ReceivedHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("received_history_pkey");

            entity.HasOne(d => d.ReceivedItem).WithMany(p => p.ReceivedHistories).HasConstraintName("received_history_received_item_id_fkey");
        });

        modelBuilder.Entity<ReceivedItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("received_item_pkey");

            entity.HasOne(d => d.Shipment).WithMany(p => p.ReceivedItems).HasConstraintName("received_item_shipment_id_fkey");

            entity.HasOne(d => d.SkuNumberNavigation).WithMany(p => p.ReceivedItems).HasConstraintName("received_item_sku_number_fkey");
        });

        modelBuilder.Entity<ReceivedShipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("received_shipment_pkey");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.ReceivedShipments).HasConstraintName("received_shipment_purchase_order_id_fkey");
        });

        modelBuilder.Entity<Shelf>(entity =>
        {
            entity.HasKey(e => e.ShelfLetter).HasName("shelf_pkey");

            entity.Property(e => e.ShelfLetter).ValueGeneratedNever();
        });

        modelBuilder.Entity<ShippedItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shippeditem_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Qty).HasDefaultValue(1);

            entity.HasOne(d => d.BoxTrackingNavigation).WithMany(p => p.ShippedItems).HasConstraintName("shippeditem_box_tracking_fkey");

            entity.HasOne(d => d.SkuNumberNavigation).WithMany(p => p.ShippedItems).HasConstraintName("shippeditem_sku_number_fkey");
        });

        modelBuilder.Entity<SoldItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("solditem_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Qty).HasDefaultValue(1);

            entity.HasOne(d => d.CustomerOrder).WithMany(p => p.SoldItems).HasConstraintName("solditem_customer_order_id_fkey");

            entity.HasOne(d => d.SkuNumberNavigation).WithMany(p => p.SoldItems).HasConstraintName("solditem_sku_number_fkey");
        });

        modelBuilder.Entity<TransferRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transferrecord_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Team2Part2\".transferrecord_id_seq'::regclass)");

            entity.HasOne(d => d.Receiveditem).WithMany(p => p.TransferRecords).HasConstraintName("transferrecord_receiveditemid_fkey");

            entity.HasOne(d => d.ShippedItem).WithMany(p => p.TransferRecords).HasConstraintName("fk_shipped_item");

            entity.HasOne(d => d.Storagelocation).WithMany(p => p.TransferRecords).HasConstraintName("transferrecord_storagelocationid_fkey");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vendor_pkey");
        });
        modelBuilder.HasSequence("shipped_item_id_seq", "Team2Part2");
        modelBuilder.HasSequence("transfer_record_id_seq", "Team2Part2");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
