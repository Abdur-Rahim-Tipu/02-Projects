﻿using System;
using System.Linq;
using System.Collections.Generic;
using Exam03Project_Method.Models;
using Exam03Project_Method.Repoes;
namespace Exam03Project_Method
{
    public static class Program
    {
        public static void Main()
        {
            RepoCreator rc = new RepoCreator();
            Console.WriteLine("Insert operation");
            var repoP = rc.GetRepo<PromotionProduct>();
            PromotionProduct p1 = new PromotionProduct(1, "250ply Tissue Paper", 27.50M);
            PromotionProduct p2 = new PromotionProduct(2, "8pcs Marker Pen", 153.00M);
            PromotionProduct p3 = new PromotionProduct(3, "Wooden Organizer", 110.00M);
            repoP.Add(p1);
            repoP.Add(p2);
            repoP.Add(p3);
            Console.WriteLine("Read operation");
            repoP.Get()
            .ToList()
            .ForEach(p => Console.WriteLine($"Id: {p.Id} Name: {p.Name} Unit Price: {p.UnitPrice}"));
            Console.WriteLine("Update operation");
            p2.Name = "4pcs Marker pen";
            p2.UnitPrice = 125.00M;
            repoP.Update(p2);
            repoP.Get()
            .ToList()
            .ForEach(p => Console.WriteLine($"Id: {p.Id} Name: {p.Name} Unit Price: {p.UnitPrice}"));
            Console.WriteLine("Delete operation");
            repoP.Delete(2);
            repoP.Get()
            .ToList()
            .ForEach(p => Console.WriteLine($"Id: {p.Id} Name: {p.Name} Unit Price: {p.UnitPrice}"));
            Console.WriteLine();
            var s1 = new Sales(1, 2022, 1, 1, 34000);
            var s2 = new Sales(2, 2022, 2, 2, 37000);
            var s3 = new Sales(1, 2022, 1, 1, 34000);
            var repoS = rc.GetRepo<Sales>();
            repoS.Add(s1);
            repoS.Add(s2);
            repoS.Add(s3);
            Console.WriteLine("Read operation");
            repoS.Get()
            .ToList()
            .ForEach(s => Console.WriteLine($"Id: {s.Id} Month: {s.Month} Product Id: {s.ProductId} Sold: {s.QuatitySold}"));
            Console.WriteLine("Update operation");
            s2.QuatitySold = 22000;
            repoS.Update(s2);
            repoS.Get()
            .ToList()
            .ForEach(s => Console.WriteLine($"Id: {s.Id} Month: {s.Month} Product Id: {s.ProductId} Sold: {s.QuatitySold}"));
            Console.WriteLine("Delete operation");
            repoS.Delete(2);
            repoS.Get()
            .ToList()
            .ForEach(s => Console.WriteLine($"Id: {s.Id} Month: {s.Month} Product Id: {s.ProductId} Sold: {s.QuatitySold}"));
            Console.ReadKey();
        }
    }
}
#region All Class
namespace Exam03Project_Method.Models
{
    public interface IBaseEntity
    {
        int Id { get; set; }
    }
    public class PromotionProduct : IBaseEntity
    {
        public PromotionProduct() { }
        public PromotionProduct(int id, string name, decimal unitPrice)
        {
            this.Id = id;
            this.Name = name;
            this.UnitPrice = unitPrice;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

    }
    public class Sales : IBaseEntity
    {
        public Sales() { }
        public Sales(int id, int year, int month, int productId, int quantitySold)
        {
            this.Id = id;
            this.Year = year;
            this.Month = month;
            this.ProductId = productId;
            this.QuatitySold = quantitySold;
        }
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int ProductId { get; set; }
        public int QuatitySold { get; set; }

    }

}
#endregion
#region Repositories
namespace Exam03Project_Method.Repoes
{
    public interface IGenericRepo<T> where T : class, IBaseEntity, new()
    {
        IList<T> Get();
        T Get(int id);
        void Add(T model);
        void Update(T model);
        void Delete(int id);
    }
    public class GenericRepo<T> : IGenericRepo<T> where T : class, IBaseEntity, new()
    {
        IList<T> dataCollection;
        public GenericRepo(IList<T> dataCollection)
        {
            this.dataCollection = dataCollection;
        }
        public IList<T> Get()
        {
            return this.dataCollection;
        }

        public T Get(int id)
        {
            return this.dataCollection.FirstOrDefault(x => x.Id == id);
        }

        public void Add(T model)
        {
            this.dataCollection.Add(model);
        }
        public void Update(T model)
        {
            var existing = this.dataCollection.FirstOrDefault(x => x.Id == model.Id);
            if (existing != null)
            {
                int i = this.dataCollection.IndexOf(existing);
                this.dataCollection.RemoveAt(i);
                this.dataCollection.Insert(i, model);
            }
        }
        public void Delete(int id)
        {
            var existing = this.dataCollection.FirstOrDefault(x => x.Id == id);

            if (existing != null)

            {
                int i = this.dataCollection.IndexOf(existing);
                this.dataCollection.RemoveAt(i);
            }
        }
    }
    #endregion
#region Repositorie Manager
    public interface IRepoCreator
    {
        IGenericRepo<T> GetRepo<T>() where T : class, IBaseEntity, new();
    }
    public class RepoCreator : IRepoCreator
    {
        public IGenericRepo<T> GetRepo<T>() where T : class, IBaseEntity, new()
        {
            return new GenericRepo<T>(new List<T>());
        }

    }

}
#endregion