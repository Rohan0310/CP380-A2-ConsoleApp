using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers
                // TODO: add LINQ logic ...
                //       ...

                .Select(c => new
                {
                    Reviews = c.Reviews.Count,
                    Average = (Double)c.Reviews.Average(a => a.stars),
                    Adjust = ratingAdjustmentService.Adjust((Double)c.Reviews.Average(a => a.stars), (Double)c.Reviews.Count),
                    Desc = c.title
                })

                .ToList()
            .OrderByDescending(ratingAdjustmentService => ratingAdjustmentService.Adjust)
            .ToList();
            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];

                Console.WriteLine($"[{j + 1}] {i.Reviews} {Math.Round(i.Average, 2)} {Math.Round(i.Adjust, 2)} {i.Desc}");
                // TODO: add output
                // Console.WriteLine( ... );
            }
            Console.ReadLine();
        }
    }
}
