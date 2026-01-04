using Steeltoe.Discovery.Eureka;
using Steeltoe.Configuration.ConfigServer;
using Microsoft.EntityFrameworkCore;
using catalog_service.Data;
using catalog_service.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Config Server
builder.AddConfigServer();

// Add services to the container.
builder.Services.AddEurekaDiscoveryClient();

builder.Services.AddControllers();

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseInMemoryDatabase("CatalogDb"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
    context.Database.EnsureCreated();
    
    if (!context.Products.Any())
    {
        // Define Categories
        var categories = new Dictionary<string, Category>();
        var categoryNames = new[] { "Clothing", "Sports", "Kitchen", "Bathroom", "Bedroom", "Accessories" };
        
        foreach (var name in categoryNames)
        {
            var category = new Category { Name = name, Description = $"{name} Items" };
            context.Categories.Add(category);
            categories[name] = category;
        }
        context.SaveChanges(); // Save categories first to get IDs

        // Helper to get category (default to Clothing if not found or null)
        Category GetCategory(string? type) 
        {
            if (string.IsNullOrEmpty(type)) return categories["Clothing"];
            return categories.TryGetValue(type, out var cat) ? cat : categories["Clothing"];
        }

        context.Products.AddRange(
            new Product { Name = "Black and Gray Athletic Cotton Socks - 6 Pairs", Description = "Comfortable and breathable athletic socks perfect for sports and everyday wear.", Price = 10.90m, Image = "images/products/athletic-cotton-socks-6-pairs.jpg", RatingStars = 4.5, RatingCount = 87, Keywords = "socks,sports,apparel", Category = GetCategory("Sports"), Stock = 100 },
            new Product { Name = "Intermediate Size Basketball", Description = "Intermediate size basketball suitable for indoor and outdoor play.", Price = 20.95m, Image = "images/products/intermediate-composite-basketball.jpg", RatingStars = 4, RatingCount = 127, Keywords = "sports,basketballs", Category = GetCategory("Sports"), Stock = 50 },
            new Product { Name = "Adults Plain Cotton T-Shirt - 2 Pack", Description = "Comfortable and stylish cotton t-shirts for everyday wear.", Price = 7.99m, Image = "images/products/adults-plain-cotton-tshirt-2-pack-teal.jpg", RatingStars = 4.5, RatingCount = 56, Keywords = "tshirts,apparel,mens", Category = GetCategory("Clothing"), Stock = 200 },
            new Product { Name = "2 Slot Toaster - Black", Description = "Compact and efficient 2-slot toaster for quick breakfasts.", Price = 18.99m, Image = "images/products/black-2-slot-toaster.jpg", RatingStars = 5, RatingCount = 2197, Keywords = "toaster,kitchen,appliances", Category = GetCategory("Kitchen"), Stock = 25 },
            new Product { Name = "6 Piece White Dinner Plate Set", Description = "Elegant and durable dinner plate set for everyday use.", Price = 20.67m, Image = "images/products/6-piece-white-dinner-plate-set.jpg", RatingStars = 4, RatingCount = 37, Keywords = "plates,kitchen,dining", Category = GetCategory("Kitchen"), Stock = 30 },
            new Product { Name = "6-Piece Nonstick, Carbon Steel Oven Bakeware Baking Set", Description = "Versatile and durable nonstick bakeware set for all your baking needs.", Price = 34.99m, Image = "images/products/6-piece-non-stick-baking-set.webp", RatingStars = 4.5, RatingCount = 175, Keywords = "kitchen,cookware", Category = GetCategory("Kitchen"), Stock = 40 },
            new Product { Name = "Plain Hooded Fleece Sweatshirt", Description = "Cozy and warm hooded fleece sweatshirt for casual wear.", Price = 24.00m, Image = "images/products/plain-hooded-fleece-sweatshirt-yellow.jpg", RatingStars = 4.5, RatingCount = 317, Keywords = "hoodies,sweaters,apparel", Category = GetCategory("Clothing"), Stock = 150 },
            new Product { Name = "Luxury Towel Set - Graphite Gray", Description = "Soft and absorbent luxury towel set for a spa-like experience at home.", Price = 35.99m, Image = "images/products/luxury-tower-set-6-piece.jpg", RatingStars = 4.5, RatingCount = 144, Keywords = "bathroom,washroom,restroom,towels,bath towels", Category = GetCategory("Bathroom"), Stock = 60 },
            new Product { Name = "Liquid Laundry Detergent, 110 Loads, 82.5 Fl Oz", Description = "High-efficiency liquid laundry detergent for a deep clean.", Price = 28.99m, Image = "images/products/liquid-laundry-detergent-plain.jpg", RatingStars = 4.5, RatingCount = 305, Keywords = "bathroom,cleaning", Category = GetCategory("Bathroom"), Stock = 80 },
            new Product { Name = "Waterproof Knit Athletic Sneakers - Gray", Description = "Comfortable and breathable athletic sneakers perfect for sports and everyday wear.", Price = 33.90m, Image = "images/products/knit-athletic-sneakers-gray.jpg", RatingStars = 4, RatingCount = 89, Keywords = "shoes,running shoes,footwear", Category = GetCategory("Clothing"), Stock = 75 },
            new Product { Name = "Women's Chiffon Beachwear Cover Up - Black", Description = "Lightweight and flowy chiffon cover-up perfect for the beach.", Price = 20.70m, Image = "images/products/women-chiffon-beachwear-coverup-black.jpg", RatingStars = 4.5, RatingCount = 235, Keywords = "robe,swimsuit,swimming,bathing,apparel", Category = GetCategory("Clothing"), Stock = 90 },
            new Product { Name = "Round Sunglasses", Description = "Stylish round sunglasses with UV protection.", Price = 15.60m, Image = "images/products/round-sunglasses-black.jpg", RatingStars = 4.5, RatingCount = 30, Keywords = "accessories,shades", Category = GetCategory("Accessories"), Stock = 50 },
            new Product { Name = "Women's Two Strap Buckle Sandals - Tan", Description = "Comfortable and stylish sandals perfect for the beach.", Price = 24.99m, Image = "images/products/women-beach-sandals.jpg", RatingStars = 4.5, RatingCount = 562, Keywords = "footwear,sandals,womens,beach,summer", Category = GetCategory("Clothing"), Stock = 120 },
            new Product { Name = "Blackout Curtains Set 4-Pack - Beige", Description = "Light-blocking curtains for better sleep and privacy.", Price = 45.99m, Image = "images/products/blackout-curtain-set-beige.webp", RatingStars = 4.5, RatingCount = 232, Keywords = "bedroom,curtains,home", Category = GetCategory("Bedroom"), Stock = 45 },
            new Product { Name = "Men's Slim-Fit Summer Shorts", Description = "Comfortable and stylish shorts perfect for summer.", Price = 16.99m, Image = "images/products/men-slim-fit-summer-shorts-gray.jpg", RatingStars = 4, RatingCount = 160, Keywords = "shorts,apparel,mens", Category = GetCategory("Clothing"), Stock = 110 },
            new Product { Name = "Electric Glass and Steel Hot Tea Water Kettle - 1.7-Liter", Description = "Compact and efficient 2-slot toaster for quick breakfasts.", Price = 30.74m, Image = "images/products/electric-glass-and-steel-hot-water-kettle.webp", RatingStars = 5, RatingCount = 846, Keywords = "water boiler,appliances,kitchen", Category = GetCategory("Kitchen"), Stock = 35 },
            new Product { Name = "Ultra Soft Tissue 2-Ply - 18 Box", Description = "Ultra-soft 2-ply facial tissues for everyday use.", Price = 23.74m, Image = "images/products/facial-tissue-2-ply-18-boxes.jpg", RatingStars = 4, RatingCount = 99, Keywords = "kleenex,tissues,kitchen,tissues box,napkins", Category = GetCategory("Kitchen"), Stock = 200 },
            new Product { Name = "Straw Lifeguard Sun Hat", Description = "Stylish straw sun hat for protection against the sun.", Price = 22.00m, Image = "images/products/straw-sunhat.webp", RatingStars = 4, RatingCount = 215, Keywords = "hats,straw hats,summer,apparel", Category = GetCategory("Accessories"), Stock = 65 },
            new Product { Name = "Sterling Silver Sky Flower Stud Earrings", Description = "Elegant and stylish sterling silver earrings with a floral design.", Price = 17.99m, Image = "images/products/sky-flower-stud-earrings.webp", RatingStars = 4.5, RatingCount = 52, Keywords = "jewelry,accessories,womens", Category = GetCategory("Accessories"), Stock = 40 },
            new Product { Name = "Women's Stretch Popover Hoodie", Description = "Comfortable and stylish cotton t-shirts for everyday wear.", Price = 13.74m, Image = "images/products/women-stretch-popover-hoodie-black.jpg", RatingStars = 4.5, RatingCount = 2465, Keywords = "hooded,hoodies,sweaters,womens,apparel", Category = GetCategory("Clothing"), Stock = 180 },
            new Product { Name = "Bathroom Bath Rug Mat 20 x 31 Inch - Grey", Description = "Soft bathroom rug.", Price = 12.50m, Image = "images/products/bathroom-rug.jpg", RatingStars = 4.5, RatingCount = 119, Keywords = "bathmat,bathroom,home", Category = GetCategory("Bathroom"), Stock = 55 },
            new Product { Name = "Women's Knit Ballet Flat", Description = "Comfortable and stylish ballet flats for everyday wear.", Price = 26.40m, Image = "images/products/women-knit-ballet-flat-black.jpg", RatingStars = 4, RatingCount = 326, Keywords = "shoes,flats,womens,footwear", Category = GetCategory("Clothing"), Stock = 95 },
            new Product { Name = "Men's Regular-Fit Quick-Dry Golf Polo Shirt", Description = "Lightweight and breathable polo shirt for active wear.", Price = 15.99m, Image = "images/products/men-golf-polo-t-shirt-blue.jpg", RatingStars = 4.5, RatingCount = 2556, Keywords = "tshirts,shirts,apparel,mens", Category = GetCategory("Clothing"), Stock = 130 },
            new Product { Name = "Trash Can with Foot Pedal - Brushed Stainless Steel", Description = "Stylish and functional trash can with foot pedal for hands-free operation.", Price = 83.00m, Image = "images/products/trash-can-with-foot-pedal-50-liter.jpg", RatingStars = 4.5, RatingCount = 2286, Keywords = "garbage,bins,cans,kitchen", Category = GetCategory("Kitchen"), Stock = 20 },
            new Product { Name = "Duvet Cover Set with Zipper Closure", Description = "Soft and breathable duvet cover set for a cozy bedroom.", Price = 23.99m, Image = "images/products/duvet-cover-set-blue-twin.jpg", RatingStars = 4, RatingCount = 456, Keywords = "bedroom,bed sheets,sheets,covers,home", Category = GetCategory("Bedroom"), Stock = 60 },
            new Product { Name = "Women's Chunky Cable Beanie - Gray", Description = "Warm and cozy cable-knit beanie for winter wear.", Price = 12.50m, Image = "images/products/women-chunky-beanie-gray.webp", RatingStars = 5, RatingCount = 83, Keywords = "hats,winter hats,beanies,tuques,apparel,womens", Category = GetCategory("Clothing"), Stock = 85 },
            new Product { Name = "Men's Classic-fit Pleated Chino Pants", Description = "Classic-fit chino pants for a timeless look.", Price = 22.90m, Image = "images/products/men-chino-pants-beige.jpg", RatingStars = 4.5, RatingCount = 9017, Keywords = "pants,apparel,mens", Category = GetCategory("Clothing"), Stock = 140 },
            new Product { Name = "Men's Athletic Sneaker", Description = "Comfortable and breathable athletic sneakers perfect for sports and everyday wear.", Price = 38.90m, Image = "images/products/men-athletic-shoes-green.jpg", RatingStars = 4, RatingCount = 229, Keywords = "shoes,running shoes,footwear,mens", Category = GetCategory("Clothing"), Stock = 100 },
            new Product { Name = "Men's Navigator Sunglasses Pilot", Description = "Stylish round sunglasses with UV protection.", Price = 16.90m, Image = "images/products/men-navigator-sunglasses-brown.jpg", RatingStars = 3.5, RatingCount = 42, Keywords = "sunglasses,glasses,accessories,shades", Category = GetCategory("Accessories"), Stock = 50 },
            new Product { Name = "Non-Stick Cookware Set, Pots, Pans and Utensils - 15 Pieces", Description = "Durable and versatile cookware set for all your cooking needs.", Price = 67.97m, Image = "images/products/non-stick-cooking-set-15-pieces.webp", RatingStars = 4.5, RatingCount = 511, Keywords = "cooking set,kitchen", Category = GetCategory("Kitchen"), Stock = 15 },
            new Product { Name = "Vanity Mirror with Heavy Base - Chrome", Description = "Sleek and modern vanity mirror with a heavy base for stability.", Price = 16.49m, Image = "images/products/vanity-mirror-silver.jpg", RatingStars = 4.5, RatingCount = 130, Keywords = "bathroom,washroom,mirrors,home", Category = GetCategory("Bathroom"), Stock = 40 },
            new Product { Name = "Women's Fleece Jogger Sweatpant", Description = "Comfortable and stylish jogger sweatpants for casual wear.", Price = 24.00m, Image = "images/products/women-french-terry-fleece-jogger-camo.jpg", RatingStars = 4.5, RatingCount = 248, Keywords = "pants,sweatpants,jogging,apparel,womens", Category = GetCategory("Clothing"), Stock = 105 },
            new Product { Name = "Double Oval Twist French Wire Earrings - Gold", Description = "Elegant and stylish gold earrings with a twist design.", Price = 24.00m, Image = "images/products/double-elongated-twist-french-wire-earrings.webp", RatingStars = 4.5, RatingCount = 117, Keywords = "accessories,womens", Category = GetCategory("Accessories"), Stock = 55 },
            new Product { Name = "Round Airtight Food Storage Containers - 5 Piece", Description = "Durable and stackable food storage containers for kitchen organization.", Price = 28.99m, Image = "images/products/round-airtight-food-storage-containers.jpg", RatingStars = 4, RatingCount = 126, Keywords = "boxes,food containers,kitchen", Category = GetCategory("Kitchen"), Stock = 70 },
            new Product { Name = "Coffeemaker with Glass Carafe and Reusable Filter - 25 Oz, Black", Description = "Compact and efficient 2-slot toaster for quick breakfasts.", Price = 22.50m, Image = "images/products/coffeemaker-with-glass-carafe-black.jpg", RatingStars = 4.5, RatingCount = 1211, Keywords = "coffeemakers,kitchen,appliances", Category = GetCategory("Kitchen"), Stock = 45 },
            new Product { Name = "Blackout Curtains Set 42 x 84-Inch - Black, 2 Panels", Description = "Light-blocking curtains for better sleep and privacy.", Price = 30.99m, Image = "images/products/blackout-curtains-black.jpg", RatingStars = 4.5, RatingCount = 363, Keywords = "bedroom,home", Category = GetCategory("Bedroom"), Stock = 60 },
            new Product { Name = "100% Cotton Bath Towels - 2 Pack, Light Teal", Description = "Soft and absorbent cotton bath towels for everyday use.", Price = 21.10m, Image = "images/products/cotton-bath-towels-teal.webp", RatingStars = 4.5, RatingCount = 93, Keywords = "bathroom,home,towels", Category = GetCategory("Bathroom"), Stock = 80 },
            new Product { Name = "Waterproof Knit Athletic Sneakers - Pink", Description = "Comfortable and stylish athletic sneakers for active wear.", Price = 33.90m, Image = "images/products/knit-athletic-sneakers-pink.webp", RatingStars = 4, RatingCount = 89, Keywords = "shoes,running shoes,footwear,womens", Category = GetCategory("Clothing"), Stock = 90 },
            new Product { Name = "Countertop Blender - 64oz, 1400 Watts", Description = "Powerful countertop blender with multiple speed settings.", Price = 107.47m, Image = "images/products/countertop-blender-64-oz.jpg", RatingStars = 4, RatingCount = 3, Keywords = "food blenders,kitchen,appliances", Category = GetCategory("Kitchen"), Stock = 20 },
            new Product { Name = "10-Piece Mixing Bowl Set with Lids - Floral", Description = "Versatile mixing bowl set with lids for all your baking needs.", Price = 38.99m, Image = "images/products/floral-mixing-bowl-set.jpg", RatingStars = 5, RatingCount = 679, Keywords = "mixing bowls,baking,cookware,kitchen", Category = GetCategory("Kitchen"), Stock = 50 },
            new Product { Name = "2-Ply Kitchen Paper Towels - 30 Pack", Description = "Highly absorbent kitchen paper towels for everyday use.", Price = 57.99m, Image = "images/products/kitchen-paper-towels-30-pack.jpg", RatingStars = 4.5, RatingCount = 1045, Keywords = "kitchen,kitchen towels,tissues", Category = GetCategory("Kitchen"), Stock = 110 },
            new Product { Name = "Men's Full-Zip Hooded Fleece Sweatshirt", Description = "Cozy and warm fleece sweatshirt for everyday wear.", Price = 24.00m, Image = "images/products/men-cozy-fleece-zip-up-hoodie-red.jpg", RatingStars = 4.5, RatingCount = 3157, Keywords = "sweaters,hoodies,apparel,mens", Category = GetCategory("Clothing"), Stock = 160 }
        );
        context.SaveChanges();
    }
}

app.Run();
