using project_context;
using project_entities;
using System.Collections.Generic;
using System.Linq;

namespace projet_API.Data
{
    public class Seeder
    {
        public static void Seed(DataContext context)
        {
            // Seed Categories first
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Sports", Description = "Sports equipment and apparel" },
                    new Category { Name = "Clothing", Description = "Men's and Women's clothing" },
                    new Category { Name = "Kitchen", Description = "Kitchenware and appliances" },
                    new Category { Name = "Bathroom", Description = "Bathroom essentials" },
                    new Category { Name = "Bedroom", Description = "Bedroom furniture and accessories" },
                    new Category { Name = "Home", Description = "General home items" },
                    new Category { Name = "Accessories", Description = "Jewelry, sunglasses, etc." },
                    new Category { Name = "General", Description = "Other items" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            var sportsCat = context.Categories.FirstOrDefault(c => c.Name == "Sports");
            var clothingCat = context.Categories.FirstOrDefault(c => c.Name == "Clothing");
            var kitchenCat = context.Categories.FirstOrDefault(c => c.Name == "Kitchen");
            var bathroomCat = context.Categories.FirstOrDefault(c => c.Name == "Bathroom");
            var bedroomCat = context.Categories.FirstOrDefault(c => c.Name == "Bedroom");
            var homeCat = context.Categories.FirstOrDefault(c => c.Name == "Home");
            var accessoriesCat = context.Categories.FirstOrDefault(c => c.Name == "Accessories");
            var generalCat = context.Categories.FirstOrDefault(c => c.Name == "General");

            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Image = "images/products/athletic-cotton-socks-6-pairs.jpg",
                        Name = "Black and Gray Athletic Cotton Socks - 6 Pairs",
                        Description = "Comfortable and breathable athletic socks perfect for sports and everyday wear.",
                        RatingStars = 4.5,
                        RatingCount = 87,
                        Price = 10.90m,
                        Keywords = "socks,sports,apparel",
                        Stock = 100,
                        IsSolde = false,
                        Category = sportsCat // inferred from sports
                    },
                    new Product
                    {
                        Image = "images/products/intermediate-composite-basketball.jpg",
                        Name = "Intermediate Size Basketball",
                        Description = "Intermediate size basketball suitable for indoor and outdoor play.",
                        RatingStars = 4.0,
                        RatingCount = 127,
                        Price = 20.95m,
                        Keywords = "sports,basketballs",
                        Stock = 50,
                        IsSolde = false,
                        Category = sportsCat
                    },
                    new Product
                    {
                        Image = "images/products/adults-plain-cotton-tshirt-2-pack-teal.jpg",
                        Name = "Adults Plain Cotton T-Shirt - 2 Pack",
                        Description = "Comfortable and stylish cotton t-shirts for everyday wear.",
                        RatingStars = 4.5,
                        RatingCount = 56,
                        Price = 7.99m,
                        Keywords = "tshirts,apparel,mens",
                        Stock = 200,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/black-2-slot-toaster.jpg",
                        Name = "2 Slot Toaster - Black",
                        Description = "Compact and efficient 2-slot toaster for quick breakfasts.",
                        RatingStars = 5.0,
                        RatingCount = 2197,
                        Price = 18.99m,
                        Keywords = "toaster,kitchen,appliances",
                        Stock = 30,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/6-piece-white-dinner-plate-set.jpg",
                        Name = "6 Piece White Dinner Plate Set",
                        Description = "Elegant and durable dinner plate set for everyday use.",
                        RatingStars = 4.0,
                        RatingCount = 37,
                        Price = 20.67m,
                        Keywords = "plates,kitchen,dining",
                        Stock = 40,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/6-piece-non-stick-baking-set.webp",
                        Name = "6-Piece Nonstick, Carbon Steel Oven Bakeware Baking Set",
                        Description = "Versatile and durable nonstick bakeware set for all your baking needs.",
                        RatingStars = 4.5,
                        RatingCount = 175,
                        Price = 34.99m,
                        Keywords = "kitchen,cookware",
                        Stock = 25,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/plain-hooded-fleece-sweatshirt-yellow.jpg",
                        Name = "Plain Hooded Fleece Sweatshirt",
                        Description = "Cozy and warm hooded fleece sweatshirt for casual wear.",
                        RatingStars = 4.5,
                        RatingCount = 317,
                        Price = 24.00m,
                        Keywords = "hoodies,sweaters,apparel",
                        Stock = 150,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/luxury-tower-set-6-piece.jpg",
                        Name = "Luxury Towel Set - Graphite Gray",
                        Description = "Soft and absorbent luxury towel set for a spa-like experience at home.",
                        RatingStars = 4.5,
                        RatingCount = 144,
                        Price = 35.99m,
                        Keywords = "bathroom,washroom,restroom,towels,bath towels",
                        Stock = 60,
                        IsSolde = false,
                        Category = bathroomCat
                    },
                    new Product
                    {
                        Image = "images/products/liquid-laundry-detergent-plain.jpg",
                        Name = "Liquid Laundry Detergent, 110 Loads, 82.5 Fl Oz",
                        Description = "High-efficiency liquid laundry detergent for a deep clean.",
                        RatingStars = 4.5,
                        RatingCount = 305,
                        Price = 28.99m,
                        Keywords = "bathroom,cleaning",
                        Stock = 100,
                        IsSolde = false,
                        Category = bathroomCat
                    },
                    new Product
                    {
                        Image = "images/products/knit-athletic-sneakers-gray.jpg",
                        Name = "Waterproof Knit Athletic Sneakers - Gray",
                        Description = "Comfortable and breathable athletic sneakers perfect for sports and everyday wear.",
                        RatingStars = 4.0,
                        RatingCount = 89,
                        Price = 33.90m,
                        Keywords = "shoes,running shoes,footwear",
                        Stock = 80,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/women-chiffon-beachwear-coverup-black.jpg",
                        Name = "Women's Chiffon Beachwear Cover Up - Black",
                        Description = "Lightweight and flowy chiffon cover-up perfect for the beach.",
                        RatingStars = 4.5,
                        RatingCount = 235,
                        Price = 20.70m,
                        Keywords = "robe,swimsuit,swimming,bathing,apparel",
                        Stock = 90,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/round-sunglasses-black.jpg",
                        Name = "Round Sunglasses",
                        Description = "Stylish round sunglasses with UV protection.",
                        RatingStars = 4.5,
                        RatingCount = 30,
                        Price = 15.60m,
                        Keywords = "accessories,shades",
                        Stock = 120,
                        IsSolde = false,
                        Category = accessoriesCat
                    },
                    new Product
                    {
                        Image = "images/products/women-beach-sandals.jpg",
                        Name = "Women's Two Strap Buckle Sandals - Tan",
                        Description = "Comfortable and stylish sandals perfect for the beach.",
                        RatingStars = 4.5,
                        RatingCount = 562,
                        Price = 24.99m,
                        Keywords = "footwear,sandals,womens,beach,summer",
                        Stock = 75,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/blackout-curtain-set-beige.webp",
                        Name = "Blackout Curtains Set 4-Pack - Beige",
                        Description = "Light-blocking curtains for better sleep and privacy.",
                        RatingStars = 4.5,
                        RatingCount = 232,
                        Price = 45.99m,
                        Keywords = "bedroom,curtains,home",
                        Stock = 45,
                        IsSolde = false,
                        Category = bedroomCat
                    },
                    new Product
                    {
                        Image = "images/products/men-slim-fit-summer-shorts-gray.jpg",
                        Name = "Men's Slim-Fit Summer Shorts",
                        Description = "Comfortable and stylish shorts perfect for summer.",
                        RatingStars = 4.0,
                        RatingCount = 160,
                        Price = 16.99m,
                        Keywords = "shorts,apparel,mens",
                        Stock = 110,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/electric-glass-and-steel-hot-water-kettle.webp",
                        Name = "Electric Glass and Steel Hot Tea Water Kettle - 1.7-Liter",
                        Description = "Compact and efficient 2-slot toaster for quick breakfasts.",
                        RatingStars = 5.0,
                        RatingCount = 846,
                        Price = 30.74m,
                        Keywords = "water boiler,appliances,kitchen",
                        Stock = 65,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/facial-tissue-2-ply-18-boxes.jpg",
                        Name = "Ultra Soft Tissue 2-Ply - 18 Box",
                        Description = "Ultra-soft 2-ply facial tissues for everyday use.",
                        RatingStars = 4.0,
                        RatingCount = 99,
                        Price = 23.74m,
                        Keywords = "kleenex,tissues,kitchen,tissues box,napkins",
                        Stock = 200,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/straw-sunhat.webp",
                        Name = "Straw Lifeguard Sun Hat",
                        Description = "Stylish straw sun hat for protection against the sun.",
                        RatingStars = 4.0,
                        RatingCount = 215,
                        Price = 22.00m,
                        Keywords = "hats,straw hats,summer,apparel",
                        Stock = 85,
                        IsSolde = false,
                        Category = accessoriesCat
                    },
                    new Product
                    {
                        Image = "images/products/sky-flower-stud-earrings.webp",
                        Name = "Sterling Silver Sky Flower Stud Earrings",
                        Description = "Elegant and stylish sterling silver earrings with a floral design.",
                        RatingStars = 4.5,
                        RatingCount = 52,
                        Price = 17.99m,
                        Keywords = "jewelry,accessories,womens",
                        Stock = 40,
                        IsSolde = false,
                        Category = accessoriesCat
                    },
                    new Product
                    {
                        Image = "images/products/women-stretch-popover-hoodie-black.jpg",
                        Name = "Women's Stretch Popover Hoodie",
                        Description = "Comfortable and stylish cotton t-shirts for everyday wear.",
                        RatingStars = 4.5,
                        RatingCount = 2465,
                        Price = 13.74m,
                        Keywords = "hooded,hoodies,sweaters,womens,apparel",
                        Stock = 180,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/bathroom-rug.jpg",
                        Name = "Bathroom Bath Rug Mat 20 x 31 Inch - Grey",
                        Description = "",
                        RatingStars = 4.5,
                        RatingCount = 119,
                        Price = 12.50m,
                        Keywords = "bathmat,bathroom,home",
                        Stock = 100,
                        IsSolde = false,
                        Category = bathroomCat
                    },
                    new Product
                    {
                        Image = "images/products/women-knit-ballet-flat-black.jpg",
                        Name = "Women's Knit Ballet Flat",
                        Description = "Comfortable and stylish ballet flats for everyday wear.",
                        RatingStars = 4.0,
                        RatingCount = 326,
                        Price = 26.40m,
                        Keywords = "shoes,flats,womens,footwear",
                        Stock = 95,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/men-golf-polo-t-shirt-blue.jpg",
                        Name = "Men's Regular-Fit Quick-Dry Golf Polo Shirt",
                        Description = "Lightweight and breathable polo shirt for active wear.",
                        RatingStars = 4.5,
                        RatingCount = 2556,
                        Price = 15.99m,
                        Keywords = "tshirts,shirts,apparel,mens",
                        Stock = 160,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/trash-can-with-foot-pedal-50-liter.jpg",
                        Name = "Trash Can with Foot Pedal - Brushed Stainless Steel",
                        Description = "Stylish and functional trash can with foot pedal for hands-free operation.",
                        RatingStars = 4.5,
                        RatingCount = 2286,
                        Price = 83.00m,
                        Keywords = "garbage,bins,cans,kitchen",
                        Stock = 20,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/duvet-cover-set-blue-twin.jpg",
                        Name = "Duvet Cover Set with Zipper Closure",
                        Description = "Soft and breathable duvet cover set for a cozy bedroom.",
                        RatingStars = 4.0,
                        RatingCount = 456,
                        Price = 23.99m,
                        Keywords = "bedroom,bed sheets,sheets,covers,home",
                        Stock = 55,
                        IsSolde = false,
                        Category = bedroomCat
                    },
                    new Product
                    {
                        Image = "images/products/women-chunky-beanie-gray.webp",
                        Name = "Women's Chunky Cable Beanie - Gray",
                        Description = "Warm and cozy cable-knit beanie for winter wear.",
                        RatingStars = 5.0,
                        RatingCount = 83,
                        Price = 12.50m,
                        Keywords = "hats,winter hats,beanies,tuques,apparel,womens",
                        Stock = 100,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/men-chino-pants-beige.jpg",
                        Name = "Men's Classic-fit Pleated Chino Pants",
                        Description = "Classic-fit chino pants for a timeless look.",
                        RatingStars = 4.5,
                        RatingCount = 9017,
                        Price = 22.90m,
                        Keywords = "pants,apparel,mens",
                        Stock = 200,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/men-athletic-shoes-green.jpg",
                        Name = "Men's Athletic Sneaker",
                        Description = "Comfortable and breathable athletic sneakers perfect for sports and everyday wear.",
                        RatingStars = 4.0,
                        RatingCount = 229,
                        Price = 38.90m,
                        Keywords = "shoes,running shoes,footwear,mens",
                        Stock = 85,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/men-navigator-sunglasses-brown.jpg",
                        Name = "Men's Navigator Sunglasses Pilot",
                        Description = "Stylish round sunglasses with UV protection.",
                        RatingStars = 3.5,
                        RatingCount = 42,
                        Price = 16.90m,
                        Keywords = "sunglasses,glasses,accessories,shades",
                        Stock = 70,
                        IsSolde = false,
                        Category = accessoriesCat
                    },
                    new Product
                    {
                        Image = "images/products/non-stick-cooking-set-15-pieces.webp",
                        Name = "Non-Stick Cookware Set, Pots, Pans and Utensils - 15 Pieces",
                        Description = "Durable and versatile cookware set for all your cooking needs.",
                        RatingStars = 4.5,
                        RatingCount = 511,
                        Price = 67.97m,
                        Keywords = "cooking set,kitchen",
                        Stock = 15,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/vanity-mirror-silver.jpg",
                        Name = "Vanity Mirror with Heavy Base - Chrome",
                        Description = "Sleek and modern vanity mirror with a heavy base for stability.",
                        RatingStars = 4.5,
                        RatingCount = 130,
                        Price = 16.49m,
                        Keywords = "bathroom,washroom,mirrors,home",
                        Stock = 60,
                        IsSolde = false,
                        Category = bathroomCat
                    },
                    new Product
                    {
                        Image = "images/products/women-french-terry-fleece-jogger-camo.jpg",
                        Name = "Women's Fleece Jogger Sweatpant",
                        Description = "Comfortable and stylish jogger sweatpants for casual wear.",
                        RatingStars = 4.5,
                        RatingCount = 248,
                        Price = 24.00m,
                        Keywords = "pants,sweatpants,jogging,apparel,womens",
                        Stock = 90,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/double-elongated-twist-french-wire-earrings.webp",
                        Name = "Double Oval Twist French Wire Earrings - Gold",
                        Description = "Elegant and stylish gold earrings with a twist design.",
                        RatingStars = 4.5,
                        RatingCount = 117,
                        Price = 24.00m,
                        Keywords = "accessories,womens",
                        Stock = 50,
                        IsSolde = false,
                        Category = accessoriesCat
                    },
                    new Product
                    {
                        Image = "images/products/round-airtight-food-storage-containers.jpg",
                        Name = "Round Airtight Food Storage Containers - 5 Piece",
                        Description = "Durable and stackable food storage containers for kitchen organization.",
                        RatingStars = 4.0,
                        RatingCount = 126,
                        Price = 28.99m,
                        Keywords = "boxes,food containers,kitchen",
                        Stock = 80,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/coffeemaker-with-glass-carafe-black.jpg",
                        Name = "Coffeemaker with Glass Carafe and Reusable Filter - 25 Oz, Black",
                        Description = "Compact and efficient 2-slot toaster for quick breakfasts.",
                        RatingStars = 4.5,
                        RatingCount = 1211,
                        Price = 22.50m,
                        Keywords = "coffeemakers,kitchen,appliances",
                        Stock = 45,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/blackout-curtains-black.jpg",
                        Name = "Blackout Curtains Set 42 x 84-Inch - Black, 2 Panels",
                        Description = "Light-blocking curtains for better sleep and privacy.",
                        RatingStars = 4.5,
                        RatingCount = 363,
                        Price = 30.99m,
                        Keywords = "bedroom,home",
                        Stock = 55,
                        IsSolde = false,
                        Category = bedroomCat
                    },
                    new Product
                    {
                        Image = "images/products/cotton-bath-towels-teal.webp",
                        Name = "100% Cotton Bath Towels - 2 Pack, Light Teal",
                        Description = "Soft and absorbent cotton bath towels for everyday use.",
                        RatingStars = 4.5,
                        RatingCount = 93,
                        Price = 21.10m,
                        Keywords = "bathroom,home,towels",
                        Stock = 120,
                        IsSolde = false,
                        Category = bathroomCat
                    },
                    new Product
                    {
                        Image = "images/products/knit-athletic-sneakers-pink.webp",
                        Name = "Waterproof Knit Athletic Sneakers - Pink",
                        Description = "Comfortable and stylish athletic sneakers for active wear.",
                        RatingStars = 4.0,
                        RatingCount = 89,
                        Price = 33.90m,
                        Keywords = "shoes,running shoes,footwear,womens",
                        Stock = 75,
                        IsSolde = false,
                        Category = clothingCat
                    },
                    new Product
                    {
                        Image = "images/products/countertop-blender-64-oz.jpg",
                        Name = "Countertop Blender - 64oz, 1400 Watts",
                        Description = "Powerful countertop blender with multiple speed settings.",
                        RatingStars = 4.0,
                        RatingCount = 3,
                        Price = 107.47m,
                        Keywords = "food blenders,kitchen,appliances",
                        Stock = 10,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/floral-mixing-bowl-set.jpg",
                        Name = "10-Piece Mixing Bowl Set with Lids - Floral",
                        Description = "Versatile mixing bowl set with lids for all your baking needs.",
                        RatingStars = 5.0,
                        RatingCount = 679,
                        Price = 38.99m,
                        Keywords = "mixing bowls,baking,cookware,kitchen",
                        Stock = 35,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/kitchen-paper-towels-30-pack.jpg",
                        Name = "2-Ply Kitchen Paper Towels - 30 Pack",
                        Description = "Highly absorbent kitchen paper towels for everyday use.",
                        RatingStars = 4.5,
                        RatingCount = 1045,
                        Price = 57.99m,
                        Keywords = "kitchen,kitchen towels,tissues",
                        Stock = 200,
                        IsSolde = false,
                        Category = kitchenCat
                    },
                    new Product
                    {
                        Image = "images/products/men-cozy-fleece-zip-up-hoodie-red.jpg",
                        Name = "Men's Full-Zip Hooded Fleece Sweatshirt",
                        Description = "Cozy and warm fleece sweatshirt for everyday wear.",
                        RatingStars = 4.5,
                        RatingCount = 3157,
                        Price = 24.00m,
                        Keywords = "sweaters,hoodies,apparel,mens",
                        Stock = 150,
                        IsSolde = false,
                        Category = clothingCat
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
