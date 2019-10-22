using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    //هر کلاسی که از این ارثبری کرد، به ازای اون یک جدول در دیتابیس ساخته میشه
    //اگه خواستیم کلاسی داشته باشیم که به ازای اون جدول ساخته بشه ولی از BaseEntity
    //ارثبری نکنیم بخاطر اینکه آی دی جدولش به نام آی دی نباشه
    //از IEntity ارثبری میکنیم
    //حالا اگه از این اینترفیس ارثبری بشه یعنی به ازای اون، جدول تشکیل میشه
    //و اگه از پایینی ارثبری بشه علاوه بر تشکیل جدول کلید اصلیش هم تشکلی میشه
    public interface IEntity
    {

    }
    //کلاس ابسترکت تعریف شد چون درخواست نیو کردن نمیخواهیم داشته باشیم
    //همینطور اگه ابسترکت نباشه به ازای این کلاس و متد ها جدول ساخته میشود
    public abstract class BaseEntity<TKey>:IEntity
    {
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {

    }
}
