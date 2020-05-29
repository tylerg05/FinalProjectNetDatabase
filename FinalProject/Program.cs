using NLog;
using NorthwindConsole.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            var db = new NorthwindContext();
            logger.Info("Started");
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Add Products records");
                    Console.WriteLine("2) Edit a Products record");
                    Console.WriteLine("3) Display Products records");
                    Console.WriteLine("4) Add Categories records");
                    Console.WriteLine("5) Edit a Categories record");
                    Console.WriteLine("6) Display Categories records");
                    Console.WriteLine("7) Delete a Products record");
                    Console.WriteLine("8) Delete a Categories record");
                    Console.WriteLine("9) Display a random product");
                    Console.WriteLine("0) Display a random category and its products");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    // add products record
                    if (choice == "1")
                    {
                        Product product = new Product();
                        Console.WriteLine("Enter Product Name:");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter the quantity per unit (string): ");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter the unit price (decimal): ");
                        product.UnitPrice = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter units in stock (int): ");
                        product.UnitsInStock = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("Enter units on order (int): ");
                        product.UnitsOnOrder = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("Enter reorder level (int): ");
                        product.ReorderLevel = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("Enter discontinued (bool): ");
                        product.Discontinued = Convert.ToBoolean(Console.ReadLine());
                        Console.WriteLine("Enter category ID (int): ");
                        product.CategoryId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter supplier ID (int): ");
                        product.SupplierId = Convert.ToInt32(Console.ReadLine());

                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if (isValid)
                        {

                            // check for unique name
                            if (db.Products.Any(p => p.ProductName == product.ProductName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            // add to db
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddProduct(product);
                                logger.Info("Product added - {name}", product.ProductName);
                            }
                        }
                    }
                    // edit a products record
                    else if (choice == "2")
                    {
                        var query = db.Products.OrderBy(b => b.ProductID);

                        var counter = 1;
                        foreach (var item in query)
                        {
                            Console.WriteLine(counter + ") " + item.ProductName);
                            counter++;
                        }
                        Console.WriteLine("Select the product number to edit: ");
                        var productSelection = Convert.ToInt32(Console.ReadLine());
                        var selectedProduct = db.Products.Find(productSelection);
                        Product newProduct = new Product();
                        Console.WriteLine("Enter new Product Name:");
                        newProduct.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter the new quantity per unit (string): ");
                        newProduct.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter the new unit price (decimal): ");
                        newProduct.UnitPrice = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter new units in stock (int): ");
                        newProduct.UnitsInStock = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("Enter new units on order (int): ");
                        newProduct.UnitsOnOrder = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("Enter new reorder level (int): ");
                        newProduct.ReorderLevel = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("Enter new discontinued (bool): ");
                        newProduct.Discontinued = Convert.ToBoolean(Console.ReadLine());
                        Console.WriteLine("Enter new category ID (int): ");
                        newProduct.CategoryId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter new supplier ID (int): ");
                        newProduct.SupplierId = Convert.ToInt32(Console.ReadLine());
                        db.EditProduct(selectedProduct, newProduct);
                    }
                    // display products records
                    else if (choice == "3")
                    {
                        string productsDisplayChoice;
                        Console.WriteLine("1) See all products");
                        Console.WriteLine("2) See discontinued products");
                        Console.WriteLine("3) See active products");
                        Console.WriteLine("4) Display a specific product");
                        productsDisplayChoice = Console.ReadLine();
                        Console.Clear();
                        logger.Info($"Option {productsDisplayChoice} selected");
                        // all products
                        if (productsDisplayChoice == "1")
                        {
                            var query = db.Products.OrderBy(p => p.ProductID);

                            Console.WriteLine($"{query.Count()} records returned");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.ProductName} - (Discontinued = {item.Discontinued})");
                            }
                            logger.Info("Product(s) displayed.");
                        }
                        // discontinued products
                        else if (productsDisplayChoice == "2")
                        {
                            var query = db.Products.OrderBy(p => p.ProductID);

                            foreach (var item in query)
                            {
                                if (item.Discontinued == true)
                                {
                                    Console.WriteLine($"{item.ProductName} - (Discontinued = {item.Discontinued})");
                                }
                            }
                            logger.Info("Product(s) displayed.");
                        }
                        // active products
                        else if (productsDisplayChoice == "3")
                        {
                            var query = db.Products.OrderBy(p => p.ProductID);

                            foreach (var item in query)
                            {
                                if (item.Discontinued == false)
                                {
                                    Console.WriteLine($"{item.ProductName} - (Discontinued = {item.Discontinued})");
                                }
                            }
                            logger.Info("Product(s) displayed.");
                        }
                        // specific product
                        else if (productsDisplayChoice == "4")
                        {
                            var query = db.Products.OrderBy(p => p.ProductID);

                            var counter = 1;
                            foreach (var item in query)
                            {
                                Console.WriteLine(counter + ") " + item.ProductName);
                                counter++;
                            }
                            Console.WriteLine("Select the product number to view: ");
                            var productSelection = Convert.ToInt32(Console.ReadLine());
                            var selectedProduct = db.Products.Find(productSelection);
                            Console.WriteLine($"{selectedProduct.ProductName}");
                            Console.WriteLine($"Product ID: {selectedProduct.ProductID}");
                            Console.WriteLine($"Quantity per Unit: {selectedProduct.QuantityPerUnit}");
                            Console.WriteLine($"Unit Price: {selectedProduct.UnitPrice}");
                            Console.WriteLine($"Units in Stock: {selectedProduct.UnitsInStock}");
                            Console.WriteLine($"Units on Order: {selectedProduct.UnitsOnOrder}");
                            Console.WriteLine($"Reorder Level: {selectedProduct.ReorderLevel}");
                            Console.WriteLine($"Discontinued: {selectedProduct.Discontinued}");
                            logger.Info("Product(s) displayed.");
                        }
                    }
                    // add categories record
                    else if (choice == "4")
                    {
                        Category category = new Category();
                        Console.WriteLine("Enter Category Name:");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        category.Description = Console.ReadLine();

                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {

                            // check for unique name
                            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            // add to db
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddCategory(category);
                                logger.Info("Category added - {name}", category.CategoryName);
                            }
                        }
                    }
                    // edit categories record
                    else if (choice == "5")
                    {
                        var query = db.Categories.OrderBy(c => c.CategoryId);

                        var counter = 1;
                        foreach (var item in query)
                        {
                            Console.WriteLine(counter + ". " + item.CategoryName);
                            counter++;
                        }
                        Console.WriteLine("Select the category number to edit: ");
                        var categorySelection = Convert.ToInt32(Console.ReadLine());
                        var selectedCategory = db.Categories.Find(categorySelection);
                        Category newCategory = new Category();
                        Console.WriteLine("Enter new Category Name:");
                        newCategory.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the new Category Description:");
                        newCategory.Description = Console.ReadLine();
                        db.EditCategory(selectedCategory, newCategory);
                    }
                    // display categories records
                    else if (choice == "6")
                    {
                        string categoryDisplayChoice;
                        Console.WriteLine("1) See all categories");
                        Console.WriteLine("2) See all categories and related active products");
                        Console.WriteLine("3) See specific category and related active products");
                        categoryDisplayChoice = Console.ReadLine();
                        Console.Clear();
                        logger.Info($"Option {categoryDisplayChoice} selected");
                        // all categories
                        if (categoryDisplayChoice == "1")
                        {
                            var query = db.Categories.OrderBy(c => c.CategoryId);

                            Console.WriteLine($"{query.Count()} records returned");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryName} - {item.Description}");
                            }
                        }
                        // all categories and products
                        else if (categoryDisplayChoice == "2")
                        {
                            var query = db.Categories.OrderBy(c => c.CategoryId);
                            var products = db.Products.OrderBy(p => p.CategoryId);

                            foreach (var item in query)
                            {
                                Console.WriteLine("------------");
                                Console.WriteLine(item.CategoryName);
                                Console.WriteLine("------------");
                                var selectedCategory = item;
                                db.productsInCategory(selectedCategory);
                            }
                        }
                        // specific category
                        else if (categoryDisplayChoice == "3")
                        {
                            var query = db.Categories.OrderBy(c => c.CategoryId);
                            var products = db.Products.OrderBy(p => p.ProductName);

                            var counter = 1;
                            foreach (var item in query)
                            {
                                Console.WriteLine(counter + ") " + item.CategoryName);
                                counter++;
                            }
                            Console.WriteLine("Select the category number to view: ");
                            var categorySelection = Convert.ToInt32(Console.ReadLine());
                            var selectedCategory = db.Categories.Find(categorySelection);
                            Console.WriteLine($"{selectedCategory.CategoryName}");
                            foreach (var item in products)
                            {
                                if (selectedCategory.CategoryId == item.CategoryId &&
                                    item.Discontinued == false)
                                {
                                    Console.WriteLine($"{item.ProductName}");
                                }
                            }
                            logger.Info("Product(s) displayed.");
                        }
                    }
                    // delete products record
                    else if (choice == "7")
                    {
                        var query = db.Products.OrderBy(p => p.ProductName);

                        var counter = 1;
                        foreach (var item in query)
                        {
                            Console.WriteLine(counter + ") " + item.ProductName);
                            counter++;
                        }
                        Console.WriteLine("Select the product number to delete: ");
                        var productSelection = Convert.ToInt32(Console.ReadLine());
                        var selectedProduct = db.Products.Find(productSelection);
                        db.DeleteProduct(selectedProduct);
                        logger.Info("Product deleted");
                    }
                    // delete categories record
                    else if (choice == "8")
                    {
                        var query = db.Categories.OrderBy(c => c.CategoryId);

                        var counter = 1;
                        foreach (var item in query)
                        {
                            Console.WriteLine(counter + ") " + item.CategoryName);
                            counter++;
                        }
                        Console.WriteLine("Select the category number to delete: ");
                        var categorySelection = Convert.ToInt32(Console.ReadLine());
                        var selectedCategory = db.Categories.Find(categorySelection);
                        db.DeleteCategory(selectedCategory);
                        logger.Info("Category deleted");
                    }
                    // view random product
                    else if (choice == "9")
                    {
                        var query = db.Products.OrderBy(p => p.ProductName);
                        int lowestProductId = 1;
                        int highestProductId = 0;

                        foreach (var item in query)
                        {
                            highestProductId = item.ProductID;
                        }

                        var rand = new Random();
                        int generatedProduct = rand.Next(lowestProductId, highestProductId);

                        var selectedProduct = db.Products.Find(generatedProduct);

                        Console.WriteLine($"{selectedProduct.ProductName}");
                        Console.WriteLine($"Product ID: {selectedProduct.ProductID}");
                        Console.WriteLine($"Quantity per Unit: {selectedProduct.QuantityPerUnit}");
                        Console.WriteLine($"Unit Price: {selectedProduct.UnitPrice}");
                        Console.WriteLine($"Units in Stock: {selectedProduct.UnitsInStock}");
                        Console.WriteLine($"Units on Order: {selectedProduct.UnitsOnOrder}");
                        Console.WriteLine($"Reorder Level: {selectedProduct.ReorderLevel}");
                        Console.WriteLine($"Discontinued: {selectedProduct.Discontinued}");
                    }
                    // view random category
                    else if (choice == "0")
                    {
                        var query = db.Categories.OrderBy(c => c.CategoryId);
                        var products = db.Products.OrderBy(p => p.ProductName);
                        int lowestCategoryId = 1;
                        int highestCategoryId = 0;

                        foreach (var item in query)
                        {
                            highestCategoryId = item.CategoryId;
                        }

                        var rand = new Random();
                        int generatedCategory = rand.Next(lowestCategoryId, highestCategoryId);

                        var selectedCategory = db.Categories.Find(generatedCategory);

                        Console.WriteLine("------------");
                        Console.WriteLine(selectedCategory.CategoryName);
                        Console.WriteLine("------------");

                        foreach (var item in products)
                        {
                            if (selectedCategory.CategoryId == item.CategoryId)
                            {
                                Console.WriteLine($"{item.ProductName}");
                            }
                        }
                    }
                    // invalid option
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                    }
                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Ended");
        }
    }
}
