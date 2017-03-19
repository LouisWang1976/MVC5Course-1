using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{
        public Product Find(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public override IQueryable<Product> All()
        {
            return base.All().Where(p => false == p.IsDeleted && p.Stock > 500);
        }

        public IQueryable<Product> All(bool showAll)
        {
            if (showAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }

        public override void Delete(Product entity)
        {
            //this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;
            //entity.IsDeleted = true;
            base.Delete(entity);
        }

    }

    public  interface IProductRepository : IRepository<Product>
	{

	}
}