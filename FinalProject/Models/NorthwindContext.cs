using System.Data.Entity;

namespace NorthwindConsole.Models
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext() : base("name=NorthwindContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public void AddCategory(Category category)
        {
            this.Categories.Add(category);
            this.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            this.Products.Add(product);
            this.SaveChanges();
        }

        public void EditCategory(Category oldCategory, Category newCategory)
        {
            var old = this.Categories.Find(oldCategory);
            old.CategoryName = newCategory.CategoryName;
            old.Description = newCategory.Description;
            this.SaveChanges();
        }

        public void EditProduct(Product oldProduct, Product newProduct)
        {
            var old = this.Products.Find(oldProduct);
            old.ProductName = newProduct.ProductName;
            old.QuantityPerUnit = newProduct.QuantityPerUnit;
            old.UnitPrice = newProduct.UnitPrice;
            old.UnitsInStock = newProduct.UnitsInStock;
            old.UnitsOnOrder = newProduct.UnitsOnOrder;
            old.ReorderLevel = newProduct.ReorderLevel;
            old.Discontinued = newProduct.Discontinued;
            old.CategoryId = newProduct.CategoryId;
            old.SupplierId = newProduct.SupplierId;
            this.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            this.Categories.Remove(category);
            this.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            this.Products.Remove(product);
            this.SaveChanges();
        }

        public void productsInCategory(Category category)
        {
            foreach (var prod in Products)
            {
                if (prod.CategoryId == category.CategoryId && prod.Discontinued == false)
                {
                    System.Console.WriteLine(prod.ProductName);
                }
            }
        }
    }
}
