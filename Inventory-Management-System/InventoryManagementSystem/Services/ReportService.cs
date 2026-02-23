// void ABCAnalysis()
// {
//     var products = _context.Products
//         .OrderByDescending(p => p.Cost)
//         .ToList();

//     int total = products.Count;

//     for (int i = 0; i < total; i++)
//     {
//         if (i < total * 0.2)
//             Console.WriteLine($"A: {products[i].ProductName}");
//         else if (i < total * 0.5)
//             Console.WriteLine($"B: {products[i].ProductName}");
//         else
//             Console.WriteLine($"C: {products[i].ProductName}");
//     }
// }
