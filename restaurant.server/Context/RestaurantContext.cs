using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using restaurant.server.Models;

namespace restaurant.server.Context;

public partial class RestaurantContext : DbContext
{
    public RestaurantContext(DbContextOptions<RestaurantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishesInOrder> DishesInOrders { get; set; }

    public virtual DbSet<MeasureUnit> MeasureUnits { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsInDish> ProductsInDishes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Supply> Supplies { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.IdDish).HasName("pk_dish_id");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.Dishes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_measure_unit_id");
        });

        modelBuilder.Entity<DishesInOrder>(entity =>
        {
            entity.HasKey(e => new { e.IdOrder, e.IdDish }).HasName("pk_dishes_in_order");

            entity.HasOne(d => d.IdDishNavigation).WithMany(p => p.DishesInOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dish_id");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.DishesInOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_id");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.DishesInOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_status_id");
        });

        modelBuilder.Entity<MeasureUnit>(entity =>
        {
            entity.HasKey(e => e.IdUnit).HasName("pk_measure_unit_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("pk_order_id");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_id");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_status_id");

            entity.HasOne(d => d.IdTableNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_table_id");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.IdPosition).HasName("pk_position_id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("pk_product_id");
        });

        modelBuilder.Entity<ProductsInDish>(entity =>
        {
            entity.HasKey(e => new { e.IdDish, e.IdProduct }).HasName("pk_products_in_dish");

            entity.HasOne(d => d.IdDishNavigation).WithMany(p => p.ProductsInDishes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dish_id");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductsInDishes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_id");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.ProductsInDishes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_measure_unit_id");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("pk_employee_id");

            entity.HasOne(d => d.IdPositionNavigation).WithMany(p => p.Staff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_position_id");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("pk_status_id");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("pk_supplier_id");
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.HasKey(e => new { e.IdSupplier, e.IdProduct, e.Date }).HasName("pk_supply");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Supplies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_id");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.Supplies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_supplier_id");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.Supplies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_measure_unit_id");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.IdTable).HasName("pk_table_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
