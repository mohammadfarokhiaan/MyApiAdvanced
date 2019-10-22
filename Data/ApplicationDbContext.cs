using Common.Utilities;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContext:DbContext
    {
        //برای روش دوم تنظیم دیتابیس باید این سازنده رو ایجاد کنیم
        public ApplicationDbContext(DbContextOptions options)
            :base(options)
        {

        }
        //خب ما باید کانکشن رو به دیتابیس معرفی کنیم پس به پکیج
        //Microsoft.aspnetCore.SqlServer
        //نیاز داریم
        //روش 1
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("");
        //    base.OnConfiguring(optionsBuilder);
        //}
        //روش 2 در استارتاپ پروژه اصلی
        //وظیفه این متد ساختن جدول برای مدل های ما هست
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //مسیر یا اسمبلی که مدل های ما داخل اون هست
            var entitiesAssembly = typeof(IEntity).Assembly;

            //به ازای این متد، که در لایه کاممان هست، تمام کلاس هایی که به عنوان مدل داریم 
            //به جدول داخل دیتابیس تبدیل میشن
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            //تمام فلونت ای پی آی هایی که در مسیر اسمبلی هستند رو اعمال میکنه
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            //این متد هم میاد رفتار پیشفرش یک جدول و کلید اصلیش رو تغییر میده
            //به این صورت که تا زمانی که تمام فرزندهاش حذف نشدن نذاریم خودش حذف بشه
            modelBuilder.AddRestrictDeleteBehaviorConvention();

            modelBuilder.AddSequentialGuidForIdConvention();
            //جمع بستن اسم جذول ها با استفاده از اسامی کلاس های مدل
            modelBuilder.AddPluralizingTableNameConvention();
        }

        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        //این متد قبل از ذخیره شدن اطلاعات در دیتابیس مقادیر رو بررسی میکنه و مثلا ی و ک عربی رو با فارسی جابجا میکنه
        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
    }
}
